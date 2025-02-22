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

    // [HttpPost]
    // public IActionResult CreateRental(int carId, string pickupOffice, string returnOffice, DateTime rentalDate, DateTime returnDate, string rentalTime, string returnTime)
    // {
    //     // Kullanıcı ID'yi Session'dan al
    //     int? sessionUserId = HttpContext.Session.GetInt32("UserId");

    //     if (sessionUserId == null || sessionUserId == 0)
    //     {
    //         TempData["Error"] = "Araç kiralamak için giriş yapmalısınız.";
    //         return RedirectToAction("Login"); // Kullanıcı giriş yapmamışsa login sayfasına yönlendir
    //     }

    //     var rental = new Rental
    //     {
    //         CarID = carId,
    //         PickupOffice = pickupOffice,
    //         ReturnOffice = returnOffice,
    //         RentalDate = rentalDate.Date, // Sadece tarihi al
    //         ReturnDate = returnDate.Date, // Sadece tarihi al
    //                                       // Saat bilgisini DateTime olarak saklamak için MinValue ile birleştiriyoruz
    //         RentalTime = DateTime.MinValue.Add(TimeSpan.Parse(rentalTime)),
    //         ReturnTime = DateTime.MinValue.Add(TimeSpan.Parse(returnTime)),
    //         RentalStatus = "Aktif",
    //         UserID = sessionUserId.Value // Session'dan gelen UserID'yi ata
    //     };

    //     _rentalDbContext.Rentals.Add(rental);
    //     _rentalDbContext.SaveChanges();

    //     return RedirectToAction("Index");
    // }

    [HttpPost]
    public IActionResult CreateRental(int carId, string pickupOffice, string returnOffice, DateTime rentalDate, DateTime returnDate, string rentalTime, string returnTime)
    {
        int? sessionUserId = HttpContext.Session.GetInt32("UserId");

        if (sessionUserId == null || sessionUserId == 0)
        {
            TempData["Error"] = "Araç kiralamak için giriş yapmalısınız.";
            return RedirectToAction("Login");
        }

        DateTime rentalDateTime = rentalDate.Date.Add(TimeSpan.Parse(rentalTime));
        DateTime returnDateTime = returnDate.Date.Add(TimeSpan.Parse(returnTime));

        var existingRentals = _rentalDbContext.Rentals
            .Where(r => r.CarID == carId)
            .ToList();

        bool isCarAvailable = !existingRentals.Any(r =>
            !(r.ReturnDate.Add(r.ReturnTime) <= rentalDateTime || r.RentalDate.Add(r.RentalTime) >= returnDateTime)
        );

        if (!isCarAvailable)
        {
            TempData["ErrorMessage"] = "Seçtiğiniz tarihlerde araç doludur.";
            TempData["SelectedRentalDate"] = rentalDate.ToString("yyyy-MM-dd") + " " + rentalTime;
            TempData["SelectedReturnDate"] = returnDate.ToString("yyyy-MM-dd") + " " + returnTime;
            return RedirectToAction("Index");
        }

        var rental = new Rental
        {
            CarID = carId,
            PickupOffice = pickupOffice,
            ReturnOffice = returnOffice,
            RentalDate = rentalDate.Date,
            ReturnDate = returnDate.Date,
            RentalTime = TimeSpan.Parse(rentalTime),
            ReturnTime = TimeSpan.Parse(returnTime),
            RentalStatus = "Aktif",
            UserID = sessionUserId.Value
        };

        _rentalDbContext.Rentals.Add(rental);
        _rentalDbContext.SaveChanges();

        TempData["SuccessMessage"] = "Araç başarıyla kiralandı!";
        TempData["SelectedRentalDate"] = rentalDate.ToString("yyyy-MM-dd") + " " + rentalTime;
        TempData["SelectedReturnDate"] = returnDate.ToString("yyyy-MM-dd") + " " + returnTime;
        return RedirectToAction("Index");
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

    //     // **Veritabanından RAM'e çek**
    //     var existingRentals = _rentalDbContext.Rentals
    //         .Where(r => r.CarID == carId)
    //         .ToList(); // **Tüm verileri çekiyoruz**

    //     // **Çakışma kontrolü**
    //     bool isCarAvailable = !existingRentals.Any(r =>
    //         !(r.ReturnDate.Add(r.ReturnTime) <= rentalDateTime || r.RentalDate.Add(r.RentalTime) >= returnDateTime)
    //     );

    //     if (!isCarAvailable)
    //     {
    //         TempData["Error"] = "Seçtiğiniz tarih ve saatlerde bu araç zaten kiralanmış. Lütfen farklı bir zaman seçin.";
    //         return RedirectToAction("Index");
    //     }

    //     var rental = new Rental
    //     {
    //         CarID = carId,
    //         PickupOffice = pickupOffice,
    //         ReturnOffice = returnOffice,
    //         RentalDate = rentalDate.Date,
    //         ReturnDate = returnDate.Date,
    //         RentalTime = TimeSpan.Parse(rentalTime),  // **TimeSpan olarak dönüştürdük**
    //         ReturnTime = TimeSpan.Parse(returnTime),
    //         RentalStatus = "Aktif",
    //         UserID = sessionUserId.Value
    //     };

    //     _rentalDbContext.Rentals.Add(rental);
    //     _rentalDbContext.SaveChanges();

    //     TempData["Success"] = "Araç başarıyla kiralandı!";
    //     return RedirectToAction("Index");
    // }














    // [HttpGet]
    // public IActionResult GetAvailableCars(DateTime startDate, DateTime endDate)
    // {
    //     // 1. Belirtilen tarihlerde kiralanmış araçları bul
    //     var rentedCarIds = _rentalDbContext.Rentals
    //         .Where(r => r.RentalDate <= endDate && (r.ReturnDate == null || r.ReturnDate >= startDate))
    //         .Select(r => r.CarID)
    //         .Distinct()
    //         .ToList();

    //     // 2. Kiralanmamış araçları getir
    //     var availableCars = _appDbContext.Cars
    //         .Where(c => !rentedCarIds.Contains(c.CarId.Value) && c.IsAvailable == true)
    //         .ToList();

    //     return View(availableCars);
    // }

    // [HttpGet]
    // public JsonResult GetAvailableCars(DateTime startDate, DateTime endDate)
    // {
    //     var rentedCarIds = _rentalDbContext.Rentals
    //         .Where(r => r.RentalDate <= endDate && (r.ReturnDate == null || r.ReturnDate >= startDate))
    //         .Select(r => r.CarID)
    //         .Distinct()
    //         .ToList();

    //     var availableCars = _appDbContext.Cars
    //         .Where(c => !rentedCarIds.Contains(c.CarId.Value) && c.IsAvailable == true)
    //         .Select(c => new
    //         {
    //             CarId = c.CarId,
    //             Name = c.Make + " " + c.Model
    //         })
    //         .ToList();

    //     return Json(availableCars);
    // }



    // public List<Car> GetAvailableCars(DateTime rentalDate, DateTime returnDate)
    // {
    //     var rentedCars = _rentalDbContext.Rentals
    //         .Where(r => !(r.ReturnDate < rentalDate || r.RentalDate > returnDate))
    //         .Select(r => r.CarID)
    //         .ToList();

    //     var availableCars = _appDbContext.Cars
    //         .Where(c => !rentedCars.Contains(c.CarId))
    //         .ToList();

    //     return availableCars;
    // }


    [HttpGet]
    public IActionResult GetAvailableCars(DateTime rentalDate, DateTime returnDate)
    {
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
