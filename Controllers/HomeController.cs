using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Car_rental.Models;
using Car_rental.Data;
using Microsoft.EntityFrameworkCore;

namespace Car_rental.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Car_rentalContext _context;

    public HomeController(ILogger<HomeController> logger,  Car_rentalContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        ViewBag.Layout = "_Layout";
        var data = _context.Car.Include(c => c.Discount).Include(c => c.category).Include(c => c.user);
        ViewBag.Cars = data.ToList();
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
