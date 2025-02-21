using Microsoft.AspNetCore.Mvc;

public class RentalController : Controller
{
    private readonly ApplicationDbContext _appContext;
    private readonly RentalDbContext _rentalContext;

    public RentalController(ApplicationDbContext appContext, RentalDbContext rentalContext)
    {
        _appContext = appContext;
        _rentalContext = rentalContext;
    }

    // GET: Rental/Create
    public IActionResult Create()
    {
        var model = new RentalDTO();

        // Veritabanından araçları alıp modele gönderiyoruz
        var cars = _appContext.Cars.Select(car => new CarDTO
        {
            CarId = car.CarId,
            Make = car.Make,
            Model = car.Model
        }).ToList();

        model.Cars = cars;  // Araba listesini view'a gönderiyoruz

        return View(model);
    }

    // POST: Rental/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateRental(RentalDTO rentalDTO)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");

        // Kullanıcı giriş yapmamışsa işlem yapma
        if (!userId.HasValue)
        {
            TempData["ErrorMessage"] = "Kiralama yapabilmek için giriş yapmalısınız.";
            return RedirectToAction("Login"); // Kullanıcı giriş sayfasına yönlendirilsin
        }

        if (ModelState.IsValid)
        {
            var rental = new Rental
            {
                UserID = userId.Value, // Kullanıcı ID burada eklendi
                                       //CarID = rentalDTO.CarID, // Eksik olan atama
                RentalDate = rentalDTO.RentalDate,
                ReturnDate = rentalDTO.ReturnDate,
                RentalStatus = "Aktif",
                PickupOffice = rentalDTO.PickupOffice,
                ReturnOffice = rentalDTO.ReturnOffice,
                RentalTime = rentalDTO.RentalTime,
                ReturnTime = rentalDTO.ReturnTime
            };

            _rentalContext.Rentals.Add(rental);
            _rentalContext.SaveChanges();

            return RedirectToAction("Index");
        }

        return View(rentalDTO);
    }

}