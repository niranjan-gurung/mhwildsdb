using mhwildsdb.Entities.Weapons;

namespace mhwildsdb.DTOs.Weapons;

public sealed record DamageDto(int Raw, int Display);

public sealed record WeaponSpecialDto(
    Guid? Id,
    WeaponSpecialType Type,
    ElementType? Element,
    StatusType? Status,
    DamageDto Damage,
    bool Hidden);

public sealed record SharpnessDto(
    int Red,
    int Orange,
    int Yellow,
    int Green,
    int Blue,
    int White,
    int Purple);

public sealed record PhialDto(PhialType Type, DamageDto? Damage);

public sealed record ShellDto(ShellType Type, int Power);

public sealed record AmmoDto(string Type, int Level, int Capacity, bool? Rapid);
