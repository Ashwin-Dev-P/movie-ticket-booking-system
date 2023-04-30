using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MovieTicketBookingApp.Models
{
    public class ApplicationUser: IdentityUser
    {
        


        // A user can have multiple bookings
        public virtual ICollection<BookingModel> Bookings { get; set; }
    }
}
