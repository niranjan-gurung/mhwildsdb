using mhwildsdb.Entities.Armours;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace mhwildsdb.Persistance.Configuration;

public class ArmourSetConfiguration : IEntityTypeConfiguration<ArmourSet>
{
    public void Configure(EntityTypeBuilder<ArmourSet> builder)
    {
        builder.ToTable("ArmourSets");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(20);

        // armourset -> armour pieces
        // 1 - many
        builder.HasMany(a => a.Pieces)
            .WithOne(p => p.ArmourSet)
            .HasForeignKey(p => p.ArmourSetId)
            .OnDelete(DeleteBehavior.SetNull);

        // set bonus (optional)
        builder.HasOne(a => a.SetBonusSkill)
            .WithMany()
            .HasForeignKey(a => a.SetBonusSkillId)
            .OnDelete(DeleteBehavior.SetNull);

        // group bonus (optional)
        builder.HasOne(a => a.GroupBonusSkill)
            .WithMany()
            .HasForeignKey(a => a.GroupBonusSkillId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(m => m.Created)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(m => m.LastModified)
            .IsRequired()
            .ValueGeneratedOnUpdate();

        builder.HasIndex(a => a.Name);
    }
}
