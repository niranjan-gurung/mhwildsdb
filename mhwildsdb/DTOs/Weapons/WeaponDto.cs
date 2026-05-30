using mhwildsdb.DTOs.Skills.SkillRank;
using mhwildsdb.Entities.Weapons;

namespace mhwildsdb.DTOs.Weapons;

public sealed record WeaponDto(
    Guid Id,
    string Name,
    string? Description,
    WeaponType WeaponType,
    int Defense,
    int Rarity,
    ICollection<int> Slots,
    int Affinity,
    DamageDto Damage,
    ICollection<WeaponSpecialDto> Specials,
    ICollection<SkillRankDetailDto> Skills,
    SharpnessDto? Sharpness,
    PhialDto? Phial,
    ShellDto? Shell,
    int? KinsectLevel,
    ICollection<AmmoDto>? Ammo,
    string? SpecialAmmo,
    ICollection<CoatingType>? Coatings);
