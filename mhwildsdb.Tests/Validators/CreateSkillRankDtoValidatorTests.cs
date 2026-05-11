using FluentAssertions;
using mhwildsdb.DTOs.Skills.SkillRank;
using mhwildsdb.Validators.SkillValidators;

namespace mhwildsdb.Tests.Validators;

public class CreateSkillRankDtoValidatorTests
{
    private readonly CreateSkillRankDtoValidator _validator = new();

    [Fact]
    public async Task Validate_WithValidData_ShouldPass()
    {
        var dto = new CreateSkillRankDto(1, "Attack +3");
        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]   // valid range is 1 - 5
    [InlineData(-1)]
    [InlineData(7)]
    public async Task Validate_WithInvalidLevel_ShouldFail(int level)
    {
        var dto = new CreateSkillRankDto(level, $"Attack +{level}");

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Level");
    }

    [Fact]
    public async Task Validate_WithEmptyDescription_ShouldFail()
    {
        var dto = new CreateSkillRankDto(1, "");

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Description");
    }

    [Fact]
    public async Task Validate_WithDescriptionExceedingMaxLength_ShouldFail()
    {
        var dto = new CreateSkillRankDto(1, new string('a', 201));

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Description");
    }
}
