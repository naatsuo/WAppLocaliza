using Microsoft.EntityFrameworkCore;
using WAppLocaliza.Entities;

namespace WAppLocaliza.Application
{
    public class ApplicationDbContext : DbContext
    {
        private string ConnectionString { get; } = "Server=10.0.0.3;Database=DevLocaliza;User Id=dev;Password=3jV1vzXP7tJQ6tcIK2qQM5zLR2toIT9tyEF6lxR4;";

        public DbSet<CarBrand> CarBrands { get; set; }
        public DbSet<CarModel> CarModels { get; set; }
        public DbSet<Car> Cars { get; set; }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Operator> Operators { get; set; }


        public ApplicationDbContext() { }

        public void DatabaseReset()
        {
#if DEBUG
            Database.EnsureDeleted();
            Database.EnsureCreated();
#endif
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString, i => i.MigrationsHistoryTable("DatabaseMigrations"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasIndex(i => i.Document).IsUnique();
            modelBuilder.Entity<User>().HasIndex(i => i.Email).IsUnique();
            modelBuilder.Entity<Operator>().HasIndex(i => i.Number).IsUnique();
            modelBuilder.Entity<CarBrand>().HasIndex(i => i.Name).IsUnique();
            modelBuilder.Entity<CarModel>().HasIndex(i => i.Description).IsUnique();
            modelBuilder.Entity<Car>().HasIndex(i => i.Plate).IsUnique();
            modelBuilder.Entity<User>().Property(i => i.Roles).HasConversion(
               i => string.Join(';', i.Select(j => j).ToArray()),
               i => i.Split(';', StringSplitOptions.None).Select(j => j).ToArray()
            );
            modelBuilder.Entity<Operator>().Property(i => i.Roles).HasConversion(
               i => string.Join(';', i.Select(j => j).ToArray()),
               i => i.Split(';', StringSplitOptions.None).Select(j => j).ToArray()
            );
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
