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
    public class CarController : Controller
    {
        private IWebHostEnvironment _hostEnvironment;
        private readonly Car_rentalContext _context;

        public CarController(IWebHostEnvironment hostEnvironment, Car_rentalContext context)
        {
            _hostEnvironment = hostEnvironment;
            _context = context;
        }

        // GET: Car
        public async Task<IActionResult> Index()
        {
            var car_rentalContext = _context.Car.Include(c => c.Discount).Include(c => c.category).Include(c => c.user);
            return View(await car_rentalContext.ToListAsync());
        }


        // GET: Car/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            ViewBag.Layout = "_Layout";
            var listImages = _context.Images.Where(i => i.carId == id).ToList();
            ViewBag.listImages = listImages;
            var Video = _context.VideoCar.FirstOrDefault(i => i.carId == id).nameFile;
            ViewBag.Video = Video;

            //random related cars base on category 
            // Random random = new Random();
            var carid = _context.Car.Find(id);
            var relatedCars = _context.Car.Include(c => c.category).Where(i => i.category_id == carid.category_id && i.id != carid.id);
            ViewBag.relatedCars = relatedCars.ToList();

            foreach(var carList in relatedCars){
                var carListId = _context.Car.Find(carList.id);
                var img = _context.Images.FirstOrDefault(i => i.carId == carListId.id);
            }

            if (id == null || _context.Car == null)
            {
                return NotFound();
            }

            var car = await _context.Car
                .Include(c => c.Discount)
                .Include(c => c.category)
                .Include(c => c.user)
                .FirstOrDefaultAsync(m => m.id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Car/Create
        public IActionResult Create()
        {
            ViewData["discount_id"] = new SelectList(_context.discount, "id", "code");
            ViewData["category_id"] = new SelectList(_context.category, "id", "type");
            ViewData["user_id"] = new SelectList(_context.user, "id", "email");
            return View();
        }

        // POST: Car/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile myVideoFile, IFormFile myfile1, IFormFile myfile2, IFormFile myfile3, IFormFile myfile4, IFormFile myfile5, [Bind("id,model,brand,seat,Mileage,Transmission,color,address,available,ReleaseDate,Type,Price,AirConditioning,ChildSeat,GPS,Luggage,Music,SeatBelt,SleepingBed,Water,Bluetooth,OnboardComputer,AudioInput,LongTermTrips,CarKit,RemoteCentralLocking,ClimateControl,discount_id,user_id,category_id")] car car)
        {
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                if (myVideoFile != null)
                {

                    string filename = Path.GetFileName(myVideoFile.FileName);
                    string replacedString = filename.Replace(" ", "");
                    var filePath = Path.Combine(_hostEnvironment.WebRootPath, "Video");
                    string fullPath = filePath + "\\" + replacedString;
                    // Copy files to FileSystem using Streams
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await myVideoFile.CopyToAsync(stream);
                    }
                    _context.VideoCar.Add(new VideoCar { nameFile = replacedString, carId = car.id });
                }
                if (myfile1 != null)
                {

                    string filename = Path.GetFileName(myfile1.FileName);
                    string replacedString = filename.Replace(" ", "");
                    var filePath = Path.Combine(_hostEnvironment.WebRootPath, "images");
                    string fullPath = filePath + "\\" + replacedString;
                    // Copy files to FileSystem using Streams
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await myfile1.CopyToAsync(stream);
                    }
                    _context.Images.Add(new Images { nameFile = replacedString, carId = car.id });
                }
                if (myfile2 != null)
                {
                    string filename = Path.GetFileName(myfile2.FileName);
                    string replacedString = filename.Replace(" ", "");
                    var filePath = Path.Combine(_hostEnvironment.WebRootPath, "images");
                    string fullPath = filePath + "\\" + replacedString;
                    // Copy files to FileSystem using Streams
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await myfile2.CopyToAsync(stream);
                    }
                    _context.Images.Add(new Images { nameFile = replacedString, carId = car.id });
                }
                if (myfile3 != null)
                {
                    string filename = Path.GetFileName(myfile3.FileName);
                    string replacedString = filename.Replace(" ", "");
                    var filePath = Path.Combine(_hostEnvironment.WebRootPath, "images");
                    string fullPath = filePath + "\\" + replacedString;
                    // Copy files to FileSystem using Streams
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await myfile3.CopyToAsync(stream);
                    }
                    _context.Images.Add(new Images { nameFile = replacedString, carId = car.id });
                }
                if (myfile4 != null)
                {
                    string filename = Path.GetFileName(myfile4.FileName);
                    string replacedString = filename.Replace(" ", "");
                    var filePath = Path.Combine(_hostEnvironment.WebRootPath, "images");
                    string fullPath = filePath + "\\" + replacedString;
                    // Copy files to FileSystem using Streams
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await myfile4.CopyToAsync(stream);
                    }
                    _context.Images.Add(new Images { nameFile = replacedString, carId = car.id });
                }
                if (myfile5 != null)
                {
                    string filename = Path.GetFileName(myfile5.FileName);
                    string replacedString = filename.Replace(" ", "");
                    var filePath = Path.Combine(_hostEnvironment.WebRootPath, "images");
                    string fullPath = filePath + "\\" + replacedString;
                    // Copy files to FileSystem using Streams
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await myfile5.CopyToAsync(stream);
                    }
                    _context.Images.Add(new Images { nameFile = replacedString, carId = car.id });
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["discount_id"] = new SelectList(_context.discount, "id", "code", car.discount_id);
            ViewData["category_id"] = new SelectList(_context.category, "id", "type", car.category_id);
            ViewData["user_id"] = new SelectList(_context.user, "id", "email", car.user_id);
            return View(car);
        }

        // GET: Car/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Car == null)
            {
                return NotFound();
            }

            var car = await _context.Car.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            ViewData["discount_id"] = new SelectList(_context.discount, "id", "code", car.discount_id);
            ViewData["category_id"] = new SelectList(_context.category, "id", "type", car.category_id);
            ViewData["user_id"] = new SelectList(_context.user, "id", "email", car.user_id);
            return View(car);
        }

        // POST: Car/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,model,brand,seat,Mileage,Transmission,color,address,available,ReleaseDate,Type,Price,AirConditioning,ChildSeat,GPS,Luggage,Music,SeatBelt,SleepingBed,Water,Bluetooth,OnboardComputer,AudioInput,LongTermTrips,CarKit,RemoteCentralLocking,ClimateControl,discount_id,user_id,category_id")] car car)
        {
            if (id != car.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!carExists(car.id))
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
            ViewData["discount_id"] = new SelectList(_context.discount, "id", "code", car.discount_id);
            ViewData["category_id"] = new SelectList(_context.category, "id", "type", car.category_id);
            ViewData["user_id"] = new SelectList(_context.user, "id", "email", car.user_id);
            return View(car);
        }

        // GET: Car/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Car == null)
            {
                return NotFound();
            }

            var car = await _context.Car
                .Include(c => c.Discount)
                .Include(c => c.category)
                .Include(c => c.user)
                .FirstOrDefaultAsync(m => m.id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Car/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Car == null)
            {
                return Problem("Entity set 'Car_rentalContext.Car'  is null.");
            }
            var car = await _context.Car.FindAsync(id);
            if (car != null)
            {
                _context.Car.Remove(car);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool carExists(int id)
        {
            return _context.Car.Any(e => e.id == id);
        }
    }
}
