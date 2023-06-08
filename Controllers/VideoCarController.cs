using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Car_rental.Data;
using Car_rental.Models;

namespace Car_rental.Controllers
{
    public class VideoCarController : Controller
    {
        private readonly Car_rentalContext _context;

        public VideoCarController(Car_rentalContext context)
        {
            _context = context;
        }

        // GET: VideoCar
        public async Task<IActionResult> Index()
        {
            var car_rentalContext = _context.VideoCar.Include(v => v.car);
            return View(await car_rentalContext.ToListAsync());
        }

        // GET: VideoCar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.VideoCar == null)
            {
                return NotFound();
            }

            var videoCar = await _context.VideoCar
                .Include(v => v.car)
                .FirstOrDefaultAsync(m => m.id == id);
            if (videoCar == null)
            {
                return NotFound();
            }

            return View(videoCar);
        }

        // GET: VideoCar/Create
        public IActionResult Create()
        {
            ViewData["carId"] = new SelectList(_context.Car, "id", "id");
            return View();
        }

        // POST: VideoCar/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,nameFile,carId")] VideoCar videoCar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(videoCar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["carId"] = new SelectList(_context.Car, "id", "id", videoCar.carId);
            return View(videoCar);
        }

        // GET: VideoCar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.VideoCar == null)
            {
                return NotFound();
            }

            var videoCar = await _context.VideoCar.FindAsync(id);
            if (videoCar == null)
            {
                return NotFound();
            }
            ViewData["carId"] = new SelectList(_context.Car, "id", "id", videoCar.carId);
            return View(videoCar);
        }

        // POST: VideoCar/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,nameFile,carId")] VideoCar videoCar)
        {
            if (id != videoCar.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(videoCar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoCarExists(videoCar.id))
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
            ViewData["carId"] = new SelectList(_context.Car, "id", "id", videoCar.carId);
            return View(videoCar);
        }

        // GET: VideoCar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.VideoCar == null)
            {
                return NotFound();
            }

            var videoCar = await _context.VideoCar
                .Include(v => v.car)
                .FirstOrDefaultAsync(m => m.id == id);
            if (videoCar == null)
            {
                return NotFound();
            }

            return View(videoCar);
        }

        // POST: VideoCar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.VideoCar == null)
            {
                return Problem("Entity set 'Car_rentalContext.VideoCar'  is null.");
            }
            var videoCar = await _context.VideoCar.FindAsync(id);
            if (videoCar != null)
            {
                _context.VideoCar.Remove(videoCar);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideoCarExists(int id)
        {
          return _context.VideoCar.Any(e => e.id == id);
        }
    }
}
