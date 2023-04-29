using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingApp.Models;

namespace MovieTicketBookingApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call the base model first and then make necessary changes
            base.OnModelCreating(modelBuilder);

            // Mention that Screen belongs to one Theater which has many Screens
            modelBuilder.Entity<ScreenModel>()
                .HasOne(u => u.Theater)
                .WithMany(s => s.Screens)
                .HasForeignKey(u => u.TheaterId)
                .OnDelete(DeleteBehavior.Cascade);

            // Mention that Theater has many screens
            modelBuilder.Entity<TheaterModel>()
                .HasMany(s => s.Screens);
                
        }

        public DbSet<TheaterModel> Theater { get; set; }
        public DbSet<ScreenModel> Screens { get; set; }

        public DbSet<MovieModel> Movies { get; set; }
    }
}