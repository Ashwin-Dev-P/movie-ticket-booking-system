using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieTicketBookingApp.Models
{
    public class BookingModel
    {
        [Key]
        public int BookingId { get; set; }

        public DateTime ? BookedAt { get; set; } = DateTime.Now;


        [Required]
        [ForeignKey("User")]
        public string UserId {get; set; }

        [Required]
        [ForeignKey("Show")]
        public int ShowId { get; set; }

        public virtual IdentityUser User { get; set; }

        public virtual ShowModel Show { get; set; }
    }
}
