using FluentAssertions;
using mhwildsdb.DTOs.Weapons;
using mhwildsdb.Entities.Weapons;
using mhwildsdb.Validators.WeaponValidators;

namespace mhwildsdb.Tests.Validators.Weapons;

public class CreateWeaponDtoValidatorTests
{
    private readonly CreateWeaponDtoValidator _validator = new();
    private readonly Guid _skillRankId = Guid.NewGuid();

    private static DamageDto Damage => new(100, 480);
    private static SharpnessDto Sharpness => new(20, 30, 40, 50, 30, 10, 0);

    private CreateWeaponDto ValidGreatsword =>
        new(
            "Hope Blade",
            null,
            WeaponType.Greatsword,
            0,
            3,
            [1],
            5,
            Damage,
            [new(null, WeaponSpecialType.Element, ElementType.Water, null, new DamageDto(20, 200), false)],
            [_skillRankId],
            Sharpness,
            null,
            null,
            null,
            null,
            null,
            null);

    [Fact]
    public async Task Validate_WithValidMeleeWeapon_ShouldPass()
    {
        var result = await _validator.ValidateAsync(ValidGreatsword, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_WithValidLightBowgun_ShouldPass()
    {
        var dto = ValidGreatsword with
        {
            Name = "Hope Rifle",
            WeaponType = WeaponType.LightBowgun,
            Slots = [],
            Affinity = 0,
            Specials = [],
            Skills = [],
            Sharpness = null,
            Ammo = [new("Normal", 1, 5, true)],
            SpecialAmmo = "Wyvernblast"
        };

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_WithMeleeWeaponMissingSharpness_ShouldFail()
    {
        var dto = ValidGreatsword with { Sharpness = null };

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateWeaponDto.Sharpness));
    }

    [Fact]
    public async Task Validate_WithElementSpecialMissingElement_ShouldFail()
    {
        var dto = ValidGreatsword with
        {
            Specials = [new(null, WeaponSpecialType.Element, null, null, new DamageDto(20, 200), false)]
        };

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains(nameof(WeaponSpecialDto.Element)));
    }

    [Fact]
    public async Task Validate_WithStatusSpecialContainingElement_ShouldFail()
    {
        var dto = ValidGreatsword with
        {
            Specials = [new(null, WeaponSpecialType.Status, ElementType.Fire, StatusType.Blast, new DamageDto(20, 200), false)]
        };

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains(nameof(WeaponSpecialDto.Element)));
    }

    [Fact]
    public async Task Validate_WithGunlanceMissingShell_ShouldFail()
    {
        var dto = ValidGreatsword with { WeaponType = WeaponType.Gunlance };

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateWeaponDto.Shell));
    }

    [Fact]
    public async Task Validate_WithLightBowgunMissingAmmo_ShouldFail()
    {
        var dto = ValidGreatsword with
        {
            WeaponType = WeaponType.LightBowgun,
            Sharpness = null,
            SpecialAmmo = "Wyvernblast"
        };

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateWeaponDto.Ammo));
    }
}
