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
    public class DiscountController : Controller
    {
        private readonly Car_rentalContext _context;

        public DiscountController(Car_rentalContext context)
        {
            _context = context;
        }

        // GET: Discount
        public async Task<IActionResult> Index()
        {
              return View(await _context.discount.ToListAsync());
        }

        // GET: Discount/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.discount == null)
            {
                return NotFound();
            }

            var discount = await _context.discount
                .FirstOrDefaultAsync(m => m.id == id);
            if (discount == null)
            {
                return NotFound();
            }

            return View(discount);
        }

        // GET: Discount/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Discount/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,code,percentage,startDate,endDate")] discount discount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(discount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(discount);
        }

        // GET: Discount/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.discount == null)
            {
                return NotFound();
            }

            var discount = await _context.discount.FindAsync(id);
            if (discount == null)
            {
                return NotFound();
            }
            return View(discount);
        }

        // POST: Discount/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,code,percentage,startDate,endDate")] discount discount)
        {
            if (id != discount.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!discountExists(discount.id))
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
            return View(discount);
        }

        // GET: Discount/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.discount == null)
            {
                return NotFound();
            }

            var discount = await _context.discount
                .FirstOrDefaultAsync(m => m.id == id);
            if (discount == null)
            {
                return NotFound();
            }

            return View(discount);
        }

        // POST: Discount/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.discount == null)
            {
                return Problem("Entity set 'Car_rentalContext.discount'  is null.");
            }
            var discount = await _context.discount.FindAsync(id);
            if (discount != null)
            {
                _context.discount.Remove(discount);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool discountExists(int id)
        {
          return _context.discount.Any(e => e.id == id);
        }
    }
}
