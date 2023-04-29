using System.ComponentModel.DataAnnotations;

namespace MovieTicketBookingApp.Models
{
    public class MovieModel
    {
        [Key]
        public int movieId { get; set; }

        [Required]
        public string title { get; set; }

        public string? description { get; set; }



    }
}
