using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Car_rental.Data;
using Car_rental.Models;
using JetBrains.Annotations;
using Car_rental.Untils;

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
        public async Task<IActionResult> BookingHistory(int? id, int pg = 1)
        {
            ViewBag.Layout = "_Layout";
            const int pageSize = 5;
            if (pg < 1)
                pg = 1;
            int recsCount = _context.bookings.Where(d => d.userId == id).Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            this.ViewBag.Pager = pager;
            var car_rentalContext = _context.bookings.Include(b => b.user).Include(b => b.payments).ThenInclude(p => p.car).Where(d => d.userId == id).Skip(recSkip).Take(pager.PageSize);
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


        public IActionResult Book(int? cardId, double? totalAmount, int? userId)
        {
            ViewBag.cardId = cardId;
            ViewBag.totalAmount = totalAmount;
            ViewBag.userId = userId;

            return View();
        }

        // POST: bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Booking(int userId, int cardId, DateTime? startDate, DateTime? endDate, double? totalAmount, string TakeCar, string CarBack)
        {
            // Set ViewBag.Layout to null (not sure why this is done, consider removing)
            ViewBag.Layout = null;

            if (ModelState.IsValid)
            {
                var paymentsWithCarId = _context.payment
                                        .Where(p => p.carId == cardId) // Filter payments by carId
                                        .Where(p =>
                                                    (p.booking.startDate <= endDate && p.booking.endDate >= startDate && (p.status == 0 || p.status == 1 || p.status == 2)) ||
                                                    (p.booking.startDate >= startDate && p.booking.startDate <= endDate && (p.status == 0 || p.status == 1 || p.status == 2)) ||
                                                    (p.booking.endDate >= startDate && p.booking.endDate <= endDate && (p.status == 0 || p.status == 1 || p.status == 2))
                                                )
                                        .ToList();
                var car = _context.Car.Find(cardId);
                var user = _context.user.Find(userId);
                var owner = _context.user.Find(car.user_id);
                if (paymentsWithCarId.Count() == 0)
                {
                    if (car.user_id != userId && user.coins >= car.Price)
                    {
                        user.coins = user.coins - car.Price;
                        _context.Update(user);
                        await _context.SaveChangesAsync();

                        // If all input is valid, proceed with booking
                        var bookings = new bookings();
                        bookings.userId = userId;
                        bookings.startDate = startDate;
                        bookings.endDate = endDate;
                        bookings.totalAmount = totalAmount;
                        bookings.TakeCar = TakeCar;
                        bookings.CarBack = CarBack;

                        // Add the booking to the context and save changes
                        _context.Add(bookings);
                        await _context.SaveChangesAsync();

                        // Create a new payment record
                        var payment = new payment();
                        payment.amount = (double)bookings.totalAmount;
                        payment.status = 0;
                        payment.booking_id = bookings.id;
                        payment.carId = cardId;
                        payment.paymentDate = DateTime.Now;
                        payment.paymentMethod = "Coin";



                        // Add the payment record to the context and save changes
                        _context.payment.Add(payment);
                        await _context.SaveChangesAsync();

                        //send mail
                        Send send = new Send();
                        var email = user.email.ToString();
                        var subject = "You have successfully booked your car rental!";
                        string body = "You have successfully booked your car rental, please click the link below to see the details: " + "\n\n" + "https://localhost:7160/bookings/BookingHistory/" + user.id; ;

                        var emailOwner = owner.email.ToString();
                        var subjectOwner = "Your car has been booked!";
                        string bodyOwner = "Your car has been booked, please click the link below to see the details: " + "\n\n" + "https://localhost:7160/Payment/OrderList?userId=" + car.user_id; ;

                        send.SendEmail(email, subject, body);
                        send.SendEmail(emailOwner, subjectOwner, bodyOwner);

                        // Redirect to the booking history page
                        return RedirectToAction("BookingHistory", "bookings", new { id = userId });
                    }
                    else if (user.coins < car.Price)
                    {

                        // Generate JavaScript code to show an alert on the client-side
                        string script = $"<script>alert('Your amount is not enough!');window.location.href = '/car/details/{car.id}';</script>";
                        return Content(script, "text/html");
                    }
                    else
                    {
                        // Generate JavaScript code to show an alert on the client-side
                        string script = $"<script>alert('You are own this car!');window.location.href = '/car/details/{car.id}';</script>";
                        return Content(script, "text/html");
                    }
                }
                else
                {
                    // Generate JavaScript code to show an alert on the client-side
                    string script = $"<script>alert('Car is busy that day!');window.location.href = '/car/details/{car.id}';</script>";
                    return Content(script, "text/html");
                }

            }

            // If there are validation errors or ModelState is not valid, return to the view
            return RedirectToAction("BookingHistory", "bookings", new { id = userId });
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
                .Include(b => b.payments)
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
            var payments = _context.payment.Where(p => p.booking_id == id);

            if (payments != null)
            {
                foreach (var item in payments)
                {
                    _context.payment.Remove(item);
                }
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
