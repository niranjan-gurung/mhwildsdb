using mhwildsdb.Entities.Skills;

namespace mhwildsdb.Entities.Weapons;

public class Damage
{
    public int Raw { get; private set; }
    public int Display { get; private set; }

    private Damage() { }

    public Damage(int raw, int display)
    {
        Raw = raw;
        Display = display;
    }
}

public class WeaponSpecial
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public WeaponSpecialType Type { get; private set; }
    public ElementType? Element { get; private set; }
    public StatusType? Status { get; private set; }
    public Damage Damage { get; private set; }
    public bool Hidden { get; private set; }

    private WeaponSpecial()
    {
        Damage = new Damage(0, 0);
    }

    private WeaponSpecial(
        WeaponSpecialType type,
        ElementType? element,
        StatusType? status,
        Damage damage,
        bool hidden)
    {
        Type = type;
        Element = element;
        Status = status;
        Damage = damage;
        Hidden = hidden;
    }

    public static WeaponSpecial CreateElement(ElementType element, Damage damage, bool hidden)
    {
        return new WeaponSpecial(WeaponSpecialType.Element, element, null, damage, hidden);
    }

    public static WeaponSpecial CreateStatus(StatusType status, Damage damage, bool hidden)
    {
        return new WeaponSpecial(WeaponSpecialType.Status, null, status, damage, hidden);
    }
}

public class Sharpness
{
    public int Red { get; private set; }
    public int Orange { get; private set; }
    public int Yellow { get; private set; }
    public int Green { get; private set; }
    public int Blue { get; private set; }
    public int White { get; private set; }
    public int Purple { get; private set; }

    private Sharpness() { }

    public Sharpness(int red, int orange, int yellow, int green, int blue, int white, int purple)
    {
        Red = red;
        Orange = orange;
        Yellow = yellow;
        Green = green;
        Blue = blue;
        White = white;
        Purple = purple;
    }
}

public class Phial
{
    public PhialType Type { get; private set; }
    public Damage? Damage { get; private set; }

    private Phial() { }

    public Phial(PhialType type, Damage? damage)
    {
        Type = type;
        Damage = damage;
    }
}

public class Shell
{
    public ShellType Type { get; private set; }
    public int Power { get; private set; }

    private Shell() { }

    public Shell(ShellType type, int power)
    {
        Type = type;
        Power = power;
    }
}

public class Ammo
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Type { get; private set; }
    public int Level { get; private set; }
    public int Capacity { get; private set; }
    public bool? Rapid { get; private set; }

    private Ammo()
    {
        Type = string.Empty;
    }

    public Ammo(string type, int level, int capacity, bool? rapid)
    {
        Type = type;
        Level = level;
        Capacity = capacity;
        Rapid = rapid;
    }
}

public sealed record WeaponCore(
    string Name,
    string? Description,
    int Defense,
    int Rarity,
    ICollection<int> Slots,
    int Affinity,
    Damage Damage,
    ICollection<WeaponSpecial> Specials,
    ICollection<SkillRank> SkillRanks);
