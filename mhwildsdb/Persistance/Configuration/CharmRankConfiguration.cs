using mhwildsdb.Entities.Talismans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace mhwildsdb.Persistance.Configuration;

public class CharmRankConfiguration : IEntityTypeConfiguration<CharmRank>
{
    public void Configure(EntityTypeBuilder<CharmRank> builder)
    {
        builder.ToTable("CharmRanks");

        builder.HasKey(cr => cr.Id);

        builder.Property(cr => cr.Name)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(cr => cr.Description)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(cr => cr.Level).IsRequired();
        builder.Property(cr => cr.Rarity).IsRequired();

        builder.Property(cr => cr.Created)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(cr => cr.LastModified)
            .IsRequired()
            .ValueGeneratedOnUpdate();

        // charm -> charmranks: 1 - many
        builder.HasOne(cr => cr.Charm)
            .WithMany(c => c.Ranks)
            .HasForeignKey(cr => cr.CharmId)
            .OnDelete(DeleteBehavior.Cascade);

        // charmranks -> skillranks: many - many
        builder.HasMany(cr => cr.Skills)
            .WithMany()
            .UsingEntity(j => j.ToTable("CharmRankSkillRanks"));

        builder.HasIndex(cr => cr.CharmId);
    }
}
