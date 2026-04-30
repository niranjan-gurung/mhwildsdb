using mhwildsdb.Entities;
using Microsoft.EntityFrameworkCore;

namespace mhwildsdb.Persistance
{
    public class MhwildsDbContext(DbContextOptions<MhwildsDbContext> options) : DbContext(options)
    {
        public DbSet<Skill> Skills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("app");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MhwildsDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        /* DB SEEDING
         * left unused, will do custom seeding in the future from separate parser
         * 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseAsyncSeeding(async (context, _, cancellationToken) =>
            {
                var sampleSkill = await context.Set<Skill>()
                    .FirstOrDefaultAsync(b => b.Name == "Dragon Resistance", cancellationToken);

                if (sampleSkill == null)
                {
                    sampleSkill = Skill.Create("Dragon Resistance", "armour", "Increases dragon resistance. Also improves defense at higher levels.");
                    await context.Set<Skill>().AddAsync(sampleSkill, cancellationToken);
                    await context.SaveChangesAsync();
                }
            })
            .UseSeeding((context, _) =>
            {
                var sampleSkill = context.Set<Skill>()
                    .FirstOrDefault(b => b.Name == "Dragon Resistance");

                if (sampleSkill == null)
                {
                    sampleSkill = Skill.Create("Dragon Resistance", "armour", "Increases dragon resistance. Also improves defense at higher levels.");
                    context.Set<Skill>().Add(sampleSkill);
                    context.SaveChanges();
                }
            });
        } */
    }
}
