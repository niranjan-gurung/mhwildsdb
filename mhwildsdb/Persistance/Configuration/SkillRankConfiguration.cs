using mhwildsdb.Entities.Skills;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace mhwildsdb.Persistance.Configuration;

public class SkillRankConfiguration : IEntityTypeConfiguration<SkillRank>
{
    public void Configure(EntityTypeBuilder<SkillRank> builder)
    {
        builder.ToTable("SkillRanks");

        builder.HasKey(sr => sr.Id);

        builder.Property(sr => sr.Level).IsRequired();
        builder.Property(sr => sr.Description)
            .IsRequired()
            .HasMaxLength(200);

        // skill -> skillRanks
        // 1 - many
        builder.HasOne(sr => sr.Skill)
            .WithMany(s => s.Ranks)
            .HasForeignKey(sr => sr.SkillId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(sr => sr.SkillId);
    }
}
