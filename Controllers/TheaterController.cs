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
    public class TheaterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TheaterController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Theater
        public async Task<IActionResult> Index()
        {
              return _context.Theater != null ? 
                          View(await _context.Theater.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Theater'  is null.");
        }

        // GET: Theater/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Theater == null)
            {
                return NotFound();
            }

            var theaterModel = await _context.Theater
                .FirstOrDefaultAsync(m => m.TheaterId == id);
            if (theaterModel == null)
            {
                return NotFound();
            }

            return View(theaterModel);
        }

        // GET: Theater/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Theater/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TheaterId,Name,Address")] TheaterModel theaterModel)
        {
            //if (ModelState.IsValid)
            //{
                
            //}
            try
            {
                _context.Add(theaterModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(theaterModel);
        }

        // GET: Theater/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Theater == null)
            {
                return NotFound();
            }

            var theaterModel = await _context.Theater.FindAsync(id);
            if (theaterModel == null)
            {
                return NotFound();
            }
            return View(theaterModel);
        }

        // POST: Theater/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TheaterId,Name,Address")] TheaterModel theaterModel)
        {
            if (id != theaterModel.TheaterId)
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
                    _context.Update(theaterModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TheaterModelExists(theaterModel.TheaterId))
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
            return View(theaterModel);
        }

        // GET: Theater/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Theater == null)
            {
                return NotFound();
            }

            var theaterModel = await _context.Theater
                .FirstOrDefaultAsync(m => m.TheaterId == id);
            if (theaterModel == null)
            {
                return NotFound();
            }

            return View(theaterModel);
        }

        // POST: Theater/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Theater == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Theater'  is null.");
            }
            var theaterModel = await _context.Theater.FindAsync(id);
            if (theaterModel != null)
            {
                _context.Theater.Remove(theaterModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TheaterModelExists(int id)
        {
          return (_context.Theater?.Any(e => e.TheaterId == id)).GetValueOrDefault();
        }
    }
}
