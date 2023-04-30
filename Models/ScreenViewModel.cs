using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieTicketBookingApp.Models
{
    public class ScreenViewModel
    {
        [Key]
        public int Id { get; set; }
        public ScreenModel Screen { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> Theaters{ get; set; }
    }
}
