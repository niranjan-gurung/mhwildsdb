namespace mhwildsdb.Entities.Weapons;

public class LightBowgun : RangedWeapon
{
    public string SpecialAmmo { get; private set; }

    private LightBowgun() : base(WeaponType.LightBowgun)
    {
        SpecialAmmo = string.Empty;
    }

    private LightBowgun(WeaponCore core, ICollection<Ammo> ammo, string specialAmmo)
        : base(core, WeaponType.LightBowgun, ammo)
    {
        SpecialAmmo = specialAmmo;
    }

    public static LightBowgun Create(WeaponCore core, ICollection<Ammo> ammo, string specialAmmo) =>
        new(core, ammo, specialAmmo);
}

public class HeavyBowgun : RangedWeapon
{
    private HeavyBowgun() : base(WeaponType.HeavyBowgun) { }

    private HeavyBowgun(WeaponCore core, ICollection<Ammo> ammo) : base(core, WeaponType.HeavyBowgun, ammo) { }

    public static HeavyBowgun Create(WeaponCore core, ICollection<Ammo> ammo) => new(core, ammo);
}

public class Bow : RangedWeapon
{
    public ICollection<CoatingType> Coatings { get; private set; } = [];

    private Bow() : base(WeaponType.Bow) { }

    private Bow(WeaponCore core, ICollection<CoatingType> coatings) : base(core, WeaponType.Bow, [])
    {
        Coatings = coatings;
    }

    public static Bow Create(WeaponCore core, ICollection<CoatingType> coatings) => new(core, coatings);
}
