using Microsoft.EntityFrameworkCore;

namespace RestaurantAPI.Entities
{
    public class RestaurantDbContext : DbContext
    {
        private string connectionString =
            "Server=(localdb)\\mssqllocaldb;Database=estaurantDb;Trusted_Connection=True;";
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Chef> Chefs { get; set; }
        public DbSet<Ingredients> Ingredients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<Dish>()
                .Property(d => d.Name)
                .IsRequired();

            modelBuilder.Entity<Ingredients>()
                .Property(i => i.Name)
                .IsRequired();

            modelBuilder.Entity<Address>(ad =>
            {
                ad.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(50);

                ad.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(50);
            });

            modelBuilder.Entity<Chef>(ch =>
            {
                ch.Property(c => c.FullName).IsRequired();
            });
                
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
