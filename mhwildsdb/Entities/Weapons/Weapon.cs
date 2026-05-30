using mhwildsdb.Entities.Skills;

namespace mhwildsdb.Entities.Weapons;

public abstract class Weapon : EntityBase
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public WeaponType WeaponType { get; private set; }
    public int Defense { get; private set; }
    public int Rarity { get; private set; }
    public ICollection<int> Slots { get; private set; } = [];
    public int Affinity { get; private set; }
    public Damage Damage { get; private set; }
    public ICollection<WeaponSpecial> Specials { get; private set; } = [];
    public ICollection<SkillRank> SkillRanks { get; private set; } = [];

    protected Weapon(WeaponType weaponType)
    {
        Name = string.Empty;
        WeaponType = weaponType;
        Damage = new Damage(0, 0);
    }

    protected Weapon(WeaponCore core, WeaponType weaponType)
    {
        Name = core.Name;
        Description = core.Description;
        WeaponType = weaponType;
        Defense = core.Defense;
        Rarity = core.Rarity;
        Slots = core.Slots;
        Affinity = core.Affinity;
        Damage = core.Damage;
        Specials = core.Specials;
        SkillRanks = core.SkillRanks;
    }
}

public abstract class MeleeWeapon : Weapon
{
    public Sharpness Sharpness { get; private set; }

    protected MeleeWeapon(WeaponType weaponType) : base(weaponType)
    {
        Sharpness = new Sharpness(0, 0, 0, 0, 0, 0, 0);
    }

    protected MeleeWeapon(WeaponCore core, WeaponType weaponType, Sharpness sharpness) : base(core, weaponType)
    {
        Sharpness = sharpness;
    }
}

public abstract class PhialWeapon : MeleeWeapon
{
    public Phial Phial { get; private set; }

    protected PhialWeapon(WeaponType weaponType, PhialType defaultPhialType) : base(weaponType)
    {
        Phial = new Phial(defaultPhialType, null);
    }

    protected PhialWeapon(WeaponCore core, WeaponType weaponType, Sharpness sharpness, Phial phial)
        : base(core, weaponType, sharpness)
    {
        Phial = phial;
    }
}

public abstract class RangedWeapon : Weapon
{
    public ICollection<Ammo> Ammo { get; private set; } = [];

    protected RangedWeapon(WeaponType weaponType) : base(weaponType) { }

    protected RangedWeapon(WeaponCore core, WeaponType weaponType, ICollection<Ammo> ammo) : base(core, weaponType)
    {
        Ammo = ammo;
    }
}
