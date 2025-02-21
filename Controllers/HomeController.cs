using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using carrentalcde.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;



namespace carrentalcde.Controllers;

public class HomeController : Controller
{
    private readonly ICarService _carService;
    private readonly RentalDbContext _rentalDbContext;
    private readonly ApplicationDbContext _appDbContext;
    private readonly UserDbContext _userDbContext;

    public HomeController(ICarService carService, RentalDbContext rentalDbContext, ApplicationDbContext appDbContext)
    {
        _carService = carService;
        _rentalDbContext = rentalDbContext;
        _appDbContext = appDbContext;
    }

    public IActionResult Index()
    {
        var cars = _carService.GetAllCars();
        return View(cars);
    }

    [HttpPost]
    public IActionResult CreateRental(int carId, string pickupOffice, string returnOffice, DateTime rentalDate, DateTime returnDate, string rentalTime, string returnTime)
    {
        // Kullanıcı ID'yi Session'dan al
        int? sessionUserId = HttpContext.Session.GetInt32("UserId");

        if (sessionUserId == null || sessionUserId == 0)
        {
            TempData["Error"] = "Araç kiralamak için giriş yapmalısınız.";
            return RedirectToAction("Login"); // Kullanıcı giriş yapmamışsa login sayfasına yönlendir
        }

        var rental = new Rental
        {
            CarID = carId,
            PickupOffice = pickupOffice,
            ReturnOffice = returnOffice,
            RentalDate = rentalDate.Date, // Sadece tarihi al
            ReturnDate = returnDate.Date, // Sadece tarihi al
                                          // Saat bilgisini DateTime olarak saklamak için MinValue ile birleştiriyoruz
            RentalTime = DateTime.MinValue.Add(TimeSpan.Parse(rentalTime)),
            ReturnTime = DateTime.MinValue.Add(TimeSpan.Parse(returnTime)),
            RentalStatus = "Aktif",
            UserID = sessionUserId.Value // Session'dan gelen UserID'yi ata
        };

        _rentalDbContext.Rentals.Add(rental);
        _rentalDbContext.SaveChanges();

        return RedirectToAction("Index");
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
