using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingApp.Data;
using MovieTicketBookingApp.Models;

namespace MovieTicketBookingApp.Controllers
{
    public class ScreenController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ScreenController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Screen
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Screens.Include(s => s.Theater);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Screen/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Screens == null)
            {
                return NotFound();
            }

            var screenModel = await _context.Screens
                .Include(s => s.Theater)
                .FirstOrDefaultAsync(m => m.ScreenId == id);
            if (screenModel == null)
            {
                return NotFound();
            }

            return View(screenModel);
        }

        // GET: Screen/Create
        public IActionResult Create()
        {
            ViewData["TheaterId"] = new SelectList(_context.Theater, "TheaterId", "Name");
            return View();
        }

        // POST: Screen/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScreenId,Name,Description,TheaterId")] ScreenModel screenModel)
        {
            //if (ModelState.IsValid)
            //{
                
            //}

            try
            {
                _context.Add(screenModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }
            ViewData["TheaterId"] = new SelectList(_context.Theater, "TheaterId", "Address", screenModel.TheaterId);
            return View(screenModel);

        }

        // GET: Screen/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Screens == null)
            {
                return NotFound();
            }

            var screenModel = await _context.Screens.FindAsync(id);
            if (screenModel == null)
            {
                return NotFound();
            }

            Console.WriteLine("Theater id from screen model" + screenModel.TheaterId);
           try
            {
                ViewData["TheaterId"] = new SelectList(_context.Theater, "TheaterId", "Address", screenModel.TheaterId);
                Console.WriteLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return View(screenModel);
        }

        // POST: Screen/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ScreenId,Name,Description,TheaterId")] ScreenModel screenModel)
        {
            if (id != screenModel.ScreenId)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                try
                {
                    _context.Update(screenModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScreenModelExists(screenModel.ScreenId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            //}
            ViewData["TheaterId"] = new SelectList(_context.Theater, "TheaterId", "Address", screenModel.TheaterId);
            return View(screenModel);
        }

        // GET: Screen/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Screens == null)
            {
                return NotFound();
            }

            var screenModel = await _context.Screens
                .Include(s => s.Theater)
                .FirstOrDefaultAsync(m => m.ScreenId == id);
            if (screenModel == null)
            {
                return NotFound();
            }

            return View(screenModel);
        }

        // POST: Screen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Screens == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Screens'  is null.");
            }
            var screenModel = await _context.Screens.FindAsync(id);
            if (screenModel != null)
            {
                _context.Screens.Remove(screenModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScreenModelExists(int id)
        {
          return (_context.Screens?.Any(e => e.ScreenId == id)).GetValueOrDefault();
        }
    }
}
