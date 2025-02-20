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
            CarId = car.CarId ?? 0,
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
        if (ModelState.IsValid)
        {
            // Rental verilerini alıp, veritabanına kaydediyoruz
            var rental = new Rental
            {
                UserID = rentalDTO.UserID, // Kullanıcı bilgisi, örneğin oturumdan alabilirsiniz.
                //CarID = rentalDTO.CarID,
                RentalDate = rentalDTO.RentalDate,
                ReturnDate = rentalDTO.ReturnDate,
                RentalStatus = "Aktif", // Örnek bir durum, sabitler veya enum kullanılabilir
                PickupOffice = rentalDTO.PickupOffice,
                ReturnOffice = rentalDTO.ReturnOffice,
                RentalTime = rentalDTO.RentalTime,
                ReturnTime = rentalDTO.ReturnTime
            };

            // Rental'ı veritabanına ekliyoruz
            _rentalContext.Rentals.Add(rental);
            _rentalContext.SaveChanges();

            return RedirectToAction("Index"); // Kiralama tamamlandıktan sonra yönlendirme
        }

        return View(rentalDTO); // Eğer model geçerli değilse aynı formu geri döneriz
    }
}