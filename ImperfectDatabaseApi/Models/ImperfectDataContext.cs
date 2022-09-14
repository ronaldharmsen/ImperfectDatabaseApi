using Microsoft.EntityFrameworkCore;

namespace ImperfectDatabaseApi.Models
{
    public class ImperfectDataContext : DbContext
    {
        public ImperfectDataContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultContainer("Imperfect");

            modelBuilder.Entity<User>()
                .OwnsOne(u => u.Profile);

            modelBuilder.Entity<Post>()
                .OwnsMany(p => p.LinksToImages);

            modelBuilder.Entity<Post>()
                .OwnsOne(p => p.Author);

        }
    }
}
