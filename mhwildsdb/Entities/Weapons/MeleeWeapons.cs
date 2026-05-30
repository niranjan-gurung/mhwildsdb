namespace mhwildsdb.Entities.Weapons;

public class Greatsword : MeleeWeapon
{
    private Greatsword() : base(WeaponType.Greatsword) { }

    private Greatsword(WeaponCore core, Sharpness sharpness) : base(core, WeaponType.Greatsword, sharpness) { }

    public static Greatsword Create(WeaponCore core, Sharpness sharpness) => new(core, sharpness);
}

public class Longsword : MeleeWeapon
{
    private Longsword() : base(WeaponType.Longsword) { }

    private Longsword(WeaponCore core, Sharpness sharpness) : base(core, WeaponType.Longsword, sharpness) { }

    public static Longsword Create(WeaponCore core, Sharpness sharpness) => new(core, sharpness);
}

public class SwordAndShield : MeleeWeapon
{
    private SwordAndShield() : base(WeaponType.SwordAndShield) { }

    private SwordAndShield(WeaponCore core, Sharpness sharpness) : base(core, WeaponType.SwordAndShield, sharpness) { }

    public static SwordAndShield Create(WeaponCore core, Sharpness sharpness) => new(core, sharpness);
}

public class DualBlades : MeleeWeapon
{
    private DualBlades() : base(WeaponType.DualBlades) { }

    private DualBlades(WeaponCore core, Sharpness sharpness) : base(core, WeaponType.DualBlades, sharpness) { }

    public static DualBlades Create(WeaponCore core, Sharpness sharpness) => new(core, sharpness);
}

public class Hammer : MeleeWeapon
{
    private Hammer() : base(WeaponType.Hammer) { }

    private Hammer(WeaponCore core, Sharpness sharpness) : base(core, WeaponType.Hammer, sharpness) { }

    public static Hammer Create(WeaponCore core, Sharpness sharpness) => new(core, sharpness);
}

public class HuntingHorn : MeleeWeapon
{
    private HuntingHorn() : base(WeaponType.HuntingHorn) { }

    private HuntingHorn(WeaponCore core, Sharpness sharpness) : base(core, WeaponType.HuntingHorn, sharpness) { }

    public static HuntingHorn Create(WeaponCore core, Sharpness sharpness) => new(core, sharpness);
}

public class Lance : MeleeWeapon
{
    private Lance() : base(WeaponType.Lance) { }

    private Lance(WeaponCore core, Sharpness sharpness) : base(core, WeaponType.Lance, sharpness) { }

    public static Lance Create(WeaponCore core, Sharpness sharpness) => new(core, sharpness);
}

public class Gunlance : MeleeWeapon
{
    public Shell Shell { get; private set; }

    private Gunlance() : base(WeaponType.Gunlance)
    {
        Shell = new Shell(ShellType.Normal, 0);
    }

    private Gunlance(WeaponCore core, Sharpness sharpness, Shell shell) : base(core, WeaponType.Gunlance, sharpness)
    {
        Shell = shell;
    }

    public static Gunlance Create(WeaponCore core, Sharpness sharpness, Shell shell) => new(core, sharpness, shell);
}

public class SwitchAxe : PhialWeapon
{
    private SwitchAxe() : base(WeaponType.SwitchAxe, PhialType.Power) { }

    private SwitchAxe(WeaponCore core, Sharpness sharpness, Phial phial)
        : base(core, WeaponType.SwitchAxe, sharpness, phial) { }

    public static SwitchAxe Create(WeaponCore core, Sharpness sharpness, Phial phial) => new(core, sharpness, phial);
}

public class ChargeBlade : PhialWeapon
{
    private ChargeBlade() : base(WeaponType.ChargeBlade, PhialType.Impact) { }

    private ChargeBlade(WeaponCore core, Sharpness sharpness, Phial phial)
        : base(core, WeaponType.ChargeBlade, sharpness, phial) { }

    public static ChargeBlade Create(WeaponCore core, Sharpness sharpness, Phial phial) => new(core, sharpness, phial);
}

public class InsectGlaive : MeleeWeapon
{
    public int KinsectLevel { get; private set; }

    private InsectGlaive() : base(WeaponType.InsectGlaive) { }

    private InsectGlaive(WeaponCore core, Sharpness sharpness, int kinsectLevel)
        : base(core, WeaponType.InsectGlaive, sharpness)
    {
        KinsectLevel = kinsectLevel;
    }

    public static InsectGlaive Create(WeaponCore core, Sharpness sharpness, int kinsectLevel) =>
        new(core, sharpness, kinsectLevel);
}
