using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Car_rental.Data;
using Car_rental.Models;
using Car_rental.ViewModels;
using Microsoft.AspNetCore.Mvc.Infrastructure;

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
        public async Task<IActionResult> Index(int pg = 1)
        {
            ViewBag.layout = "_AdminLayout";
            const int pageSize = 5;
            if (pg < 1)
                pg = 1;
            int recsCount = _context.Car.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var car_rentalContext = _context.Car.Include(c => c.category).Include(c => c.user).Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(car_rentalContext);
        }
        public async Task<IActionResult> Home()
        {
            ViewBag.Layout = "_Layout";
            ViewBag.carCount = 0;
            if (_context.Car.ToList().Count > 0)
            {
                ViewBag.carCount = _context.Car.ToList().Count;
            }
            if (_context.user.ToList().Count > 0)
            {
                ViewBag.userCount = _context.user.ToList().Count;
            }

            // List<string> distinctBrands = _context.Car.ToList().Select(car => car.brand).Distinct().ToList();
            // ViewBag.carBrandCount = distinctBrands.Count;
            try
            {
                List<string> distinctBrands = _context.Car.ToList().Select(car => car.brand).Distinct().ToList();
                ViewBag.carBrandCount = distinctBrands.Count;
            }
            catch (Exception ex)
            {
                ViewBag.carBrandCount = 0;
                // Xử lý lỗi ở đây nếu cần thiết
            }
            var car_rentalContext = _context.Car.Include(c => c.category).Include(c => c.user).Include(c => c.images).Where(c => c.available == 1).Select(c => new CarViewModel // Tạo một ViewModel mới để lưu trữ dữ liệu cần thiết
            {
                Id = c.id,
                Model = c.model,
                Brand = c.brand,
                Price = c.Price,
                address = c.address,
                ImageName = c.images.FirstOrDefault(i => i.carId == c.id) != null ? c.images.FirstOrDefault(i => i.carId == c.id).nameFile : null // Lấy nameFile đầu tiên từ images
            }); ;
            return View(await car_rentalContext.ToListAsync());
        }

        public async Task<IActionResult> About()
        {
            ViewBag.Layout = "_Layout";
            return View();
        }
        public async Task<IActionResult> Cars(int pg = 1)
        {
            ViewBag.Layout = "_Layout";
            const int pageSize = 12;
            if (pg < 1)
                pg = 1;
            int recsCount = _context.Car.Where(c => c.available == 1).Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var car_rentalContext = _context.Car.Include(c => c.category).Include(c => c.user).Include(c => c.images).Where(c => c.available == 1).Select(c => new CarViewModel // Tạo một ViewModel mới để lưu trữ dữ liệu cần thiết
            {
                Id = c.id,
                Model = c.model,
                Brand = c.brand,
                Price = c.Price,
                address = c.address,
                ImageName = c.images.FirstOrDefault(i => i.carId == c.id) != null ? c.images.FirstOrDefault(i => i.carId == c.id).nameFile : null
            }).Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(car_rentalContext);
        }
        public IActionResult CarListForManager(int UserId, int pg = 1)
        {
            ViewBag.layout = "_AdminLayout";
            var userRole = _context.userRole.Include(u => u.role).FirstOrDefault(u => u.userId == UserId);
            var userRoleName = _context.roles.FirstOrDefault(i => i.id == userRole.roleId).role;
            const int pageSize = 5;
            if (pg < 1)
                pg = 1;

            int recsCount = _context.Car.Where(u => u.user_id == UserId).Count();
            if (userRoleName == "Admin")
            {
                recsCount = _context.Car.Count();
            }
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var car_rentalContext = _context.Car.Include(c => c.category).Include(c => c.user).Where(u => u.user_id == UserId).Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            if (userRoleName == "Admin")
            {
                car_rentalContext = _context.Car.Include(c => c.category).Include(c => c.user).Skip(recSkip).Take(pager.PageSize).ToList();
            }
            return View(car_rentalContext);
        }
        public ActionResult SearchforUser(string query, string model, string location, double? minPrice, double? maxPrice, int? minSeat, int? maxSeat, DateTime? startDate1, DateTime? endDate1, int pg = 1)
        {
            ViewBag.Layout = "_Layout";
            const int pageSize = 12;
            if (pg < 1)
                pg = 1;

            var queryable = _context.Car
                .Include(c => c.category)
                .Include(c => c.user)
                .Include(c => c.images)
                .Where(d => d.available == 1);

            if (!string.IsNullOrEmpty(query))
            {
                queryable = queryable.Where(d => d.address.Contains(query));
            }

            if (!string.IsNullOrEmpty(model))
            {
                queryable = queryable.Where(d => d.model.Contains(model));
            }

            if (!string.IsNullOrEmpty(location))
            {
                queryable = queryable.Where(d => d.address.Contains(location));
            }

            if (minPrice.HasValue)
            {
                queryable = queryable.Where(d => d.Price >= minPrice);
            }

            if (maxPrice.HasValue)
            {
                queryable = queryable.Where(d => d.Price <= maxPrice);
            }

            if (minSeat.HasValue)
            {
                queryable = queryable.Where(d => d.seat >= minSeat);
            }

            if (maxSeat.HasValue)
            {
                queryable = queryable.Where(d => d.seat <= maxSeat);
            }

            if (startDate1.HasValue && endDate1.HasValue)
            {
                queryable = queryable.Where(c =>
                    !c.payments.Any(p =>
                        (p.booking.startDate >= startDate1 && p.booking.startDate <= endDate1 && (p.status == 0 || p.status == 1 || p.status == 2)) ||
                        (p.booking.endDate >= startDate1 && p.booking.endDate <= endDate1 && (p.status == 0 || p.status == 1 || p.status == 2)) ||
                        (p.booking.startDate <= startDate1 && p.booking.endDate >= endDate1 && (p.status == 0 || p.status == 1 || p.status == 2))
                    )
                );
            }

            int recsCount = queryable.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            ViewBag.Pager = pager;

            var car_rentalContext = queryable
                .Select(c => new CarViewModel
                {
                    Id = c.id,
                    Model = c.model,
                    Brand = c.brand,
                    Price = c.Price,
                    address = c.address,
                    ImageName = c.images.FirstOrDefault(i => i.carId == c.id) != null ? c.images.FirstOrDefault(i => i.carId == c.id).nameFile : null
                })
                .Skip(recSkip)
                .Take(pager.PageSize)
                .ToList();

            ViewBag.query = query;
            ViewBag.model = model;
            ViewBag.location = location;
            ViewBag.minPrice = minPrice;
            ViewBag.maxPrice = maxPrice;
            ViewBag.minSeat = minSeat;
            ViewBag.maxSeat = maxSeat;

            return View(car_rentalContext);
        }
        public IActionResult SetStatus(int userId, int carId)
        {
            ViewBag.Layout = "_Layout";
            var car = _context.Car.Find(carId);
            var userRole = _context.userRole.Include(u => u.role).FirstOrDefault(u => u.userId == userId);
            var userRoleName = _context.roles.FirstOrDefault(i => i.id == userRole.roleId).role;
            if (car == null)
            {
                // Xử lý khi không tìm thấy xe
                return NotFound();
            }
            // check car match with user id
            if (car.user_id == userId || userRoleName == "Admin")
            {
                if (car.available == 0)
                {
                    car.available = 1;
                    _context.Update(car);
                    _context.SaveChanges();
                }
                else
                {
                    car.available = 0;
                    _context.Update(car);
                    _context.SaveChanges();
                }
            }
            else
            {
                return BadRequest("Access denied: You don't have permission to perform this action.");
            }
            _context.Update(car);
            _context.SaveChanges();
            return RedirectToAction("CarListForManager", new { userId = userId });
        }
        public async Task<IActionResult> CreateCar()
        {
            ViewBag.Layout = "_AdminLayout";
            var cars = new List<car>();
            string[] modelNames = { "Innova", "Vios", "Camry", "Corolla Altis", "Fortuner", "Raize" };

            for (int i = 0; i < 5; i++)
            {
                var car = new car
                {
                    model = modelNames[i],
                    brand = "Toyota",
                    seat = 4,
                    Mileage = 1000 * i,
                    Transmission = "Auto",
                    color = "White",
                    address = "NinhKieu, Cantho",
                    available = 1,
                    ReleaseDate = DateTime.Now.AddDays(i),
                    Type = "SUV",
                    Price = 1000 * (i + 1),
                    Description = "Powerful yet smooth, classy yet comfortable, endlessly exciting yet exceptionally safe, the Honda CR-V powerfully breaks through with its owner on every journey.",
                    AirConditioning = i % 2 == 0,
                    ChildSeat = i % 2 == 0,
                    GPS = i % 2 == 0,
                    Luggage = i % 2 == 0,
                    Music = i % 2 == 0,
                    SeatBelt = i % 2 == 0,
                    SleepingBed = i % 2 == 0,
                    Water = i % 2 == 0,
                    Bluetooth = i % 2 == 0,
                    OnboardComputer = i % 2 == 0,
                    AudioInput = i % 2 == 0,
                    LongTermTrips = i % 2 == 0,
                    CarKit = i % 2 == 0,
                    RemoteCentralLocking = i % 2 == 0,
                    ClimateControl = i % 2 == 0,
                    user_id = 2, // Set the appropriate user ID
                    category_id = 1,
                };

                cars.Add(car);
            }
            string[] modelNames2 = { "BR-V", "HR-v", "City", "Civic", "Accord", "CT-V" };
            for (int i = 0; i < 5; i++)
            {
                var car = new car
                {
                    model = modelNames2[i],
                    brand = "Honda",
                    seat = 4,
                    Mileage = 1000 * i,
                    Transmission = "Auto",
                    color = "Black",
                    address = "NinhKieu, Cantho",
                    available = 1,
                    ReleaseDate = DateTime.Now.AddDays(i),
                    Type = "SUV",
                    Price = 1000 * (i + 1),
                    Description = "Powerful yet smooth, classy yet comfortable, endlessly exciting yet exceptionally safe, the Honda CR-V powerfully breaks through with its owner on every journey.",
                    AirConditioning = i % 2 == 0,
                    ChildSeat = i % 2 == 0,
                    GPS = i % 2 == 0,
                    Luggage = i % 2 == 0,
                    Music = i % 2 == 0,
                    SeatBelt = i % 2 == 0,
                    SleepingBed = i % 2 == 0,
                    Water = i % 2 == 0,
                    Bluetooth = i % 2 == 0,
                    OnboardComputer = i % 2 == 0,
                    AudioInput = i % 2 == 0,
                    LongTermTrips = i % 2 == 0,
                    CarKit = i % 2 == 0,
                    RemoteCentralLocking = i % 2 == 0,
                    ClimateControl = i % 2 == 0,
                    user_id = 2, // Set the appropriate user ID
                    category_id = 1,

                };

                cars.Add(car);
            }
            string[] modelNames3 = { "Explorer", "Everest Wildtrak", "Territory", "Ranger Raptor", "Ranger", "Ecosport" };
            for (int i = 0; i < 5; i++)
            {
                var car = new car
                {
                    model = modelNames3[i],
                    brand = "Ford",
                    seat = 7,
                    Mileage = 1000 * i,
                    Transmission = "Auto",
                    color = "Red",
                    address = "NinhKieu, CanTho",
                    available = 1,
                    ReleaseDate = DateTime.Now.AddDays(i),
                    Type = "SUV",
                    Price = 1000 * (i + 1),
                    Description = "Powerful yet smooth, classy yet comfortable, endlessly exciting yet exceptionally safe, the Honda CR-V powerfully breaks through with its owner on every journey.",
                    AirConditioning = i % 2 == 0,
                    ChildSeat = i % 2 == 0,
                    GPS = i % 2 == 0,
                    Luggage = i % 2 == 0,
                    Music = i % 2 == 0,
                    SeatBelt = i % 2 == 0,
                    SleepingBed = i % 2 == 0,
                    Water = i % 2 == 0,
                    Bluetooth = i % 2 == 0,
                    OnboardComputer = i % 2 == 0,
                    AudioInput = i % 2 == 0,
                    LongTermTrips = i % 2 == 0,
                    CarKit = i % 2 == 0,
                    RemoteCentralLocking = i % 2 == 0,
                    ClimateControl = i % 2 == 0,
                    user_id = 2, // Set the appropriate user ID
                    category_id = 1,

                };

                cars.Add(car);
            }


            _context.Car.AddRange(cars);
            _context.SaveChanges();
            return RedirectToAction("AddImg", "images");
        }

        // GET: Car/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            ViewBag.Layout = "_Layout";
            if (id == null || _context.Car == null)
            {
                return NotFound();
            }
            var listImages = _context.Images.Where(i => i.carId == id).ToList();
            ViewBag.listImages = listImages;


            var car = await _context.Car
                .Include(c => c.category)
                .Include(c => c.user)
                .FirstOrDefaultAsync(m => m.id == id);

            if (car == null)
            {
                return NotFound();
            }

            // Retrieve related cars efficiently
            var relatedCars = _context.Car.Include(c => c.category).Where(i => i.id != car.id);
            ViewBag.relatedCars = relatedCars.ToList();

            //showing rating
            var rating = _context.rating.Include(r => r.user).Include(r => r.car).Where(r => r.carId == id);
            ViewBag.rating = rating.ToList();
            ViewBag.rating5 = rating.Where(r => r.Star == 5).ToList();
            ViewBag.rating4 = rating.Where(r => r.Star == 4).ToList();
            ViewBag.rating3 = rating.Where(r => r.Star == 3).ToList();
            ViewBag.rating2 = rating.Where(r => r.Star == 2).ToList();
            ViewBag.rating1 = rating.Where(r => r.Star == 1).ToList();


            // // Retrieve related images for related cars
            // var relatedCarIds = relatedCars.Select(c => c.id).ToList();
            // var relatedImages = _context.Images.Where(i => relatedCarIds.Contains(i.carId)).ToList();

            // // Create a dictionary to map car IDs to their images
            // Dictionary<int, Images> carIdToImageMap = relatedImages.ToDictionary(i => i.carId);
            // List<Images> relatedCarImages = new List<Images>();
            // foreach (var relatedCar in relatedCars)
            // {
            //     if (carIdToImageMap.TryGetValue(relatedCar.id, out Images img))
            //     {
            //         relatedCarImages.Add(img);
            //     }
            //     else
            //     {
            //         // Handle the case where there's no image associated with the car
            //         // For example, you might add a default image or set img to null
            //         relatedCarImages.Add(null); // Placeholder for no image
            //     }
            // }
            // ViewBag.relatedCarImages = relatedCarImages;
            // Retrieve related images for related cars
            var relatedCarIds = relatedCars.Select(c => c.id).ToList();
            var relatedImages = _context.Images.Where(i => relatedCarIds.Contains(i.carId)).ToList();

            // Create a dictionary to map car IDs to their images
            Dictionary<int, List<Images>> carIdToImageMap = relatedImages
                .GroupBy(i => i.carId)
                .ToDictionary(
                    group => group.Key,
                    group => group.ToList()
                );

            List<List<Images>> relatedCarImages = new List<List<Images>>();
            foreach (var relatedCar in relatedCars)
            {
                if (carIdToImageMap.TryGetValue(relatedCar.id, out List<Images> carImages))
                {
                    relatedCarImages.Add(carImages);
                }
                else
                {
                    // Handle the case where there are no images associated with the car
                    relatedCarImages.Add(new List<Images>()); // Empty list for no images
                }
            }
            ViewBag.relatedCarImages = relatedCarImages;




            return View(car);
        }

        // GET: Car/Create
        public IActionResult Create()
        {
            ViewBag.layout = "_AdminLayout";
            ViewData["category_id"] = new SelectList(_context.category, "id", "type");
            ViewData["user_id"] = new SelectList(_context.user, "id", "email");
            return View();
        }

        // POST: Car/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int userId, IFormFile myfile1, IFormFile myfile2, IFormFile myfile3, IFormFile myfile4, IFormFile myfile5, [Bind("id,model,brand,seat,Mileage,Transmission,color,address,available,ReleaseDate,Type,Price,Description,AirConditioning,ChildSeat,GPS,Luggage,Music,SeatBelt,SleepingBed,Water,Bluetooth,OnboardComputer,AudioInput,LongTermTrips,CarKit,RemoteCentralLocking,ClimateControl,discount_id,user_id,category_id")] car car)
        {
            ViewBag.layout = "_AdminLayout";

            if (ModelState.IsValid)
            {
                if (car.Price < 100)
                {
                    ViewBag.errMsg = "Car Price should >= 100!";
                    return View(car);
                }
                if (car.ReleaseDate >= DateTime.Now.AddMonths(-2))
                {
                    ViewBag.errMsg = "Vehicle release date must be 2 months before current date!";
                    return View(car);
                }
                car.user_id = userId;
                _context.Add(car);
                await _context.SaveChangesAsync();
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
                return RedirectToAction("CarListForManager", new { UserId = userId });
            }

            ViewData["category_id"] = new SelectList(_context.category, "id", "type", car.category_id);
            ViewData["user_id"] = new SelectList(_context.user, "id", "id", car.user_id);

            return View(car);
        }

        // GET: Car/Edit/5
        public async Task<IActionResult> Edit(int? id, int userId)
        {
            ViewBag.layout = "_AdminLayout";
            if (id == null || _context.Car == null)
            {
                return NotFound();
            }

            var car = await _context.Car.FindAsync(id);
            var userRole = _context.userRole.Include(u => u.role).FirstOrDefault(u => u.userId == userId);
            var userRoleName = _context.roles.FirstOrDefault(i => i.id == userRole.roleId);
            // Check if the userId matches the user associated with the car
            if (car.user_id != userId && userRoleName.role != "Admin")
            {
                return BadRequest("You are not authorized to Edit this car.");
            }
            if (car == null)
            {
                return NotFound();
            }
            var imgList = _context.Images.Where(i => i.carId == id);
            ViewBag.imgList = imgList;

            ViewData["category_id"] = new SelectList(_context.category, "id", "type", car.category_id);
            return View(car);
        }

        // POST: Car/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int userId, IFormFile myfile1, IFormFile myfile2, IFormFile myfile3, IFormFile myfile4, IFormFile myfile5, [Bind("id,model,brand,seat,Mileage,Transmission,color,address,available,ReleaseDate,Type,Price,Description,AirConditioning,ChildSeat,GPS,Luggage,Music,SeatBelt,SleepingBed,Water,Bluetooth,OnboardComputer,AudioInput,LongTermTrips,CarKit,RemoteCentralLocking,ClimateControl,discount_id,user_id,category_id")] car car)
        {
            ViewBag.layout = "_AdminLayout";
            if (id != car.id)
            {
                return NotFound();
            }
            var carTemp = await _context.Car.FindAsync(id);
            _context.Entry(carTemp).State = EntityState.Detached;

            if (ModelState.IsValid && userId == carTemp.user_id)
            {
                try
                {
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
                        _context.Images.Add(new Images { nameFile = replacedString, carId = id });
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
                        _context.Images.Add(new Images { nameFile = replacedString, carId = id });
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
                        _context.Images.Add(new Images { nameFile = replacedString, carId = id });
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
                        _context.Images.Add(new Images { nameFile = replacedString, carId = id });
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
                        _context.Images.Add(new Images { nameFile = replacedString, carId = id });
                    }
                    car.user_id = carTemp.user_id;
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
                return RedirectToAction("CarListForManager", new { UserId = car.user_id });
            }
            else
            {
                return BadRequest("You are not authorized to edit this car.");
            }


            ViewData["category_id"] = new SelectList(_context.category, "id", "type", car.category_id);
            ViewData["user_id"] = new SelectList(_context.user, "id", "email", car.user_id);
            return View(car);
        }

        // GET: Car/Delete/5
        public async Task<IActionResult> Delete(int? id, int userId)
        {
            ViewBag.layout = "_AdminLayout";
            if (id == null || _context.Car == null)
            {
                return NotFound();
            }
            var car = await _context.Car
                .Include(c => c.category)
                .Include(c => c.user)
                .FirstOrDefaultAsync(m => m.id == id);
            // Check if the userId matches the user associated with the car
            var userRole = _context.userRole.Include(u => u.role).FirstOrDefault(u => u.userId == userId);
            var userRoleName = _context.roles.FirstOrDefault(i => i.id == userRole.roleId);
            if (car.user_id != userId && userRoleName.role != "Admin")
            {
                return BadRequest("You are not authorized to delete this car.");
            }
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Car/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int userId)
        {
            ViewBag.layout = "_AdminLayout";
            if (_context.Car == null)
            {
                return Problem("Entity set 'Car_rentalContext.Car'  is null.");
            }
            var car = await _context.Car.FindAsync(id);
            if (car != null)
            {
                // Check if the userId matches the user associated with the car
                if (car.user_id != userId)
                {
                    return BadRequest("You are not authorized to delete this car.");
                }
                else
                {
                    _context.Car.Remove(car);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Chart(int userId)
        {
            ViewBag.Layout = "_AdminLayout";
            // var getUserRole = _context.userRole.Include(u => u.role).FirstOrDefault(u => u.userId == userId);
            // var userRoleName = _context.roles.FirstOrDefault(i => i.id == getUserRole.roleId).role;

            var user = _context.user.Find(userId);
            var roleList = _context.roles.GroupBy(r => r.role)
            .Select(g => new
            {
                RoleName = g.Key,
                Percentage = (double)0
            }).ToList();
            var userRole = _context.userRole.Count();

            var data = _context.userRole.Include(r => r.role)
                .GroupBy(ur => ur.role.role)
                .Select(group => new
                {
                    RoleName = group.Key,
                    Percentage = (double)group.Count() / userRole * 100
                })
                .ToList();
            foreach (var item in roleList)
            {
                //Console.WriteLine(item.FirstOrDefault().role);
                var existingData = data.FirstOrDefault(r => r.RoleName.Contains(item.RoleName));
                if (existingData == null)
                {
                    data.Add(item);
                }
            }

            string[] labels = new string[data.Count()];
            double[] count = new double[data.Count()]; // Changed to double[]

            for (int i = 0; i < data.Count(); i++)
            {
                labels[i] = data[i].RoleName;
                count[i] = data[i].Percentage; // Use roleStatistics instead of listUserRole
            }


            ///////brand count
            var brandCounts = await _context.Car
                                    .GroupBy(c => c.brand)
                                    .Select(g => new { Brand = g.Key, Count = g.Count() })
                                    .ToListAsync();

            var labelsCarBrand = brandCounts.Select(bc => bc.Brand).ToList();
            var dataCarBrand = brandCounts.Select(bc => bc.Count).ToList();

            ViewBag.BrandLabels = labelsCarBrand;
            ViewBag.BrandData = dataCarBrand;
            /////brand count manager
            var brandCountsManager = await _context.Car.Where(c => c.user_id == user.id)
                                    .GroupBy(c => c.brand)
                                    .Select(g => new { Brand = g.Key, Count = g.Count() })
                                    .ToListAsync();

            var labelsCarBrandManager = brandCountsManager.Select(bc => bc.Brand).ToList();
            var dataCarBrandManager = brandCountsManager.Select(bc => bc.Count).ToList();

            ViewBag.BrandLabelsManager = labelsCarBrandManager;
            ViewBag.BrandDataManager = dataCarBrandManager;

            ///// car order
            // Truy vấn cơ sở dữ liệu để lấy số lượng đặt chỗ thành công của mỗi xe
            var query = from car in _context.Car.Where(c => c.user_id == user.id)
                        join payment in _context.payment on car.id equals payment.carId
                        where payment.status == 3 // Điều kiện để xác định đặt chỗ thành công
                        group car by car.model into modelGroup
                        select new { Model = modelGroup.Key, Count = modelGroup.Count() };

            var models = query.Select(item => item.Model).ToList();
            var counts = query.Select(item => item.Count).ToList();

            //ViewData["models"] = String.Format("'{0}'", String.Join("','", models));
            ViewBag.models = models;
            ViewBag.counts = counts;

            ViewData["labels"] = String.Format("'{0}'", String.Join("','", labels));
            ViewData["Percent"] = String.Join(",", count);
            return View();
        }

        private bool carExists(int id)
        {
            ViewBag.layout = "_AdminLayout";
            return _context.Car.Any(e => e.id == id);
        }
    }
}
