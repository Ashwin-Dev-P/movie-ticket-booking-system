using System;
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
    public class ShowController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShowController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Show
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Show.Include(s => s.Movie).Include(s => s.Screen);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Show/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Show == null)
            {
                return NotFound();
            }

            var showModel = await _context.Show
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                .FirstOrDefaultAsync(m => m.ShowId == id);
            if (showModel == null)
            {
                return NotFound();
            }

            return View(showModel);
        }

        // GET: Show/Create
        public IActionResult Create()
        {
            ViewData["MovieId"] = new SelectList(_context.Movies, "movieId", "title");
            ViewData["ScreenId"] = new SelectList(_context.Screens, "ScreenId", "ScreenId");
            return View();
        }

        // POST: Show/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShowId,ShowTime,ScreenId,MovieId")] ShowModel showModel)
        {
            //if (ModelState.IsValid)
            //{
                
            //}

            try
            {
                _context.Add(showModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "movieId", "title", showModel.MovieId);
            ViewData["ScreenId"] = new SelectList(_context.Screens, "ScreenId", "ScreenId", showModel.ScreenId);
            return View(showModel);
        }

        // GET: Show/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Show == null)
            {
                return NotFound();
            }

            var showModel = await _context.Show.FindAsync(id);
            if (showModel == null)
            {
                return NotFound();
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "movieId", "title", showModel.MovieId);
            ViewData["ScreenId"] = new SelectList(_context.Screens, "ScreenId", "ScreenId", showModel.ScreenId);
            return View(showModel);
        }

        // POST: Show/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShowId,ShowTime,ScreenId,MovieId")] ShowModel showModel)
        {
            if (id != showModel.ShowId)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                
            //}

            try
            {
                try
                {
                    _context.Update(showModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShowModelExists(showModel.ShowId))
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
                Console.WriteLine("Error:" + ex.Message);
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "movieId", "title", showModel.MovieId);
            ViewData["ScreenId"] = new SelectList(_context.Screens, "ScreenId", "ScreenId", showModel.ScreenId);
            return View(showModel);
        }

        // GET: Show/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Show == null)
            {
                return NotFound();
            }

            var showModel = await _context.Show
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                .FirstOrDefaultAsync(m => m.ShowId == id);
            if (showModel == null)
            {
                return NotFound();
            }

            return View(showModel);
        }

        // POST: Show/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Show == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Show'  is null.");
            }
            var showModel = await _context.Show.FindAsync(id);
            if (showModel != null)
            {
                _context.Show.Remove(showModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShowModelExists(int id)
        {
          return (_context.Show?.Any(e => e.ShowId == id)).GetValueOrDefault();
        }
    }
}
