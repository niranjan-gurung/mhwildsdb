using mhwildsdb.Entities.Weapons;

namespace mhwildsdb.DTOs.Weapons;

public sealed record CreateWeaponDto(
    string Name,
    string? Description,
    WeaponType WeaponType,
    int Defense,
    int Rarity,
    ICollection<int>? Slots,
    int Affinity,
    DamageDto Damage,
    ICollection<WeaponSpecialDto>? Specials,
    ICollection<Guid>? Skills,
    SharpnessDto? Sharpness,
    PhialDto? Phial,
    ShellDto? Shell,
    int? KinsectLevel,
    ICollection<AmmoDto>? Ammo,
    string? SpecialAmmo,
    ICollection<CoatingType>? Coatings);
