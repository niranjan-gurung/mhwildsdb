using FluentAssertions;
using mhwildsdb.DTOs;
using mhwildsdb.DTOs.Armours;
using mhwildsdb.Entities.Skills;
using mhwildsdb.Exceptions;
using mhwildsdb.Persistance;
using mhwildsdb.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace mhwildsdb.Tests.Services;

public class ArmourServiceTests
{
    private readonly MhwildsDbContext _context;
    private readonly ArmourService _service;
    private static readonly ResistancesDto _validResistances = new(-3, 1, -1, 1, 2);
    private Guid _skillRankId;
    
    private CreateArmourDto ValidArmour => 
        new("Conga Helm α", "head", "high", 5, 36, _validResistances, [1], [_skillRankId]);
    
    private CreateArmourDto InvalidArmour =>
        new("Conga Helm α", "head", "high", 5, 36, _validResistances, [1], [Guid.NewGuid()]);

    private ICollection<CreateArmourDto> ValidArmours =>
        [
            new("Conga Helm α", "head", "high", 5, 36, _validResistances, [1], [_skillRankId]),
            new("Conga Mail α", "chest", "high", 5, 36, _validResistances, [], [_skillRankId]),
            new("Conga Vambraces α", "arms", "high", 5, 36, _validResistances, [], [_skillRankId])
        ];

    public ArmourServiceTests()
    {
        var options = new DbContextOptionsBuilder<MhwildsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new MhwildsDbContext(options);
        _service = new ArmourService(_context, NSubstitute.Substitute.For<ILogger<ArmourService>>());

        SeedInMemoryDb();
    }

    private void SeedInMemoryDb()
    {
        var skill = Skill.Create("Attack Boost", "Weapon", 
            [SkillRank.Create(1, "Attack +3")], "Increases attack power.");

        _context.Skills.Add(skill);
        _context.SaveChanges();

        _skillRankId = _context.SkillRanks.First().Id;
    }

    [Fact]
    public async Task CreateArmourAsync_WithValidData_ShouldReturnArmourDto()
    {
        var result = await _service.CreateArmourAsync(ValidArmour);

        result.Should().NotBeNull();
        result.Name.Should().Be(ValidArmour.Name);
        result.Skills.Should().HaveCount(ValidArmour.SkillRankIds.Count);
    }

    [Fact]
    public async Task CreateArmourRangeAsync_WithValidData_ShouldReturnArmourDtoList()
    {
        var result = await _service.CreateArmourRangeAsync(ValidArmours);

        result.Should().NotBeNull();
        result.Select(a => a.Name).Should()
            .BeEquivalentTo(ValidArmours.Select(a => a.Name));
    }

    [Fact]
    public async Task CreateArmourAsync_WithDuplicateName_ShouldThrowConflictException()
    {
        await _service.CreateArmourAsync(ValidArmour);

        var act = async () => await _service.CreateArmourAsync(ValidArmour);
        await act.Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task GetArmourByIdAsync_WithNonExistentId_ShouldThrowNotFoundException()
    {
        var act = async () => await _service.GetArmourByIdAsync(Guid.NewGuid());
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateArmourAsync_WithNonExistentSkillRankId_ShouldThrowNotFoundException()
    {
        var act = async () => await _service.CreateArmourAsync(InvalidArmour);
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
