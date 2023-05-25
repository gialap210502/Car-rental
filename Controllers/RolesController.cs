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
    public class RolesController : Controller
    {
        private readonly Car_rentalContext _context;

        public RolesController(Car_rentalContext context)
        {
            _context = context;
        }

        // GET: Roles
        public async Task<IActionResult> Index()
        {
            return _context.roles != null ?
                        View(await _context.roles.ToListAsync()) :
                        Problem("Entity set 'Car_rentalContext.roles'  is null.");
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.roles == null)
            {
                return NotFound();
            }

            var roles = await _context.roles
                .FirstOrDefaultAsync(m => m.id == id);
            if (roles == null)
            {
                return NotFound();
            }

            return View(roles);
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,role")] roles roles)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roles);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(roles);
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.roles == null)
            {
                return NotFound();
            }

            var roles = await _context.roles.FindAsync(id);
            if (roles == null)
            {
                return NotFound();
            }
            return View(roles);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,role")] roles roles)
        {
            if (id != roles.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roles);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!rolesExists(roles.id))
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
            return View(roles);
        }

        // GET: Roles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.roles == null)
            {
                return NotFound();
            }

            var roles = await _context.roles
                .FirstOrDefaultAsync(m => m.id == id);
            if (roles == null)
            {
                return NotFound();
            }

            return View(roles);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.roles == null)
            {
                return Problem("Entity set 'Car_rentalContext.roles'  is null.");
            }
            var roles = await _context.roles.FindAsync(id);
            if (roles != null)
            {
                _context.roles.Remove(roles);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool rolesExists(int id)
        {
            return (_context.roles?.Any(e => e.id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> CreateRole()
        {
            if (!_context.roles.Any())
            {
                _context.roles.Add(new roles { role = "Admin" });
                _context.roles.Add(new roles { role = "Owner" });
                _context.roles.Add(new roles { role = "User" });
                await _context.SaveChangesAsync();
                return RedirectToAction("index", "roles");
            }
            return View();
        }
    }
}
