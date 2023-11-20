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
        public async Task<IActionResult> AddImg()
        {
            ViewBag.Layout = "_AdminLayout";
            _context.Images.Add(new Images { nameFile = "1.jpg", carId = 1 });
            _context.Images.Add(new Images { nameFile = "2.jpg", carId = 1 });
            _context.Images.Add(new Images { nameFile = "3.png", carId = 2 });
            _context.Images.Add(new Images { nameFile = "4.jpg", carId = 3 });
            _context.Images.Add(new Images { nameFile = "5.png", carId = 3 });
            _context.Images.Add(new Images { nameFile = "6.jpg", carId = 4 });
            _context.Images.Add(new Images { nameFile = "7.jpg", carId = 5 });
            _context.Images.Add(new Images { nameFile = "8.jpg", carId = 5 });

            _context.Images.Add(new Images { nameFile = "9.jpg", carId = 6 });
            _context.Images.Add(new Images { nameFile = "11.jpg", carId = 7 });
            _context.Images.Add(new Images { nameFile = "12.jpg", carId = 7 });
            _context.Images.Add(new Images { nameFile = "13.jpeg", carId = 8 });
            _context.Images.Add(new Images { nameFile = "14.jpg", carId = 9 });
            _context.Images.Add(new Images { nameFile = "15.jpg", carId = 9 });
            _context.Images.Add(new Images { nameFile = "16.jpg", carId = 10 });

            _context.Images.Add(new Images { nameFile = "17.jpg", carId = 11 });
            _context.Images.Add(new Images { nameFile = "18.jpg", carId = 11 });
            _context.Images.Add(new Images { nameFile = "19.jpg", carId = 12 });
            _context.Images.Add(new Images { nameFile = "20.jpg", carId = 13 });
            _context.Images.Add(new Images { nameFile = "21.jpg", carId = 14 });
            _context.Images.Add(new Images { nameFile = "22.jpg", carId = 15 });


            _context.SaveChanges();
            return RedirectToAction("Home", "car");
        }
    }
}
