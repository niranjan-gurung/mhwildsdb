using mhwildsdb.Entities.Skills;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace mhwildsdb.Persistance.Configuration;

public class SkillConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.ToTable("Skills");

        builder.HasKey(s => s.Id);
        
        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(s => s.Type)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(s => s.Description)
            .IsRequired()
            .HasMaxLength(200);

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
