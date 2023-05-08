using System;
using System.Collections.Generic;
using System.Data;
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
using static System.Net.Mime.MediaTypeNames;

// Email service
using MovieTicketBookingApp.Interfaces;

namespace MovieTicketBookingApp.Controllers
{
    //[Authorize(Roles = "Admin,Customer")]
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender emailSender;

        public MovieController(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            this.emailSender = emailSender;
        }

        // GET: Movie
        public async Task<IActionResult> Index()
        {
              return _context.Movies != null ? 
                          View(await _context.Movies.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Movies'  is null.");
        }

        // GET: Movie
        public  IActionResult GetTheatersWithMovie(int id)
        {
            var theaters = from show in _context.Show join screen in _context.Screens on show.ScreenId equals screen.ScreenId join theater in _context.Theater on screen.TheaterId equals theater.TheaterId where(show.MovieId == id)  select (theater)  ;
            
            ViewBag.theaterCount = theaters.ToList().Count;
            

            return View(theaters);
            //return View(await _context.Show.Where(show => show.MovieId == id).Include("Theater").ToListAsync());
                        
        }

        // Get: Screens
        [HttpGet]
        public IActionResult ViewScreens([FromQuery(Name = "TheaterId")] int TheaterId, [FromQuery(Name = "MovieId")] int MovieId)
        {
            //var screens = from show in _context.Show join screen in _context.Screens on show.ScreenId equals screen.ScreenId join theater in _context.Theater on screen.TheaterId equals theater.TheaterId where (show.MovieId == id) select (theater);
            var screensAndTimings = from myscreen in _context.Screens where (myscreen.TheaterId == TheaterId) join show in _context.Show  on myscreen.ScreenId equals show.ScreenId where ( show.MovieId == MovieId) //join movie in _context.Movies on show.MovieId equals movie.movieId
                          select ( new { showId=show.ShowId,  showTime = show.ShowTime, screenName= myscreen.Name, screenDescription = myscreen.Description , price = show.Price }   );

            
            ViewBag.screensAndTimings = screensAndTimings;
            return View();
            //return View(await _context.Show.Where(show => show.MovieId == id).Include("Theater").ToListAsync());

        }

        // GET: Movie/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movieModel = await _context.Movies
                .FirstOrDefaultAsync(m => m.movieId == id);
            if (movieModel == null)
            {
                return NotFound();
            }

            return View(movieModel);
        }

        // GET: Movie/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles ="Customer")]
        // GET: Movie/ConfirmBooking/5
        //[HttpGet("/{ShowId:int}")]
        [HttpGet]
        public IActionResult ConfirmBooking(int id)
        {
            
            int ShowId = id;
            
            var showDetails = (from show in _context.Show
                              where (show.ShowId == ShowId)
                              join 
                              movie in _context.Movies on show.MovieId equals movie.movieId
                              join
                              screen in _context.Screens on show.ScreenId equals screen.ScreenId
                              join
                              theater in _context.Theater on screen.TheaterId equals theater.TheaterId
                              select (new { movieId = movie.movieId, movieName = movie.title, theaterId= theater.TheaterId, theaterAddress = theater.Address , theaterName = theater.Name, screenName = screen.Name, showId= ShowId, price= show.Price, showTiming= show.ShowTime  })
                              ).Single()
                              ;

           
            ViewBag.showDetails = showDetails;
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.UserId = userId;
            
            return View();
        }

        // POST: Movie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("movieId,title,description")] MovieModel movieModel, IFormFile moviepicture)
        {

            // Check whether the image is selected
            ViewBag.errorMsg = null;
            if ( moviepicture == null)
            {
                ViewBag.errorMsg = "Please select a picture";
                return View(movieModel);
            }
            
            

            //if (ModelState.IsValid)
            //{

            //}
            try
            {
                // get extension of picture
                string ext = Path.GetExtension(this.Request.Form.Files[0].FileName);

                movieModel.imageExtension = ext;

                _context.Add(movieModel);
                await _context.SaveChangesAsync();


                // Generate name for the file
                int movieId = movieModel.movieId;
                string fileName = Convert.ToString(movieId) + ext;

                // Create path and stream it to the location
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\img\uploads\movies\", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    this.Request.Form.Files[0].CopyTo(stream);
                }

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);
            }
            return View(movieModel);
        }

        
        


        // GET: Movie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movieModel = await _context.Movies.FindAsync(id);
            if (movieModel == null)
            {
                return NotFound();
            }
            return View(movieModel);
        }

        // POST: Movie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("movieId,title,description,imageExtension")] MovieModel movieModel)
        {
            if (id != movieModel.movieId)
            {
                return NotFound();
            }

            
            try
            {
                try
                {

                    Console.WriteLine(movieModel);
                    _context.Update(movieModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieModelExists(movieModel.movieId))
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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(movieModel);
        }

        // GET: Movie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movieModel = await _context.Movies
                .FirstOrDefaultAsync(m => m.movieId == id);
            if (movieModel == null)
            {
                return NotFound();
            }

            return View(movieModel);
        }

        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movies == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Movies'  is null.");
            }
            var movieModel = await _context.Movies.FindAsync(id);
            if (movieModel != null)
            {
                // Send mail to all users who have booked show for the movie
                var customers = from show in _context.Show
                                 where show.MovieId == id
                                 join
                                 booking in _context.Bookings on show.ShowId equals booking.ShowId
                                 join
                                 user in _context.AspNetUsers on booking.UserId equals user.Id
                                 select (
                                        new {
                                            customerData = user.EmailConfirmed ? 
                                            
                                            new
                                            {
                                                email = user.Email,
                                                body = $"Your booking {booking.BookingId} for the show at {show.ShowTime} has been cancelled."

                                            }
                                            :
                                            null
                                            ,
                                            }
                                    )
                                 ;


                string subject = "Movie booking cancelled";
                
                foreach (var customer in customers)
                {

                    Console.WriteLine($"Customer {customer.customerData.email}");
                    await emailSender.SendEmailAsync(customer.customerData.email, subject, customer.customerData.body);


                }

                // Generate name for the file
                int movieId = movieModel.movieId;
                string ext = movieModel.imageExtension;
                string fileName = Convert.ToString(movieId) + ext;

                // Create path and delete it from the location
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\img\uploads\movies\", fileName);
                System.IO.File.Delete(filePath);

                _context.Movies.Remove(movieModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieModelExists(int id)
        {
          return (_context.Movies?.Any(e => e.movieId == id)).GetValueOrDefault();
        }
    }
}
