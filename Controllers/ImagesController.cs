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
    public class ImagesController : Controller
    {
        private readonly Car_rentalContext _context;

        public ImagesController(Car_rentalContext context)
        {
            _context = context;
        }

        // GET: Images
        public async Task<IActionResult> Index()
        {
            var car_rentalContext = _context.Images.Include(i => i.car);
            return View(await car_rentalContext.ToListAsync());
        }

        // GET: Images/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Images == null)
            {
                return NotFound();
            }

            var images = await _context.Images
                .Include(i => i.car)
                .FirstOrDefaultAsync(m => m.id == id);
            if (images == null)
            {
                return NotFound();
            }

            return View(images);
        }

        // GET: Images/Create
        public IActionResult Create()
        {
            ViewData["carId"] = new SelectList(_context.Car, "id", "id");
            return View();
        }

        // POST: Images/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,nameFile,carId")] Images images)
        {
            if (ModelState.IsValid)
            {
                _context.Add(images);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["carId"] = new SelectList(_context.Car, "id", "id", images.carId);
            return View(images);
        }

        // GET: Images/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Images == null)
            {
                return NotFound();
            }

            var images = await _context.Images.FindAsync(id);
            if (images == null)
            {
                return NotFound();
            }
            ViewData["carId"] = new SelectList(_context.Car, "id", "id", images.carId);
            return View(images);
        }

        // POST: Images/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,nameFile,carId")] Images images)
        {
            if (id != images.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(images);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImagesExists(images.id))
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
            ViewData["carId"] = new SelectList(_context.Car, "id", "id", images.carId);
            return View(images);
        }

        // GET: Images/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Images == null)
            {
                return NotFound();
            }

            var images = await _context.Images
                .Include(i => i.car)
                .FirstOrDefaultAsync(m => m.id == id);
            if (images == null)
            {
                return NotFound();
            }

            return View(images);
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Images == null)
            {
                return Problem("Entity set 'Car_rentalContext.Images'  is null.");
            }
            var images = await _context.Images.FindAsync(id);
            if (images != null)
            {
                _context.Images.Remove(images);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImagesExists(int id)
        {
          return _context.Images.Any(e => e.id == id);
        }
    }
}
