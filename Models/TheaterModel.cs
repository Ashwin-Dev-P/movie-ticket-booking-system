using System.ComponentModel.DataAnnotations;

namespace MovieTicketBookingApp.Models
{
    public class TheaterModel
    {

        [Required]
        [Key]
        public int TheaterId { get; set; }

        [Required] 
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        // A theater can have multiple screens
        public virtual ICollection<ScreenModel> Screens { get; set; }
    }
}
