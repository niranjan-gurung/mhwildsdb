using FluentAssertions;
using mhwildsdb.DTOs.Skills;
using mhwildsdb.Exceptions;
using mhwildsdb.Persistance;
using mhwildsdb.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace mhwildsdb.Tests.Services;

public class SkillServiceTests
{
    private readonly MhwildsDbContext _context;
    private readonly SkillService _service;

    private readonly CreateSkillDto _validSkill = new("Attack Boost", "weapon", "Increase attack power.",
        [ new(1, "Attack +3")] );

    private readonly ICollection<CreateSkillDto> _validSkills =
        [
            new("Attack Boost", "weapon", "Increase attack power.",
                [
                    new(1, "Attack +3"),
                    new(2, "Attack +5"),
                    new(3, "Attack +7"),
                    new(4, "Attack +2% Bonus: +8"),
                    new(5, "Attack +4% Bonus: +9")
                ]),
            new("Offensive Guard", "weapon", "Temporarily increases attack power after executing a perfectly-timed guard.",
                [
                    new(1, "Attack +5% while active."),
                    new(2, "Attack +10% while active"),
                    new(3, "Attack +15% while active")
                ]),
            new("Critical Eye", "weapon", "Increases affinity.",
                [
                    new(1, "Affinity +4%"),
                    new(2, "Affinity +8%"),
                    new(3, "Affinity +12%"),
                    new(4, "Affinity +16%"),
                    new(5, "Affinity +20%")
                ])
        ];

    public SkillServiceTests()
    {
        var options = new DbContextOptionsBuilder<MhwildsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new MhwildsDbContext(options);
        _service = new SkillService(_context, NSubstitute.Substitute.For<ILogger<SkillService>>());
    }

    [Fact]
    public async Task CreateSkillAsync_WithValidData_ShouldReturnSkillDto()
    {
        var result = await _service.CreateSkillAsync(_validSkill);

        result.Should().NotBeNull();
        result.Name.Should().Be(_validSkill.Name);
        result.Ranks.Should().HaveCount(_validSkill.Ranks.Count);
    }

    [Fact]
    public async Task CreateSkillRangeAsync_WithValidData_ShouldReturnSkillDtoList()
    {
        var result = await _service.CreateSkillRangeAsync(_validSkills);

        result.Should().NotBeNull();
        result.Select(s => s.Name).Should()
            .BeEquivalentTo(_validSkills.Select(s => s.Name));
    }

    [Fact]
    public async Task CreateSkillRangeAsync_WithDuplicateName_ShouldReturnConflictException()
    {
        // seed existing skill
        await _service.CreateSkillAsync(_validSkill);

        var result = async () => await _service.CreateSkillRangeAsync(_validSkills);
        await result.Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task CreateSkillAsync_WithDuplicateName_ShouldReturnConflictException()
    {
        // seed existing skill
        await _service.CreateSkillAsync(_validSkill);

        var result = async () => await _service.CreateSkillAsync(_validSkill);
        await result.Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task GetSkillByIdAsync_WithNonExistingId_ShouldReturnNotFoundException()
    {
        var result = async () => await _service.GetSkillByIdAsync(Guid.NewGuid());
        await result.Should().ThrowAsync<NotFoundException>();
    }
}
