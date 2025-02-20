using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using carrentalcde.Models;

namespace carrentalcde.Controllers;

public class HomeController : Controller
{
    private readonly ICarService _carService;
    private readonly RentalDbContext _rentalDbContext;
    private readonly ApplicationDbContext _appDbContext;

    // Constructor: CarService bağımlılığı enjekte ediliyor
    public HomeController(ICarService carService, RentalDbContext rentalDbContext, ApplicationDbContext appDbContext)
    {
        _carService = carService;
        _rentalDbContext = rentalDbContext;
        _appDbContext = appDbContext;
    }

    // Index methodu: Araçları alır ve View'e gönderir
    public IActionResult Index()
    {
        var cars = _carService.GetAllCars(); // CarService ile araçları alıyoruz
        return View(cars); // Ve bunları view'e gönderiyoruz
    }

    [HttpPost]
    public IActionResult CreateRental(int carId, string pickupOffice, string returnOffice, DateTime rentalDate, DateTime returnDate, string rentalTime, string returnTime)
    {
        // Seçilen aracı veritabanına kirala
        var rental = new Rental
        {
            CarID = carId,
            PickupOffice = pickupOffice,
            ReturnOffice = returnOffice,
            RentalDate = rentalDate,
            ReturnDate = returnDate,
            RentalTime = DateTime.Parse(rentalDate.ToShortDateString() + " " + rentalTime),
            ReturnTime = DateTime.Parse(returnDate.ToShortDateString() + " " + returnTime),
            RentalStatus = "Aktif" // Örnek bir durum, sabitler veya enum kullanılabilir
        };

        _rentalDbContext.Rentals.Add(rental);
        _rentalDbContext.SaveChanges();

        return RedirectToAction("Index");  // Kiralama işlemi tamamlandığında anasayfaya yönlendir
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