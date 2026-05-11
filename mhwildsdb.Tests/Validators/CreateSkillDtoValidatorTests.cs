using FluentAssertions;
using mhwildsdb.DTOs.Skills.Skill;
using mhwildsdb.DTOs.Skills.SkillRank;
using mhwildsdb.Validators.SkillValidators;

namespace mhwildsdb.Tests.Validators;

public class CreateSkillDtoValidatorTests
{
    private readonly CreateSkillDtoValidator _validator = new();
    private readonly List<CreateSkillRankDto> _validRanks =
    [
        new(1, "Attack +3"),
        new(2, "Attack +6")
    ];
    private readonly List<CreateSkillRankDto> _invalidRanks =
    [
        new(1, "Attack +3"),
        new(1, "Attack +6")
    ];

    [Fact]
    public async Task Validate_WithValidData_ShouldPass()
    {
        var dto = new CreateSkillDto("Attack Boost", "weapon", "Increase attack power.", _validRanks);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        // assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "weapon", "some descrtiption")]                     // missing name field
    [InlineData("AttackBoost", "armourWeapon", "some descrtiption")]    // type field exceeds length 6
    [InlineData("AttackBoost123", "weapon", "some descrtiption")]       // name field contains non char types
    public async Task Validate_WithInvalidFields_ShouldFail(string name, string type, string description)
    {
        var dto = new CreateSkillDto(name, type, description, _validRanks);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task Validate_WithDuplicateLevels_ShouldFail()
    {
        var dto = new CreateSkillDto("Attack Boost", "weapon", "Increases attack power.", _invalidRanks);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Ranks must have unique levels.");
    }
}
