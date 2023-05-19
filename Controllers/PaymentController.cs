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
    public class PaymentController : Controller
    {
        private readonly Car_rentalContext _context;

        public PaymentController(Car_rentalContext context)
        {
            _context = context;
        }

        // GET: Payment
        public async Task<IActionResult> Index()
        {
            var car_rentalContext = _context.payment.Include(p => p.car);
            return View(await car_rentalContext.ToListAsync());
        }

        // GET: Payment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.payment == null)
            {
                return NotFound();
            }

            var payment = await _context.payment
                .Include(p => p.car)
                .FirstOrDefaultAsync(m => m.id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: Payment/Create
        public IActionResult Create()
        {
            ViewData["carId"] = new SelectList(_context.Car, "id", "id");
            return View();
        }

        // POST: Payment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,amount,paymentDate,paymentMethod,carId,booking_id")] payment payment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(payment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["carId"] = new SelectList(_context.Car, "id", "id", payment.carId);
            return View(payment);
        }

        // GET: Payment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.payment == null)
            {
                return NotFound();
            }

            var payment = await _context.payment.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["carId"] = new SelectList(_context.Car, "id", "id", payment.carId);
            return View(payment);
        }

        // POST: Payment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,amount,paymentDate,paymentMethod,carId,booking_id")] payment payment)
        {
            if (id != payment.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!paymentExists(payment.id))
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
            ViewData["carId"] = new SelectList(_context.Car, "id", "id", payment.carId);
            return View(payment);
        }

        // GET: Payment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.payment == null)
            {
                return NotFound();
            }

            var payment = await _context.payment
                .Include(p => p.car)
                .FirstOrDefaultAsync(m => m.id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.payment == null)
            {
                return Problem("Entity set 'Car_rentalContext.payment'  is null.");
            }
            var payment = await _context.payment.FindAsync(id);
            if (payment != null)
            {
                _context.payment.Remove(payment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool paymentExists(int id)
        {
          return (_context.payment?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
