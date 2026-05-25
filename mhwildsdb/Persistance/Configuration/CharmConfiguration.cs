using mhwildsdb.Entities.Talismans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace mhwildsdb.Persistance.Configuration;

public class CharmConfiguration : IEntityTypeConfiguration<Charm>
{
    public void Configure(EntityTypeBuilder<Charm> builder)
    {
        builder.ToTable("Charms");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.Created)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(c => c.LastModified)
            .IsRequired()
            .ValueGeneratedOnUpdate();

        builder.HasIndex(c => c.Name);
    }
}
