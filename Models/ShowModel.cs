using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieTicketBookingApp.Models
{
    public class ShowModel
    {
        [Required]
        [Key]
        public int ShowId { get; set; }

        [Required]
        public DateTime ShowTime { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        [ForeignKey("Screen")]
        public int ScreenId { get; set; }

        [Required]
        [ForeignKey("Movie")]
        public int MovieId { get; set; }

        public virtual MovieModel Movie { get; set; }
        public virtual ScreenModel Screen { get; set; }



        // A show can have multiple bookings
         public virtual ICollection<BookingModel> Bookings { get; set; }

    }
}
