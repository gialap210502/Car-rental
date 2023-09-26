using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Car_rental.Data;
using Car_rental.Models;
using Car_rental.Untils;

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
            ViewBag.layout = "_AdminLayout";
            return _context.roles != null ?
                        View(await _context.roles.ToListAsync()) :
                        Problem("Entity set 'Car_rentalContext.roles'  is null.");
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.layout = "_AdminLayout";
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
            ViewBag.layout = "_AdminLayout";
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,role")] roles roles)
        {
            ViewBag.layout = "_AdminLayout";
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
            ViewBag.layout = "_AdminLayout";
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
            ViewBag.layout = "_AdminLayout";
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
            ViewBag.layout = "_AdminLayout";
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
            ViewBag.layout = "_AdminLayout";
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
            ViewBag.layout = "_AdminLayout";
            return (_context.roles?.Any(e => e.id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> CreateRole()
        {
            ViewBag.layout = "_AdminLayout";
            var Encode = new Encode();
            if (!_context.roles.Any())
            {
                var user1 = new user
                {
                    name = "Lap Duong",
                    citizen_identification = "123456789",
                    driver_license = "ABC123",
                    phone = "123456789",
                    dob = new DateTime(1990, 1, 1),
                    email = "lap@gmail.com",
                    address = "AnPhu,AnGiang",
                    password = Encode.encode("123"),
                    flag = 0,
                    image = "@@@@.png",
                    coins = 3000000
                };

                var category1 = new category
                {
                    type = "Sedan",
                    Status = 1,
                    Deleted_Status = 0
                };
                _context.roles.Add(new roles { role = "Admin" });
                _context.roles.Add(new roles { role = "Owner" });
                _context.roles.Add(new roles { role = "User" });
                _context.user.Add(user1);

                _context.category.Add(category1);

                await _context.SaveChangesAsync();

                var setRole = new userRole
                {
                    userId = 1,
                    roleId = 1
                };
                _context.userRole.Add(setRole);
                var discount1 = new discount
                {
                    code = "SUMMER2023",
                    percentage = 10,
                    startDate = new DateTime(2023, 6, 1),
                    endDate = new DateTime(2023, 8, 31),
                    userId = 1
                };
                _context.discount.Add(discount1);
                await _context.SaveChangesAsync();


                return RedirectToAction("CreateCar", "Car");
            }
            return View();
        }
    }
}
