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

            modelBuilder.Entity<ShowModel>()
                .HasOne(s => s.Screen)
                .WithMany(u => u.Shows)
                .HasForeignKey(t => t.MovieId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(u => u.ScreenId)
                .OnDelete(DeleteBehavior.Cascade);

            // A screen has multiple shows
            modelBuilder.Entity<ScreenModel>().HasMany(s => s.Shows);

            // modelBuilder.Entity<MovieModel>().HasMany(T => T.Shows);

            


            // Auto generated date time on creation of the row on the sql
            //modelBuilder.Entity<BookingModel>()
            //    .HasOne(u=>u.User)
            //    .WithMany(s=>)
            //    .Property(b => b.BookedAt)
            //    .HasDefaultValueSql("getdate()");
        
        }

        public DbSet<TheaterModel> Theater { get; set; }
        public DbSet<ScreenModel> Screens { get; set; }

        public DbSet<MovieModel> Movies { get; set; }
        public DbSet<ShowModel> Show { get; set; }
        


        

       
    }
}