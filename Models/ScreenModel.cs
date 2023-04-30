using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieTicketBookingApp.Models
{
    public class ScreenModel
    {
        [Key]
        public int ScreenId { get; set; }

       
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        
        [ForeignKey("Theater")]
        public int TheaterId { get; set; }

        
        public virtual TheaterModel Theater { get; set; }

        // A Screen can have multiple shows
        public virtual ICollection<ShowModel> Shows { get; set; }
    }
}
