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
    
    private CreateArmourDto _validArmour => 
        new("Conga Helm α", "head", "high", 5, 36, _validResistances, [1], [_skillRankId]);
    
    private CreateArmourDto _invalidArmour =>
        new("Conga Helm α", "head", "high", 5, 36, _validResistances, [1], [Guid.NewGuid()]);

    private ICollection<CreateArmourDto> _validArmours =>
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
        var skill = Skill.Create("Attack Boost", "Weapon", "Increases attack power.",
            [SkillRank.Create(1, "Attack +3")]);

        _context.Skills.Add(skill);
        _context.SaveChanges();

        _skillRankId = _context.SkillRanks.First().Id;
    }

    [Fact]
    public async Task CreateArmourAsync_WithValidData_ShouldReturnArmourDto()
    {
        var result = await _service.CreateArmourAsync(_validArmour);

        result.Should().NotBeNull();
        result.Name.Should().Be(_validArmour.Name);
        result.Skills.Should().HaveCount(_validArmour.SkillRankIds.Count);
    }

    [Fact]
    public async Task CreateArmourRangeAsync_WithValidData_ShouldReturnArmourDtoList()
    {
        var result = await _service.CreateArmourRangeAsync(_validArmours);

        result.Should().NotBeNull();
        result.Select(a => a.Name).Should()
            .BeEquivalentTo(_validArmours.Select(a => a.Name));
    }

    [Fact]
    public async Task CreateArmourAsync_WithDuplicateName_ShouldThrowConflictException()
    {
        await _service.CreateArmourAsync(_validArmour);

        var act = async () => await _service.CreateArmourAsync(_validArmour);
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
        var act = async () => await _service.CreateArmourAsync(_invalidArmour);
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
