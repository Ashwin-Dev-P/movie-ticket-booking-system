using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieTicketBookingApp.Models
{
    public class ScreenModel
    {
        [Key]
        public int ScreenId { get; set; }

        [Required]
        public string Name { get; set; }


        public string? Description { get; set; }

        [ForeignKey("Theater")]
        public int TheaterId { get; set; }

        [Required]
        public virtual TheaterModel Theater { get; set; }
    }
}
