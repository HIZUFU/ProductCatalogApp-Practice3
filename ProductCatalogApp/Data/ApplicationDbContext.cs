using Microsoft.EntityFrameworkCore;
using ProductCatalogApp.Models;

namespace ProductCatalogApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProductionSite> ProductionSites =>
     Set<ProductionSite>();

        public DbSet<Equipment> EquipmentItems =>
            Set<Equipment>();
        public DbSet<AppUser> Users { get; set; } = null!;

        public DbSet<Role> Roles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>()
                .HasIndex(user => user.Username)
                .IsUnique();

            modelBuilder.Entity<Role>()
                .HasIndex(role => role.Name)
                .IsUnique();

            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "User"
                },
                new Role
                {
                    Id = 2,
                    Name = "Admin"
                }
            );
        }
    }
}