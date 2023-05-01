using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingApp.Data;
using MovieTicketBookingApp.Data.Migrations;
using MovieTicketBookingApp.Models;

namespace MovieTicketBookingApp.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Booking
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Bookings.Include(b => b.Show).Include(b => b.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Booking/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var bookingModel = await _context.Bookings
                .Include(b => b.Show)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (bookingModel == null)
            {
                return NotFound();
            }

            return View(bookingModel);
        }

        // GET: Booking/Create
        public IActionResult Create()
        {
            ViewData["ShowId"] = new SelectList(_context.Show, "ShowId", "ShowId");
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Booking/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,BookedAt,ShowId,UserId")] BookingModel bookingModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookingModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ShowId"] = new SelectList(_context.Show, "ShowId", "ShowId", bookingModel.ShowId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", bookingModel.UserId);
            return View(bookingModel);
        }

        // GET: Booking/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var bookingModel = await _context.Bookings.FindAsync(id);
            if (bookingModel == null)
            {
                return NotFound();
            }
            ViewData["ShowId"] = new SelectList(_context.Show, "ShowId", "ShowId", bookingModel.ShowId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", bookingModel.UserId);
            return View(bookingModel);
        }

        // GET: Booking/ShowMyBookings
        [Authorize(Roles ="Customer")]
        [HttpGet]
        public  IActionResult ShowMyBookings()
        {

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var bookings = await _context.Bookings.Include(b => b.Show).Include(b => b.User).Where(b=>b.UserId == userId ).ToListAsync();
            var bookings =  from booking in _context.Bookings
                       where booking.UserId == userId
                       join
                       myshow in _context.Show on booking.ShowId equals myshow.ShowId
                       join
                        movie in _context.Movies on myshow.MovieId equals movie.movieId
                        join
                        screen in _context.Screens on myshow.ScreenId equals screen.ScreenId
                        join
                        theater in _context.Theater on screen.TheaterId equals theater.TheaterId
                        select (new { bookingId= booking.BookingId, movieId = movie.movieId, movieName = movie.title, theaterId = theater.TheaterId, theaterAddress = theater.Address, theaterName = theater.Name, screenName = screen.Name, showId = myshow.ShowId, price = myshow.Price, showTiming = myshow.ShowTime })
                              
                              ;
            ViewBag.bookings =  bookings;
            Console.WriteLine();
            if (bookings.ToArray().Length == 0)
            {
                ViewBag.bookings = null;
            }
            return View();
        }



        // POST: Booking/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,BookedAt,ShowId,UserId")] BookingModel bookingModel)
        {
            if (id != bookingModel.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookingModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingModelExists(bookingModel.BookingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ShowId"] = new SelectList(_context.Show, "ShowId", "ShowId", bookingModel.ShowId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", bookingModel.UserId);
            return View(bookingModel);
        }

        // GET: Booking/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var bookingModel = await _context.Bookings
                .Include(b => b.Show)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (bookingModel == null)
            {
                return NotFound();
            }

            return View(bookingModel);
        }

        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bookings == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Bookings'  is null.");
            }
            var bookingModel = await _context.Bookings.FindAsync(id);
            if (bookingModel != null)
            {
                _context.Bookings.Remove(bookingModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingModelExists(int id)
        {
          return (_context.Bookings?.Any(e => e.BookingId == id)).GetValueOrDefault();
        }
    }
}
