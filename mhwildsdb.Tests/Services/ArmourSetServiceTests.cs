using FluentAssertions;
using mhwildsdb.DTOs.Armours.ArmourSet;
using mhwildsdb.Entities;
using mhwildsdb.Entities.Armours;
using mhwildsdb.Entities.Skills;
using mhwildsdb.Exceptions;
using mhwildsdb.Persistance;
using mhwildsdb.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace mhwildsdb.Tests.Services;

public class ArmourSetServiceTests
{
    private readonly MhwildsDbContext _context;
    private readonly ArmourSetService _service;

    private Guid _armourPieceId1;
    private Guid _armourPieceId2;
    private Guid _setBonusSkillId;
    private Guid _groupBonusSkillId;

    // valid set: includes set bonus + group skill 
    private CreateArmourSetDto ValidArmourSet => new(
        "Doshaguma",
        [_armourPieceId1, _armourPieceId2],
        _setBonusSkillId,
        _groupBonusSkillId);

    // valid set: no set bonus or group skill
    private CreateArmourSetDto ValidArmourSetNoBonusOrGroup => new(
        "Conga α",
        [_armourPieceId1],
        null,
        null);

    public ArmourSetServiceTests()
    {
        var options = new DbContextOptionsBuilder<MhwildsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new MhwildsDbContext(options);
        _service = new ArmourSetService(_context, NSubstitute.Substitute.For<ILogger<ArmourSetService>>());

        SeedDatabase();
    }

    private void SeedDatabase()
    {
        // seed skill + skill ranks
        var skill1 = Skill.Create("Doshaguma's Might", "set",
            [SkillRank.Create(
                1, 
                "Temporarily grants attack +10 after a successful Power Clash or Offset attack", 
                "Powerhouse I", 
                2),
             SkillRank.Create(
                 2, 
                 "Temporarily grants attack +25 after a successful Power Clash or Offset attack", 
                 "Powerhouse II", 
                 4)
             ]);

        var skill2 = Skill.Create("Fortifying Pelt", "group",
            [SkillRank.Create(
                1,
                "Increases attack and defense after fainting during a quest. (Can be used twice.)",
                "Fortify",
                3)
            ]);

        _context.Skills.AddRange(skill1, skill2);
        _context.SaveChanges();

        _setBonusSkillId = skill1.Id;
        _groupBonusSkillId = skill2.Id;

        var skill1Rank1 = _context.SkillRanks.First(sr => sr.SkillId == skill1.Id && sr.Level == 1);
        var skill2Rank1 = _context.SkillRanks.First(sr => sr.SkillId == skill2.Id && sr.Level == 1);

        var armour1 = Armour.Create("Doshaguma Helm", "head", "low", 3, 24,
            new Resistances(0, 0, 0, 0, 0), [],
            [skill1Rank1, skill2Rank1]);

        var armour2 = Armour.Create("Doshaguma Mail", "chest", "low", 3, 24,
            new Resistances(0, 0, 0, 0, 0), [],
            [skill1Rank1, skill2Rank1]);

        _context.Armours.AddRange(armour1, armour2);
        _context.SaveChanges();

        _armourPieceId1 = armour1.Id;
        _armourPieceId2 = armour2.Id;
    }

    [Fact]
    public async Task CreateArmourSetAsync_WithValidData_ShouldReturnArmourSetDto()
    {
        var result = await _service.CreateArmourSetAsync(ValidArmourSet);

        result.Should().NotBeNull();
        result.Name.Should().Be(ValidArmourSet.Name);
        result.Pieces.Should().HaveCount(ValidArmourSet.ArmourPieceIds.Count);
    }

    [Fact]
    public async Task CreateArmourSetAsync_WithNoBonusSkill_ShouldReturnArmourSetDtoWithNullSkills()
    {
        var result = await _service.CreateArmourSetAsync(ValidArmourSetNoBonusOrGroup);

        result.Should().NotBeNull();
        result.SetBonusSkill.Should().BeNull();
        result.GroupBonusSkill.Should().BeNull();
    }

    [Fact]
    public async Task CreateArmourSetAsync_WithValidBonusSkill_ShouldReturnArmourSetDtoWithBonusAndGroupSkills()
    {
        var result = await _service.CreateArmourSetAsync(ValidArmourSet);

        result.SetBonusSkill.Should().NotBeNull();
        result.SetBonusSkill!.Id.Should().Be(_setBonusSkillId);
        result.GroupBonusSkill.Should().NotBeNull();
        result.GroupBonusSkill!.Id.Should().Be(_groupBonusSkillId);
    }

    [Fact]
    public async Task CreateArmourSetAsync_WithDuplicateName_ShouldThrowConflictException()
    {
        await _service.CreateArmourSetAsync(ValidArmourSet);
        var act = async () => await _service.CreateArmourSetAsync(ValidArmourSet);
        await act.Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task CreateArmourSetAsync_WithNonExistentArmourPieceId_ShouldThrowNotFoundException()
    {
        var invalidRequest = new CreateArmourSetDto(
            "Invalid Set",
            [Guid.NewGuid()],
            null,
            null);

        var act = async () => await _service.CreateArmourSetAsync(invalidRequest);
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateArmourSetAsync_WithNonExistentBonusSkillId_ShouldThrowNotFoundException()
    {
        var invalidRequest = new CreateArmourSetDto(
            "Invalid Set",
            [_armourPieceId1],
            Guid.NewGuid(),
            null);

        var act = async () => await _service.CreateArmourSetAsync(invalidRequest);
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetArmourSetByIdAsync_WithNonExistentId_ShouldThrowNotFoundException()
    {
        var act = async () => await _service.GetArmourSetByIdAsync(Guid.NewGuid());
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task UpdateArmourSetAsync_WithNonExistentId_ShouldThrowNotFoundException()
    {
        var act = async () => await _service.UpdateArmourSetAsync(
            Guid.NewGuid(),
            new UpdateArmourSetDto("New Name"));

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task DeleteArmourSetAsync_ShouldNullifyArmourPieceSetId()
    {
        var created = await _service.CreateArmourSetAsync(ValidArmourSet);

        await _service.DeleteArmourSetAsync(created.Id);

        // armour pieces should still exist but with null ArmourSetId
        var armour = await _context.Armours.FindAsync(
            [_armourPieceId1], 
            TestContext.Current.CancellationToken);

        armour!.ArmourSetId.Should().BeNull();
    }
}
