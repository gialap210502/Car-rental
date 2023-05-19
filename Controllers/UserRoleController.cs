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
    public class UserRoleController : Controller
    {
        private readonly Car_rentalContext _context;

        public UserRoleController(Car_rentalContext context)
        {
            _context = context;
        }

        // GET: UserRole
        public async Task<IActionResult> Index()
        {
            var car_rentalContext = _context.userRole.Include(u => u.role).Include(u => u.user);
            return View(await car_rentalContext.ToListAsync());
        }

        // GET: UserRole/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.userRole == null)
            {
                return NotFound();
            }

            var userRole = await _context.userRole
                .Include(u => u.role)
                .Include(u => u.user)
                .FirstOrDefaultAsync(m => m.id == id);
            if (userRole == null)
            {
                return NotFound();
            }

            return View(userRole);
        }

        // GET: UserRole/Create
        public IActionResult Create()
        {
            ViewData["roleId"] = new SelectList(_context.roles, "id", "role");
            ViewData["userId"] = new SelectList(_context.user, "id", "email");
            return View();
        }

        // POST: UserRole/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,userId,roleId")] userRole userRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["roleId"] = new SelectList(_context.roles, "id", "role", userRole.roleId);
            ViewData["userId"] = new SelectList(_context.user, "id", "email", userRole.userId);
            return View(userRole);
        }

        // GET: UserRole/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.userRole == null)
            {
                return NotFound();
            }

            var userRole = await _context.userRole.FindAsync(id);
            if (userRole == null)
            {
                return NotFound();
            }
            ViewData["roleId"] = new SelectList(_context.roles, "id", "role", userRole.roleId);
            ViewData["userId"] = new SelectList(_context.user, "id", "email", userRole.userId);
            return View(userRole);
        }

        // POST: UserRole/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,userId,roleId")] userRole userRole)
        {
            if (id != userRole.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!userRoleExists(userRole.id))
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
            ViewData["roleId"] = new SelectList(_context.roles, "id", "role", userRole.roleId);
            ViewData["userId"] = new SelectList(_context.user, "id", "email", userRole.userId);
            return View(userRole);
        }

        // GET: UserRole/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.userRole == null)
            {
                return NotFound();
            }

            var userRole = await _context.userRole
                .Include(u => u.role)
                .Include(u => u.user)
                .FirstOrDefaultAsync(m => m.id == id);
            if (userRole == null)
            {
                return NotFound();
            }

            return View(userRole);
        }

        // POST: UserRole/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.userRole == null)
            {
                return Problem("Entity set 'Car_rentalContext.userRole'  is null.");
            }
            var userRole = await _context.userRole.FindAsync(id);
            if (userRole != null)
            {
                _context.userRole.Remove(userRole);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool userRoleExists(int id)
        {
          return (_context.userRole?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
