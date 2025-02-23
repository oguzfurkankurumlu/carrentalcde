using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using carrentalcde.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Linq;



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

    [HttpGet]
    public IActionResult SearchCars()
    {
        return View();
    }

    [HttpPost]
    public IActionResult SearchCars(SearchCarsViewModel model)
    {
        int? sessionUserId = HttpContext.Session.GetInt32("UserId");

        if (sessionUserId == null || sessionUserId == 0)
        {
            TempData["Error"] = "Araç kiralamak için giriş yapmalısınız.";
            return RedirectToAction("Login", "Account");
        }

        var rentedCarIds = _rentalDbContext.Rentals
            .Where(r => (model.RentalDate >= r.RentalDate && model.RentalDate <= r.ReturnDate) ||
                        (model.ReturnDate >= r.RentalDate && model.ReturnDate <= r.ReturnDate))
            .Select(r => r.CarID)
            .ToList();

        var availableCars = _appDbContext.Cars
            .Where(c => !rentedCarIds.Contains(c.CarId))
            .ToList();

        ViewBag.RentalDate = model.RentalDate;
        ViewBag.ReturnDate = model.ReturnDate;
        ViewBag.PickupOffice = model.PickupOffice;
        ViewBag.ReturnOffice = model.ReturnOffice;
        ViewBag.RentalTime = model.RentalTime;
        ViewBag.ReturnTime = model.ReturnTime;

        ViewData["HideTopbar"] = false;
        ViewData["HideNavbar"] = false;
        ViewData["HideFooter"] = false;

        

        return View("AvailableCars", availableCars);
    }

    [HttpPost]
    public IActionResult CreateRental(int carId, string pickupOffice, string returnOffice, DateTime rentalDate, DateTime returnDate, string rentalTime, string returnTime)
    {
        int? sessionUserId = HttpContext.Session.GetInt32("UserId");

        if (sessionUserId == null || sessionUserId == 0)
        {
            TempData["Error"] = "Araç kiralamak için giriş yapmalısınız.";
            return RedirectToAction("Login", "Account");
        }

        DateTime rentalDateTime = rentalDate.Date.Add(TimeSpan.Parse(rentalTime));
        DateTime returnDateTime = returnDate.Date.Add(TimeSpan.Parse(returnTime));
        var car = _appDbContext.Cars.Find(carId);
        var totalDays = (returnDateTime - rentalDateTime).Days;
        if (totalDays == 0) totalDays = 1; // Aynı gün kiralamalar için en az 1 gün sayılır
        var totalAmount = totalDays * car.PricePerDay;

        var rental = new Rental
        {
            CarID = carId,
            UserID = sessionUserId.Value,
            PickupOffice = pickupOffice,
            ReturnOffice = returnOffice,
            RentalDate = rentalDateTime,
            ReturnDate = returnDateTime,
            RentalTime = TimeSpan.Parse(rentalTime),
            ReturnTime = TimeSpan.Parse(returnTime),
            RentalStatus = "Aktif",
            TotalAmount = totalAmount,
        };

        _rentalDbContext.Rentals.Add(rental);
        _rentalDbContext.SaveChanges();

        TempData["SuccessMessage"] = "Kiralama işleminiz başarılı.";
        return RedirectToAction("UserRentals", "Account");
    }




    // [HttpPost]
    // public IActionResult CreateRental(int carId, string pickupOffice, string returnOffice, DateTime rentalDate, DateTime returnDate, string rentalTime, string returnTime)
    // {

    //     int? sessionUserId = HttpContext.Session.GetInt32("UserId");

    //     if (sessionUserId == null || sessionUserId == 0)
    //     {
    //         TempData["Error"] = "Araç kiralamak için giriş yapmalısınız.";
    //         return RedirectToAction("Login");
    //     }

    //     DateTime rentalDateTime = rentalDate.Date.Add(TimeSpan.Parse(rentalTime));
    //     DateTime returnDateTime = returnDate.Date.Add(TimeSpan.Parse(returnTime));

    //     var existingRentals = _rentalDbContext.Rentals
    //         .Where(r => r.CarID == carId)
    //         .ToList();

    //     bool isCarAvailable = !existingRentals.Any(r =>
    //         !(r.ReturnDate.Add(r.ReturnTime) <= rentalDateTime || r.RentalDate.Add(r.RentalTime) >= returnDateTime)
    //     );

    //     if (!isCarAvailable)
    //     {
    //         TempData["ErrorMessage"] = "Seçtiğiniz tarihlerde araç doludur.";
    //         TempData["SelectedRentalDate"] = rentalDate.ToString("yyyy-MM-dd") + " " + rentalTime;
    //         TempData["SelectedReturnDate"] = returnDate.ToString("yyyy-MM-dd") + " " + returnTime;
    //         return RedirectToAction("Index");
    //     }

    //     var rental = new Rental
    //     {
    //         CarID = carId,
    //         PickupOffice = pickupOffice,
    //         ReturnOffice = returnOffice,
    //         RentalDate = rentalDate.Date,
    //         ReturnDate = returnDate.Date,
    //         RentalTime = TimeSpan.Parse(rentalTime),
    //         ReturnTime = TimeSpan.Parse(returnTime),
    //         RentalStatus = "Aktif",
    //         UserID = sessionUserId.Value
    //     };

    //     _rentalDbContext.Rentals.Add(rental);
    //     _rentalDbContext.SaveChanges();

    //     TempData["SuccessMessage"] = "Araç başarıyla kiralandı!";
    //     TempData["SelectedRentalDate"] = rentalDate.ToString("yyyy-MM-dd") + " " + rentalTime;
    //     TempData["SelectedReturnDate"] = returnDate.ToString("yyyy-MM-dd") + " " + returnTime;
    //     return RedirectToAction("Index");
    // }




    [HttpGet]
    public IActionResult GetAvailableCars(DateTime rentalDate, DateTime returnDate)
    {
        ViewData["HideTopbar"] = false;
        ViewData["HideNavbar"] = false;
        ViewData["HideFooter"] = false;
        var rentedCars = _rentalDbContext.Rentals
            .Where(r => !(r.ReturnDate < rentalDate || r.RentalDate > returnDate))
            .Select(r => (int?)r.CarID) // int? olarak aldık
            .ToList();

        var availableCars = _appDbContext.Cars
            .Where(c => !rentedCars.Contains(c.CarId))
            .ToList();

        return Json(availableCars); // JSON formatında müsait araçları döndür
    }


    public IActionResult Cars()
    {
        ViewData["HideTopbar"] = false;
        ViewData["HideNavbar"] = false;
        ViewData["HideFooter"] = false;
        var cars = _appDbContext.Cars.ToList(); // Veritabanındaki araçları çekiyoruz.
        return View(cars); // Cars.cshtml sayfasına listeyi gönderiyoruz.
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
