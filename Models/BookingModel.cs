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

        public virtual ShowModel Show { get; set; }
        [ForeignKey("Show")] public int ShowId { get; set; }




        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }


        


    }
}
