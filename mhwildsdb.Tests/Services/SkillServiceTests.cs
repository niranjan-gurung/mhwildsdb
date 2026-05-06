using FluentAssertions;
using mhwildsdb.DTOs.Skills.Skill;
using mhwildsdb.Entities.Skills;
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
        new List<CreateSkillRankDto>
        {
            new(1, "Attack +3")
        });

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
    public async Task CreateSkillsAsync_WithDuplicateName_ShouldReturnConflictException()
    {
        // seed existing skill
        _context.Skills.Add(Skill.Create("Attack Boost", "weapon", "Increase attack power.", 
            new List<SkillRank>
            {
                SkillRank.Create(1, "Attack +3")
            }));

        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);

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
