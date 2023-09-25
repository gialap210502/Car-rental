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
        public async Task<IActionResult> Index(int pg = 1)
        {
            ViewBag.layout = "_AdminLayout";
            const int pageSize = 5;
            if (pg < 1)
                pg = 1;
            int recsCount = _context.payment.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var car_rentalContext = _context.payment.Include(p => p.booking).ThenInclude(b => b.user).Include(p => p.car).Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(car_rentalContext);
        }
        public async Task<IActionResult> OrderList(int UserId, int pg = 1)
        {
            ViewBag.layout = "_AdminLayout";
            const int pageSize = 5;
            if (pg < 1)
                pg = 1;

            int recsCount = _context.payment.Include(p => p.car).ThenInclude(c => c.user).Where(p => p.car.user.id == UserId).Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var car_rentalContext = _context.payment.Include(p => p.booking).ThenInclude(b => b.user).Include(p => p.car).Where(p => p.car.user.id == UserId).Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(car_rentalContext);
        }

        // GET: Payment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.layout = "_AdminLayout";
            if (id == null || _context.payment == null)
            {
                return NotFound();
            }

            var payment = await _context.payment
                .Include(p => p.booking)
                .Include(p => p.car)
                .FirstOrDefaultAsync(m => m.id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        public IActionResult SetStatus(int userId, int paymentId, int status)
        {
            ViewBag.layout = "_AdminLayout";
            var payment = _context.payment.Include(p => p.car).FirstOrDefault(p => p.id == paymentId);
            var userRole = _context.userRole.Include(u => u.role).Include(u => u.user).Where(u => u.userId == userId).FirstOrDefault();
            if (userRole == null)
            {
                // Xử lý khi không tìm thấy 
                return BadRequest("Access denied: You don't have permission to perform this action.");
            }
            var userRoleName = _context.roles.FirstOrDefault(i => i.id == userRole.roleId)?.role;

            if (payment == null)
            {
                // Xử lý khi không tìm thấy 
                return NotFound();
            }

            var booking = _context.bookings.Find(payment.booking_id);

            var car = _context.Car.Find(payment.carId);
            var user = _context.user.Find(userId);
            // check payment match with user id
            if (booking.userId == userId || userRoleName == "Admin" || userRoleName == "Owner" && booking.payments.FirstOrDefault().carId == car.id)
            {
                if (status >= 0 && status <= 4 && userRoleName == "Owner" && booking.payments.FirstOrDefault().carId == car.id)
                {
                    payment.status = status;
                    _context.Update(payment);
                    _context.SaveChanges();

                    user.coins = user.coins + car.Price;
                    _context.Update(user);
                    _context.SaveChanges();

                    return RedirectToAction("OrderList", "Payment", new { UserId = userId });
                }
                else if (status >= 0 && status <= 4)
                {
                    if (status == 4)
                    {
                        car.available = 1;
                        _context.Update(car);
                        _context.SaveChanges();
                    }
                    payment.status = status;
                    _context.Update(payment);
                    _context.SaveChanges();

                    user.coins = user.coins + car.Price;
                    _context.Update(user);
                    _context.SaveChanges();

                    return RedirectToAction("BookingHistory", "bookings", new { id = userId });
                }
                else
                {
                    return BadRequest("Invalid status value.");
                }
            }
            else
            {
                return BadRequest("Access denied: You don't have permission to perform this action.1");
            }

            return RedirectToAction("OrderList", new { userId = userId });
        }


        // GET: Payment/Create
        public IActionResult Create()
        {
            ViewBag.layout = "_AdminLayout";
            ViewData["booking_id"] = new SelectList(_context.bookings, "id", "id");
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
            ViewBag.layout = "_AdminLayout";
            if (ModelState.IsValid)
            {
                _context.Add(payment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["booking_id"] = new SelectList(_context.bookings, "id", "id", payment.booking_id);
            ViewData["carId"] = new SelectList(_context.Car, "id", "id", payment.carId);
            return View(payment);
        }

        // GET: Payment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.layout = "_AdminLayout";
            if (id == null || _context.payment == null)
            {
                return NotFound();
            }

            var payment = await _context.payment.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["booking_id"] = new SelectList(_context.bookings, "id", "id", payment.booking_id);
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
            ViewBag.layout = "_AdminLayout";
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
            ViewData["booking_id"] = new SelectList(_context.bookings, "id", "id", payment.booking_id);
            ViewData["carId"] = new SelectList(_context.Car, "id", "id", payment.carId);
            return View(payment);
        }

        // GET: Payment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.layout = "_AdminLayout";
            if (id == null || _context.payment == null)
            {
                return NotFound();
            }

            var payment = await _context.payment
                .Include(p => p.booking)
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
            ViewBag.layout = "_AdminLayout";
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
            ViewBag.layout = "_AdminLayout";
            return (_context.payment?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
