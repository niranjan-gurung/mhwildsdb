using mhwildsdb.DTOs.Weapons;
using mhwildsdb.Entities.Weapons;

namespace mhwildsdb.Helpers.Extensions.Mapping;

public static class WeaponMappingExtension
{
    public static WeaponDto ToDto(this Weapon weapon) =>
        new(
            weapon.Id,
            weapon.Name,
            weapon.Description,
            weapon.WeaponType,
            weapon.Defense,
            weapon.Rarity,
            weapon.Slots.ToList(),
            weapon.Affinity,
            weapon.Damage.ToDto(),
            weapon.Specials.Select(s => s.ToDto()).ToList(),
            weapon.SkillRanks.Select(sr => sr.ToDetailDto()).ToList(),
            weapon is MeleeWeapon melee ? melee.Sharpness.ToDto() : null,
            weapon is PhialWeapon phialWeapon ? phialWeapon.Phial.ToDto() : null,
            weapon is Gunlance gunlance ? gunlance.Shell.ToDto() : null,
            weapon is InsectGlaive insectGlaive ? insectGlaive.KinsectLevel : null,
            weapon is RangedWeapon ranged ? ranged.Ammo.Select(a => a.ToDto()).ToList() : null,
            weapon is LightBowgun lightBowgun ? lightBowgun.SpecialAmmo : null,
            weapon is Bow bow ? bow.Coatings.ToList() : null
        );

    public static Damage ToDomain(this DamageDto dto) => new(dto.Raw, dto.Display);

    public static DamageDto ToDto(this Damage damage) => new(damage.Raw, damage.Display);

    public static WeaponSpecial ToDomain(this WeaponSpecialDto dto) =>
        dto.Type switch
        {
            WeaponSpecialType.Element => WeaponSpecial.CreateElement(dto.Element!.Value, dto.Damage.ToDomain(), dto.Hidden),
            WeaponSpecialType.Status => WeaponSpecial.CreateStatus(dto.Status!.Value, dto.Damage.ToDomain(), dto.Hidden),
            _ => throw new ArgumentOutOfRangeException(nameof(dto.Type), dto.Type, "Unsupported weapon special type.")
        };

    public static WeaponSpecialDto ToDto(this WeaponSpecial special) =>
        new(special.Id, special.Type, special.Element, special.Status, special.Damage.ToDto(), special.Hidden);

    public static Sharpness ToDomain(this SharpnessDto dto) =>
        new(dto.Red, dto.Orange, dto.Yellow, dto.Green, dto.Blue, dto.White, dto.Purple);

    public static SharpnessDto ToDto(this Sharpness sharpness) =>
        new(
            sharpness.Red,
            sharpness.Orange,
            sharpness.Yellow,
            sharpness.Green,
            sharpness.Blue,
            sharpness.White,
            sharpness.Purple);

    public static Phial ToDomain(this PhialDto dto) => new(dto.Type, dto.Damage?.ToDomain());

    public static PhialDto ToDto(this Phial phial) => new(phial.Type, phial.Damage?.ToDto());

    public static Shell ToDomain(this ShellDto dto) => new(dto.Type, dto.Power);

    public static ShellDto ToDto(this Shell shell) => new(shell.Type, shell.Power);

    public static Ammo ToDomain(this AmmoDto dto) => new(dto.Type, dto.Level, dto.Capacity, dto.Rapid);

    public static AmmoDto ToDto(this Ammo ammo) => new(ammo.Type, ammo.Level, ammo.Capacity, ammo.Rapid);
}
