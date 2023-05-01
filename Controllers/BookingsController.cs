using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingApp.Data;
using MovieTicketBookingApp.Models;

namespace MovieTicketBookingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookingModel(int id)
        {
            if (_context.Bookings == null)
            {
                return NotFound();
            }
            var bookingModel = await _context.Bookings.FindAsync(id);
            if (bookingModel == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(bookingModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingModelExists(int id)
        {
            return (_context.Bookings?.Any(e => e.BookingId == id)).GetValueOrDefault();
        }
    }
}
