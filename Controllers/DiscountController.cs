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
        public async Task<IActionResult> Index(int pg = 1)
        {
            ViewBag.layout = "_AdminLayout";
                        const int pageSize = 5;
            if (pg < 1)
                pg = 1;
            int recsCount = _context.discount.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            this.ViewBag.Pager = pager;
            var data = _context.discount.Skip(recSkip).Take(pager.PageSize);
            return View(await data.ToListAsync());
        }
        public async Task<IActionResult> Discounts(int id, int pg = 1)
        {
            ViewBag.layout = "_AdminLayout";
                        const int pageSize = 5;
            if (pg < 1)
                pg = 1;
            int recsCount = _context.discount.Where(x => x.userId == id).Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            this.ViewBag.Pager = pager;
            var data = _context.discount.Where(x => x.userId == id).Skip(recSkip).Take(pager.PageSize);
            return View(await data.ToListAsync());
        }

        // GET: Discount/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.layout = "_AdminLayout";
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
            ViewBag.layout = "_AdminLayout";
            return View();
        }

        // POST: Discount/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,code,percentage,startDate,endDate,userId")] discount discount)
        {
            ViewBag.layout = "_AdminLayout";
            if (discount.startDate <= DateTime.Now || discount.endDate <= DateTime.Now)
            {
                ViewBag.message = "Start date and end date must after today 1 day";
                return View(discount);
            }
            if (discount.endDate <= discount.startDate)
            {
                ViewBag.message = "End date must after start date 1 day";
                return View(discount);
            }
            // if (discount.percentage > 0 && discount.percentage <= 100)
            // {
            //     ViewBag.message = "Percentage must be between 0 and 100!";
            //     return View(discount);
            // }
            if (ModelState.IsValid && discount.endDate > discount.startDate && discount.startDate > DateTime.Now || discount.endDate > DateTime.Now)
            {
                _context.Add(discount);
                await _context.SaveChangesAsync();
                return RedirectToAction("Discounts", new { id = discount.userId });
            }
            return View(discount);
        }

        // GET: Discount/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.layout = "_AdminLayout";
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
            ViewBag.layout = "_AdminLayout";
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
            ViewBag.layout = "_AdminLayout";
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
            ViewBag.layout = "_AdminLayout";
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
            ViewBag.layout = "_AdminLayout";
            return _context.discount.Any(e => e.id == id);
        }
    }
}
