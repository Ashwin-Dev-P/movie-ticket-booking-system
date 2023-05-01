﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingApp.Data;
using MovieTicketBookingApp.Models;

namespace MovieTicketBookingApp.Controllers
{
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovieController(ApplicationDbContext context)
        {
            _context = context;
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
                              select (new { movieName = movie.title, theaterAddress = theater.Address , theaterName = theater.Name, screenName = screen.Name, showId= ShowId, price= show.Price, showTiming= show.ShowTime  })
                              ).Single()
                              ;

           
            ViewBag.showDetails = showDetails;

            
            return View();
        }

        // POST: Movie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("movieId,title,description")] MovieModel movieModel)
        {
            //if (ModelState.IsValid)
            //{
                
            //}
            try
            {
                _context.Add(movieModel);
                await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Edit(int id, [Bind("movieId,title,description")] MovieModel movieModel)
        {
            if (id != movieModel.movieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
