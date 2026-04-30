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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder
            //    .UseAsyncSeeding(async (context, _, cancellationToken) =>
            //{
            //    var sampleSkill = await context.Set<Skill>().FirstOrDefaultAsync(cancellationToken);
            //});
        }
    }
}
