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
    public class RatingController : Controller
    {
        private readonly Car_rentalContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RatingController(Car_rentalContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Rating
        public async Task<IActionResult> Index()
        {
            var car_rentalContext = _context.rating.Include(r => r.car).Include(r => r.user);
            return View(await car_rentalContext.ToListAsync());
        }
        public IActionResult Rate()
        {
            ViewData["carId"] = new SelectList(_context.Car, "id", "id");
            ViewData["userId"] = new SelectList(_context.user, "id", "email");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rate(int carId, [Bind("id,dateRating,Status,Star,comment,carId,userId")] rating rating)
        {
            var userId = HttpContext.Session.GetInt32("_ID").GetValueOrDefault();
            var existRatingCheck = _context.rating.Where(i => i.carId == carId && i.userId == userId).ToList();
            if(existRatingCheck.Count() == 0){
                rating.Status = 1;
                rating.carId = carId;
                rating.userId = userId;
                rating.dateRating = DateTime.Now;
                _context.Add(rating);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "rating", new { id = carId });
            }
            if(existRatingCheck.Count() > 0){
                var ratingExists = _context.rating.Where(i => i.carId == carId && i.userId == userId).FirstOrDefault();
                ratingExists.dateRating = DateTime.Now;
                ratingExists.comment = rating.comment;
                ratingExists.Star = rating.Star;
                _context.Update(ratingExists);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Car", new { id = carId });
            }
            return RedirectToAction("Login", "User");
        }

        // GET: Rating/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.rating == null)
            {
                return NotFound();
            }

            var rating = await _context.rating
                .Include(r => r.car)
                .Include(r => r.user)
                .FirstOrDefaultAsync(m => m.id == id);
            if (rating == null)
            {
                return NotFound();
            }

            return View(rating);
        }

        // GET: Rating/Create
        public IActionResult Create()
        {
            ViewData["carId"] = new SelectList(_context.Car, "id", "id");
            ViewData["userId"] = new SelectList(_context.user, "id", "email");
            return View();
        }

        // POST: Rating/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,dateRating,Status,Star,comment,carId,userId")] rating rating)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rating);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["carId"] = new SelectList(_context.Car, "id", "id", rating.carId);
            ViewData["userId"] = new SelectList(_context.user, "id", "email", rating.userId);
            return View(rating);
        }

        // GET: Rating/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.rating == null)
            {
                return NotFound();
            }

            var rating = await _context.rating.FindAsync(id);
            if (rating == null)
            {
                return NotFound();
            }
            ViewData["carId"] = new SelectList(_context.Car, "id", "id", rating.carId);
            ViewData["userId"] = new SelectList(_context.user, "id", "email", rating.userId);
            return View(rating);
        }

        // POST: Rating/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,dateRating,Status,Star,comment,carId,userId")] rating rating)
        {
            if (id != rating.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rating);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ratingExists(rating.id))
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
            ViewData["carId"] = new SelectList(_context.Car, "id", "id", rating.carId);
            ViewData["userId"] = new SelectList(_context.user, "id", "email", rating.userId);
            return View(rating);
        }

        // GET: Rating/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.rating == null)
            {
                return NotFound();
            }

            var rating = await _context.rating
                .Include(r => r.car)
                .Include(r => r.user)
                .FirstOrDefaultAsync(m => m.id == id);
            if (rating == null)
            {
                return NotFound();
            }

            return View(rating);
        }

        // POST: Rating/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.rating == null)
            {
                return Problem("Entity set 'Car_rentalContext.rating'  is null.");
            }
            var rating = await _context.rating.FindAsync(id);
            if (rating != null)
            {
                _context.rating.Remove(rating);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ratingExists(int id)
        {
            return (_context.rating?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
