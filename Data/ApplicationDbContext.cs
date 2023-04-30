using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingApp.Models;

namespace MovieTicketBookingApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
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

            // A show has many bookings
            modelBuilder.Entity<ShowModel>()
                .HasMany(s => s.Bookings);

            // A screen has multiple shows
            modelBuilder.Entity<ScreenModel>().HasMany(s => s.Shows);

            // A movie has many shows
            modelBuilder.Entity<MovieModel>().HasMany(T => T.Shows);


            

            // A booking has a user with many bookings
            modelBuilder.Entity<BookingModel>()
                .HasOne(u => u.User)
                .WithMany(s => s.Bookings)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // A booking has one show with many bookings
            modelBuilder.Entity<BookingModel>()
                .HasOne(u=>u.Show)
                .WithMany(s=>s.Bookings)
                .HasForeignKey(t=>t.ShowId)
                .OnDelete(DeleteBehavior.Cascade);

            // Auto generated date time on creation of the row on the sql
            modelBuilder.Entity<BookingModel>()
                .Property(b => b.BookedAt)
                .HasDefaultValueSql("getdate()");

            // A user can have many bookings
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u=>u.Bookings);
        }

        public DbSet<TheaterModel> Theater { get; set; }
        public DbSet<ScreenModel> Screens { get; set; }

        public DbSet<MovieModel> Movies { get; set; }
        public DbSet<ShowModel> Show { get; set; }

        public DbSet<BookingModel> Bookings { get; set; }

        public DbSet<ApplicationUser> AspNetUsers { get; set; }
        


        

       
    }
}