using mhwildsdb.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace mhwildsdb.Persistance.Configuration;

public class ArmourConfiguration : IEntityTypeConfiguration<Armour>
{
    public void Configure(EntityTypeBuilder<Armour> builder)
    {
        builder.ToTable("Armours");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(s => s.Piece)
            .IsRequired()
            .HasMaxLength(5);

        builder.Property(s => s.Rank)
            .IsRequired()
            .HasMaxLength(4);

        builder.Property(s => s.Rarity).IsRequired();
        builder.Property(s => s.Defense).IsRequired();

        // configure resistances as owned entity
        builder.OwnsOne(a => a.Resistances, r =>
        {
            r.Property(x => x.Fire).HasColumnName("FireResistance").IsRequired();
            r.Property(x => x.Water).HasColumnName("WaterResistance").IsRequired();
            r.Property(x => x.Ice).HasColumnName("IceResistance").IsRequired();
            r.Property(x => x.Thunder).HasColumnName("ThunderResistance").IsRequired();
            r.Property(x => x.Dragon).HasColumnName("DragonResistance").IsRequired();
        });

        // store slots as integer[]
        builder.PrimitiveCollection(a => a.Slots)
            .HasColumnName("Slots")
            .IsRequired();

        builder.Property(m => m.Created)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(m => m.LastModified)
            .IsRequired()
            .ValueGeneratedOnUpdate();

        // index name field
        builder.HasIndex(m => m.Name);
    }
}
