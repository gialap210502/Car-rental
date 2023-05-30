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
    public class CategoryController : Controller
    {
        private readonly Car_rentalContext _context;

        public CategoryController(Car_rentalContext context)
        {
            _context = context;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            return _context.category != null ?
                        View(await _context.category.ToListAsync()) :
                        Problem("Entity set 'Car_rentalContext.category'  is null.");
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.category == null)
            {
                return NotFound();
            }

            var category = await _context.category
                .FirstOrDefaultAsync(m => m.id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,type,Status,Deleted_Status")] category category)
        {
            if (ModelState.IsValid)
            {
                var catCheck = _context.category.Where(i => i.type == category.type).ToList();
                if (catCheck.Count() >= 1)
                {
                    ViewBag.ErrorMessage = "The vehicle type has been exist!";
                    return View();
                }
                else
                {
                    category.Status = 1;
                    category.Deleted_Status = 0;
                    _context.Add(category);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.category == null)
            {
                return NotFound();
            }

            var category = await _context.category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(bool updateCheckBox,int id, [Bind("id,type,Status,Deleted_Status")] category category)
        {
            if (id != category.id)
            {
                return NotFound();
            }
            var categoryTemp = await _context.category.FindAsync(id);
             _context.Entry(categoryTemp).State = EntityState.Detached;
            if (ModelState.IsValid)
            {
                try
                {
                    if(updateCheckBox == true){
                        category.Status = 1;
                    }else{
                        category.Status = categoryTemp.Status;
                    }
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!categoryExists(category.id))
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
            return View(category);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.category == null)
            {
                return NotFound();
            }

            var category = await _context.category
                .FirstOrDefaultAsync(m => m.id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.category == null)
            {
                return Problem("Entity set 'Car_rentalContext.category'  is null.");
            }
            var category = await _context.category.FindAsync(id);
            if (category != null)
            {
                category.Status = 0;
                category.Deleted_Status = 1;
                _context.Update(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool categoryExists(int id)
        {
            return (_context.category?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
