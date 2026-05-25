using FluentAssertions;
using mhwildsdb.DTOs.Decorations;
using mhwildsdb.Entities.Skills;
using mhwildsdb.Exceptions;
using mhwildsdb.Persistance;
using mhwildsdb.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace mhwildsdb.Tests.Services;

public class DecorationServiceTests
{
    private readonly MhwildsDbContext _context;
    private readonly DecorationService _service;
    private Guid _skillRankId;

    private CreateDecorationDto ValidDecoration =>
        new("Attack Jewel", "Grants Attack Boost.", "Weapon", 3, 1, [_skillRankId]);

    private ICollection<CreateDecorationDto> ValidDecorations =>
    [
        new("Attack Jewel", "Grants Attack Boost.", "Weapon", 3, 1, [_skillRankId]),
        new("Defense Jewel", "Grants Defense Boost.", "Armor", 3, 1, [_skillRankId]),
        new("Vitality Jewel", "Grants Health Boost.", "Armor", 4, 2, [_skillRankId])
    ];

    public DecorationServiceTests()
    {
        var options = new DbContextOptionsBuilder<MhwildsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new MhwildsDbContext(options);
        _service = new DecorationService(_context, NSubstitute.Substitute.For<ILogger<DecorationService>>());

        SeedDatabase();
    }

    private void SeedDatabase()
    {
        var skill = Skill.Create("Attack Boost", "weapon",
            [SkillRank.Create(1, "Attack +3")], "Increases attack power.");

        _context.Skills.Add(skill);
        _context.SaveChanges();

        _skillRankId = _context.SkillRanks.First().Id;
    }

    [Fact]
    public async Task CreateDecorationAsync_WithValidData_ShouldReturnDecorationDto()
    {
        var result = await _service.CreateDecorationAsync(ValidDecoration);

        result.Should().NotBeNull();
        result.Name.Should().Be(ValidDecoration.Name);
        result.Skills.Should().HaveCount(1);
        result.Skills.First().Id.Should().Be(_skillRankId);
    }

    [Fact]
    public async Task CreateDecorationAsync_WithDuplicateName_ShouldThrowConflictException()
    {
        await _service.CreateDecorationAsync(ValidDecoration);

        var act = async () => await _service.CreateDecorationAsync(ValidDecoration);
        await act.Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task CreateDecorationAsync_WithNonExistentSkillRankId_ShouldThrowNotFoundException()
    {
        var invalid = new CreateDecorationDto(
            "Attack Jewel", "Grants Attack Boost.", "Weapon", 3, 1, [Guid.NewGuid()]);

        var act = async () => await _service.CreateDecorationAsync(invalid);
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateDecorationRangeAsync_WithValidData_ShouldReturnDecorationDtoList()
    {
        var result = await _service.CreateDecorationRangeAsync(ValidDecorations);

        result.Should().HaveCount(ValidDecorations.Count);
        result.Select(d => d.Name).Should()
            .BeEquivalentTo(ValidDecorations.Select(d => d.Name));
    }

    [Fact]
    public async Task GetDecorationByIdAsync_WithNonExistentId_ShouldThrowNotFoundException()
    {
        var act = async () => await _service.GetDecorationByIdAsync(Guid.NewGuid());
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task DeleteDecorationAsync_WithNonExistentId_ShouldThrowNotFoundException()
    {
        var act = async () => await _service.DeleteDecorationAsync(Guid.NewGuid());
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
