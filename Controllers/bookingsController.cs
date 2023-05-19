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
    public class bookingsController : Controller
    {
        private readonly Car_rentalContext _context;

        public bookingsController(Car_rentalContext context)
        {
            _context = context;
        }

        // GET: bookings
        public async Task<IActionResult> Index()
        {
            var car_rentalContext = _context.bookings.Include(b => b.user);
            return View(await car_rentalContext.ToListAsync());
        }

        // GET: bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.bookings == null)
            {
                return NotFound();
            }

            var bookings = await _context.bookings
                .Include(b => b.user)
                .FirstOrDefaultAsync(m => m.id == id);
            if (bookings == null)
            {
                return NotFound();
            }

            return View(bookings);
        }

        // GET: bookings/Create
        public IActionResult Create()
        {
            ViewData["userId"] = new SelectList(_context.user, "id", "email");
            return View();
        }

        // POST: bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,startDate,endDate,totalAmount,userId")] bookings bookings)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookings);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["userId"] = new SelectList(_context.user, "id", "email", bookings.userId);
            return View(bookings);
        }

        // GET: bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.bookings == null)
            {
                return NotFound();
            }

            var bookings = await _context.bookings.FindAsync(id);
            if (bookings == null)
            {
                return NotFound();
            }
            ViewData["userId"] = new SelectList(_context.user, "id", "email", bookings.userId);
            return View(bookings);
        }

        // POST: bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,startDate,endDate,totalAmount,userId")] bookings bookings)
        {
            if (id != bookings.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookings);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!bookingsExists(bookings.id))
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
            ViewData["userId"] = new SelectList(_context.user, "id", "email", bookings.userId);
            return View(bookings);
        }

        // GET: bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.bookings == null)
            {
                return NotFound();
            }

            var bookings = await _context.bookings
                .Include(b => b.user)
                .FirstOrDefaultAsync(m => m.id == id);
            if (bookings == null)
            {
                return NotFound();
            }

            return View(bookings);
        }

        // POST: bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.bookings == null)
            {
                return Problem("Entity set 'Car_rentalContext.bookings'  is null.");
            }
            var bookings = await _context.bookings.FindAsync(id);
            if (bookings != null)
            {
                _context.bookings.Remove(bookings);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool bookingsExists(int id)
        {
          return (_context.bookings?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
