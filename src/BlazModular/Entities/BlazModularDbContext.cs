using Microsoft.EntityFrameworkCore;

namespace BlazModular.Entities
{
    public class BlazModularDbContext : DbContext
    {
        public DbSet<Module> Module { get; set; }

        public BlazModularDbContext(DbContextOptions<BlazModularDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Module>().HasKey(c => c.Id);
        }
    }
}
