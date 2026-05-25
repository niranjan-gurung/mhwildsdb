using FluentAssertions;
using mhwildsdb.DTOs.Charms;
using mhwildsdb.Entities.Skills;
using mhwildsdb.Exceptions;
using mhwildsdb.Persistance;
using mhwildsdb.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace mhwildsdb.Tests.Services;

public class CharmServiceTests
{
    private readonly MhwildsDbContext _context;
    private readonly CharmService _service;
    private Guid _skillRankId;

    private CreateCharmDto ValidCharm => new(
        "Attack Charm",
        [
            new("Attack Charm I", "Attack +4", 1, 3, [_skillRankId]),
            new("Attack Charm II", "Attack +8", 2, 5, [_skillRankId]),
            new("Attack Charm III", "Attack +12", 3, 7, [_skillRankId])
        ]);

    private CreateCharmDto ValidCharmNoSkills => new(
        "Blank Charm",
        [new("Blank Charm I", "No effect.", 1, 1, [])]);

    public CharmServiceTests()
    {
        var options = new DbContextOptionsBuilder<MhwildsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new MhwildsDbContext(options);
        _service = new CharmService(_context, NSubstitute.Substitute.For<ILogger<CharmService>>());

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
    public async Task CreateCharmAsync_WithValidData_ShouldReturnCharmDto()
    {
        var result = await _service.CreateCharmAsync(ValidCharm);

        result.Should().NotBeNull();
        result.Name.Should().Be(ValidCharm.Name);
        result.Ranks.Should().HaveCount(ValidCharm.Ranks.Count);
    }

    [Fact]
    public async Task CreateCharmAsync_WithNoSkills_ShouldReturnCharmDtoWithEmptySkills()
    {
        var result = await _service.CreateCharmAsync(ValidCharmNoSkills);

        result.Should().NotBeNull();
        result.Ranks[0].Skills.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateCharmAsync_RanksShouldContainSkillRankDetails()
    {
        var result = await _service.CreateCharmAsync(ValidCharm);

        result.Ranks[0].Skills.Should().HaveCount(1);
        result.Ranks[0].Skills[0].Id.Should().Be(_skillRankId);
    }

    [Fact]
    public async Task CreateCharmAsync_WithDuplicateName_ShouldThrowConflictException()
    {
        await _service.CreateCharmAsync(ValidCharm);

        var act = async () => await _service.CreateCharmAsync(ValidCharm);
        await act.Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task CreateCharmAsync_WithNonExistentSkillRankId_ShouldThrowNotFoundException()
    {
        var invalidCharm = new CreateCharmDto(
            "Invalid Charm",
            [new("Invalid Charm I", "Some description.", 1, 3, [Guid.NewGuid()])]);

        var act = async () => await _service.CreateCharmAsync(invalidCharm);
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateCharmRangeAsync_WithValidData_ShouldReturnCharmDtoList()
    {
        ICollection<CreateCharmDto> charms =
        [
            ValidCharm,
            new("Vitality Charm",
            [
                new("Vitality Charm I", "HP +10", 1, 3, [_skillRankId]),
                new("Vitality Charm II", "HP +20", 2, 5, [_skillRankId])
            ])
        ];

        var result = await _service.CreateCharmRangeAsync(charms);

        result.Should().HaveCount(2);
        result.Select(c => c.Name).Should().BeEquivalentTo(charms.Select(c => c.Name));
    }

    [Fact]
    public async Task CreateCharmRangeAsync_WithDuplicateName_ShouldThrowConflictException()
    {
        await _service.CreateCharmAsync(ValidCharm);

        var act = async () => await _service.CreateCharmRangeAsync([ValidCharm]);
        await act.Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task GetCharmByIdAsync_WithNonExistentId_ShouldThrowNotFoundException()
    {
        var act = async () => await _service.GetCharmByIdAsync(Guid.NewGuid());
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task UpdateCharmAsync_WithNonExistentId_ShouldThrowNotFoundException()
    {
        var act = async () => await _service.UpdateCharmAsync(Guid.NewGuid(), new UpdateCharmDto("New Name"));
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task DeleteCharmAsync_WithNonExistentId_ShouldThrowNotFoundException()
    {
        var act = async () => await _service.DeleteCharmAsync(Guid.NewGuid());
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
