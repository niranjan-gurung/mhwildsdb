using FluentAssertions;
using mhwildsdb.DTOs.Talismans.CharmRank;
using mhwildsdb.Validators.CharmValidators;

namespace mhwildsdb.Tests.Validators.Charms;

public class CreateCharmRankDtoValidatorTests
{
    private readonly CreateCharmRankDtoValidator _validator = new();
    private readonly Guid _skillRankId = Guid.NewGuid();

    [Fact]
    public async Task Validate_WithValidData_ShouldPass()
    {
        var dto = new CreateCharmRankDto("Attack Charm I", "Increases attack.", 1, 3, [_skillRankId]);
        
        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_WithEmptySkills_ShouldFail()
    {
        // charms cannot have empty skill rank collections
        var dto = new CreateCharmRankDto("Attack Charm I", "Increases attack.", 1, 3, []);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("", "Some description", 1, 3)]              // empty name
    [InlineData("Attack123", "Some description", 1, 3)]     // name with numbers
    [InlineData("Attack Charm I", "", 1, 3)]                // empty description
    public async Task Validate_WithInvalidStringFields_ShouldFail(
        string name, string description, int level, int rarity)
    {
        var dto = new CreateCharmRankDto(name, description, level, rarity, [_skillRankId]);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(0)]   // below minimum
    [InlineData(6)]   // above maximum
    public async Task Validate_WithInvalidLevel_ShouldFail(int level)
    {
        var dto = new CreateCharmRankDto("Attack Charm I", "Increases attack.", level, 3, [_skillRankId]);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateCharmRankDto.Level));
    }

    [Theory]
    [InlineData(0)]   // below minimum
    [InlineData(13)]  // above maximum
    public async Task Validate_WithInvalidRarity_ShouldFail(int rarity)
    {
        var dto = new CreateCharmRankDto("Attack Charm I", "Increases attack.", 1, rarity, [_skillRankId]);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateCharmRankDto.Rarity));
    }

    [Fact]
    public async Task Validate_WithDuplicateSkillRankIds_ShouldFail()
    {
        var dto = new CreateCharmRankDto("Attack Charm I", "Increases attack.", 1, 3,
            [_skillRankId, _skillRankId]);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should()
            .Contain(e => e.PropertyName == nameof(CreateCharmRankDto.Skills));
    }

    [Fact]
    public async Task Validate_WithEmptyGuidSkillRankId_ShouldFail()
    {
        var dto = new CreateCharmRankDto("Attack Charm I", "Increases attack.", 1, 3, [Guid.Empty]);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);
        result.IsValid.Should().BeFalse();
    }
}
