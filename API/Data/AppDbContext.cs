using API.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<UserOrganisation> UserOrganisations { get; set; }
       
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.UserId)
                .IsUnique();

           modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Organisation>()
                .HasIndex(o => o.OrgId)
                .IsUnique();

            // Configure many to many relatonship...
            modelBuilder.Entity<UserOrganisation>()
                .HasKey(uo => new { uo.UserId, uo.OrganisationId });

            modelBuilder.Entity<UserOrganisation>()
                .HasOne(uo => uo.AppUser)
                .WithMany(u => u.UserOrganisations)
                .HasForeignKey(uo => uo.UserId);

            modelBuilder.Entity<UserOrganisation>()
                .HasOne(uo => uo.Organisation)
                .WithMany(uo => uo.UserOrganisations)
                .HasForeignKey(uo => uo.OrganisationId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
