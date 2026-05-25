using mhwildsdb.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace mhwildsdb.Persistance.Configuration;

public class DecorationConfiguration : IEntityTypeConfiguration<Decoration>
{
    public void Configure(EntityTypeBuilder<Decoration> builder)
    {
        builder.ToTable("Decorations");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(d => d.Description)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.Type)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(d => d.Rarity).IsRequired();
        builder.Property(d => d.Slot).IsRequired();

        builder.Property(d => d.Created)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(d => d.LastModified)
            .IsRequired()
            .ValueGeneratedOnUpdate();

        // decorations -> skillranks: many - many
        builder.HasMany(d => d.Skills)
            .WithMany()
            .UsingEntity(j =>
            {
                j.ToTable("DecorationSkillRanks");
                j.Property<Guid>("SkillsId").HasColumnName("SkillRanksId");
            });

        builder.HasIndex(d => d.Name);
    }
}