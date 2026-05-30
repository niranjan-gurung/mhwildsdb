using mhwildsdb.Entities.Weapons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace mhwildsdb.Persistance.Configuration;

public class WeaponConfiguration : IEntityTypeConfiguration<Weapon>
{
    public void Configure(EntityTypeBuilder<Weapon> builder)
    {
        builder.ToTable("Weapons");

        builder.HasKey(w => w.Id);

        builder.HasDiscriminator(w => w.WeaponType)
            .HasValue<Greatsword>(WeaponType.Greatsword)
            .HasValue<Longsword>(WeaponType.Longsword)
            .HasValue<SwordAndShield>(WeaponType.SwordAndShield)
            .HasValue<DualBlades>(WeaponType.DualBlades)
            .HasValue<Hammer>(WeaponType.Hammer)
            .HasValue<HuntingHorn>(WeaponType.HuntingHorn)
            .HasValue<SwitchAxe>(WeaponType.SwitchAxe)
            .HasValue<ChargeBlade>(WeaponType.ChargeBlade)
            .HasValue<Lance>(WeaponType.Lance)
            .HasValue<Gunlance>(WeaponType.Gunlance)
            .HasValue<InsectGlaive>(WeaponType.InsectGlaive)
            .HasValue<LightBowgun>(WeaponType.LightBowgun)
            .HasValue<HeavyBowgun>(WeaponType.HeavyBowgun)
            .HasValue<Bow>(WeaponType.Bow);

        builder.Property(w => w.WeaponType)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(w => w.Name)
            .IsRequired()
            .HasMaxLength(80);

        builder.Property(w => w.Description)
            .HasMaxLength(500);

        builder.Property(w => w.Defense).IsRequired();
        builder.Property(w => w.Rarity).IsRequired();
        builder.Property(w => w.Affinity).IsRequired();

        builder.PrimitiveCollection(w => w.Slots)
            .HasColumnName("Slots")
            .IsRequired();

        builder.OwnsOne(w => w.Damage, damage =>
        {
            damage.Property(d => d.Raw).HasColumnName("RawDamage").IsRequired();
            damage.Property(d => d.Display).HasColumnName("DisplayDamage").IsRequired();
        });

        builder.OwnsMany(w => w.Specials, special =>
        {
            special.ToTable("WeaponSpecials");
            special.WithOwner().HasForeignKey("WeaponId");
            special.HasKey(s => s.Id);

            special.Property(s => s.Type)
                .HasConversion<string>()
                .HasMaxLength(10)
                .IsRequired();

            special.Property(s => s.Element)
                .HasConversion<string>()
                .HasMaxLength(10);

            special.Property(s => s.Status)
                .HasConversion<string>()
                .HasMaxLength(10);

            special.OwnsOne(s => s.Damage, damage =>
            {
                damage.Property(d => d.Raw).HasColumnName("RawDamage").IsRequired();
                damage.Property(d => d.Display).HasColumnName("DisplayDamage").IsRequired();
            });

            special.Property(s => s.Hidden).IsRequired();
        });

        builder.HasMany(w => w.SkillRanks)
            .WithMany()
            .UsingEntity(j =>
            {
                j.ToTable("WeaponSkillRanks");
                j.Property<Guid>("SkillRanksId").HasColumnName("SkillRanksId");
            });

        builder.Property(w => w.Created)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(w => w.LastModified)
            .IsRequired()
            .ValueGeneratedOnUpdate();

        builder.HasIndex(w => w.Name);
    }
}

public class MeleeWeaponConfiguration : IEntityTypeConfiguration<MeleeWeapon>
{
    public void Configure(EntityTypeBuilder<MeleeWeapon> builder)
    {
        builder.OwnsOne(w => w.Sharpness, sharpness =>
        {
            sharpness.Property(s => s.Red).HasColumnName("SharpnessRed");
            sharpness.Property(s => s.Orange).HasColumnName("SharpnessOrange");
            sharpness.Property(s => s.Yellow).HasColumnName("SharpnessYellow");
            sharpness.Property(s => s.Green).HasColumnName("SharpnessGreen");
            sharpness.Property(s => s.Blue).HasColumnName("SharpnessBlue");
            sharpness.Property(s => s.White).HasColumnName("SharpnessWhite");
            sharpness.Property(s => s.Purple).HasColumnName("SharpnessPurple");
        });
    }
}

public class PhialWeaponConfiguration : IEntityTypeConfiguration<PhialWeapon>
{
    public void Configure(EntityTypeBuilder<PhialWeapon> builder)
    {
        builder.OwnsOne(w => w.Phial, phial =>
        {
            phial.Property(p => p.Type)
                .HasColumnName("PhialType")
                .HasConversion<string>()
                .HasMaxLength(15);

            phial.OwnsOne(p => p.Damage, damage =>
            {
                damage.Property(d => d.Raw).HasColumnName("PhialRawDamage");
                damage.Property(d => d.Display).HasColumnName("PhialDisplayDamage");
            });
        });
    }
}

public class GunlanceConfiguration : IEntityTypeConfiguration<Gunlance>
{
    public void Configure(EntityTypeBuilder<Gunlance> builder)
    {
        builder.OwnsOne(w => w.Shell, shell =>
        {
            shell.Property(s => s.Type)
                .HasColumnName("ShellType")
                .HasConversion<string>()
                .HasMaxLength(10);

            shell.Property(s => s.Power).HasColumnName("ShellPower");
        });
    }
}

public class InsectGlaiveConfiguration : IEntityTypeConfiguration<InsectGlaive>
{
    public void Configure(EntityTypeBuilder<InsectGlaive> builder)
    {
        builder.Property(w => w.KinsectLevel);
    }
}

public class RangedWeaponConfiguration : IEntityTypeConfiguration<RangedWeapon>
{
    public void Configure(EntityTypeBuilder<RangedWeapon> builder)
    {
        builder.OwnsMany(w => w.Ammo, ammo =>
        {
            ammo.ToTable("WeaponAmmo");
            ammo.WithOwner().HasForeignKey("WeaponId");
            ammo.HasKey(a => a.Id);

            ammo.Property(a => a.Type)
                .IsRequired()
                .HasMaxLength(30);

            ammo.Property(a => a.Level).IsRequired();
            ammo.Property(a => a.Capacity).IsRequired();
            ammo.Property(a => a.Rapid);
        });
    }
}

public class LightBowgunConfiguration : IEntityTypeConfiguration<LightBowgun>
{
    public void Configure(EntityTypeBuilder<LightBowgun> builder)
    {
        builder.Property(w => w.SpecialAmmo)
            .HasMaxLength(50);
    }
}

public class BowConfiguration : IEntityTypeConfiguration<Bow>
{
    public void Configure(EntityTypeBuilder<Bow> builder)
    {
        builder.PrimitiveCollection(w => w.Coatings)
            .HasColumnName("Coatings")
            .IsRequired();
    }
}
