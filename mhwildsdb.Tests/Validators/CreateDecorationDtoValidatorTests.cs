using FluentAssertions;
using mhwildsdb.DTOs.Decorations;
using mhwildsdb.Validators.DecorationValidators;

namespace mhwildsdb.Tests.Validators;

public class CreateDecorationDtoValidatorTests
{
    private readonly CreateDecorationDtoValidator _validator = new();
    private readonly Guid _skillRankId = Guid.NewGuid();

    [Fact]
    public async Task Validate_WithValidData_ShouldPass()
    {
        var dto = new CreateDecorationDto("Attack Jewel", "Grants Attack Boost.", "Weapon", 3, 1, [_skillRankId]);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "Grants Attack Boost.", "Weapon")]                      // empty name
    [InlineData("Attack123", "Grants Attack Boost.", "Weapon")]             // name with numbers
    [InlineData("Attack Jewel", "", "Weapon")]                              // empty description
    [InlineData("Attack Jewel", "Grants Attack Boost.", "")]                // empty type
    [InlineData("Attack Jewel", "Grants Attack Boost.", "WeaponArmour")]    // type exceeds 10 chars
    public async Task Validate_WithInvalidStringFields_ShouldFail(
        string name, string description, string type)
    {
        var dto = new CreateDecorationDto(name, description, type, 3, 1, [_skillRankId]);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(0)]     // below minimum
    [InlineData(13)]    // above maximum
    public async Task Validate_WithInvalidRarity_ShouldFail(int rarity)
    {
        var dto = new CreateDecorationDto("Attack Jewel", "Grants Attack Boost.", "Weapon", rarity, 1, [_skillRankId]);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateDecorationDto.Rarity));
    }

    [Theory]
    [InlineData(0)]   // below minimum
    [InlineData(4)]   // above maximum
    public async Task Validate_WithInvalidSlot_ShouldFail(int slot)
    {
        var dto = new CreateDecorationDto("Attack Jewel", "Grants Attack Boost.", "Weapon", 3, slot, [_skillRankId]);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateDecorationDto.Slot));
    }

    [Fact]
    public async Task Validate_WithEmptySkills_ShouldFail()
    {
        var dto = new CreateDecorationDto("Attack Jewel", "Grants Attack Boost.", "Weapon", 3, 1, []);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateDecorationDto.Skills));
    }

    [Fact]
    public async Task Validate_WithDuplicateSkillRankIds_ShouldFail()
    {
        var dto = new CreateDecorationDto(
            "Attack Jewel", "Grants Attack Boost.", "Weapon", 3, 1, [_skillRankId, _skillRankId]);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateDecorationDto.Skills));
    }

    [Fact]
    public async Task Validate_WithEmptyGuidSkillRankId_ShouldFail()
    {
        var dto = new CreateDecorationDto("Attack Jewel", "Grants Attack Boost.", "Weapon", 3, 1, [Guid.Empty]);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);
        result.IsValid.Should().BeFalse();
    }
}
