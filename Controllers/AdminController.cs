using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{
    private readonly IUserService _userService;
    private readonly RentalDbContext _rentalDbContext;


    public AdminController(IUserService userService, RentalDbContext rentalDbContext)
    {
        _userService = userService;
        _rentalDbContext = rentalDbContext;
    }

    // Admin Paneli - Kullanıcı Listesi
    public IActionResult Index()
    {
        ViewData["HideTopbar"] = false;
        ViewData["HideNavbar"] = false;
        ViewData["HideFooter"] = false;
        return View();
    }

    public IActionResult UserList()
    {
        ViewData["HideTopbar"] = false;
        ViewData["HideNavbar"] = false;
        ViewData["HideFooter"] = false;
        var users = _userService.GetAllUsers();
        return View(users);  // Kullanıcıları view'a gönderiyoruz
    }

    // Kullanıcı Ekleme
    public IActionResult AddUser()
    {

        ViewData["HideTopbar"] = false;
        ViewData["HideNavbar"] = false;
        ViewData["HideFooter"] = false;
        return View();
    }

    [HttpPost]
    public IActionResult AddUser(UserDTO userDto)
    {

        _userService.AddUser(userDto);
        return RedirectToAction("Index");  // Kullanıcı ekledikten sonra Admin Paneli sayfasına dön
    }

    // // Kullanıcı Düzenleme
    // public IActionResult EditUser(int id)
    // {

    //     ViewData["HideTopbar"] = false;
    //     ViewData["HideNavbar"] = false;
    //     ViewData["HideFooter"] = false;

    //     var user = _userService.GetUserById(id);
    //     if (user == null)
    //         return NotFound();
    //     return View(user);  // Kullanıcıyı düzenlemeye gönderiyoruz
    // }

    // Kullanıcı Düzenleme (GET)
    public IActionResult EditUser(int id)
    {
        ViewData["HideTopbar"] = false;
        ViewData["HideNavbar"] = false;
        ViewData["HideFooter"] = false;

        var user = _userService.GetUserById(id);

        if (user == null)
        {
            return NotFound();
        }

        var userDto = new UserDTO
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            City = user.City,
            Role = user.Role,
            Password = user.Password // Password alanını da ekleyelim
        };

        return View(userDto);
    }

    // Kullanıcı Düzenleme (POST)
    [HttpPost]
    public IActionResult EditUser(UserDTO userDto)
    {
        if (!ModelState.IsValid)
        {
            return View(userDto);
        }

        _userService.UpdateUser(userDto);

        return RedirectToAction("UserList");
    }



    // Kullanıcı Silme
    public IActionResult DeleteUser(int id)
    {

        ViewData["HideTopbar"] = false;
        ViewData["HideNavbar"] = false;
        ViewData["HideFooter"] = false;
        _userService.DeleteUser(id);
        return RedirectToAction("Index");  // Kullanıcı silindikten sonra Admin Paneli sayfasına dön
    }





    // Kiralanan Araçlar Listesi
    public IActionResult Rentals()
    {
        ViewData["HideTopbar"] = false;
        ViewData["HideNavbar"] = false;
        ViewData["HideFooter"] = false;

        var rentals = _rentalDbContext.Rentals
            .Select(r => new RentalDTO
            {
                RentalID = r.RentalID,
                CarID = r.CarID,
                UserID = r.UserID,
                PickupOffice = r.PickupOffice,
                ReturnOffice = r.ReturnOffice,
                RentalDate = r.RentalDate,
                ReturnDate = r.ReturnDate,
                RentalTime = r.RentalTime,
                ReturnTime = r.ReturnTime,
                RentalStatus = r.RentalStatus
            })
            .ToList();

        return View(rentals);
    }


    // Kiralama Düzenleme (GET)
    public IActionResult EditRental(int id)
    {
        ViewData["HideTopbar"] = false;
        ViewData["HideNavbar"] = false;
        ViewData["HideFooter"] = false;

        var rental = _rentalDbContext.Rentals
            .Where(r => r.RentalID == id)
            .Select(r => new RentalDTO
            {
                RentalID = r.RentalID,
                CarID = r.CarID,
                UserID = r.UserID,
                PickupOffice = r.PickupOffice,
                ReturnOffice = r.ReturnOffice,
                RentalDate = r.RentalDate,
                ReturnDate = r.ReturnDate,
                RentalTime = r.RentalTime,
                ReturnTime = r.ReturnTime,
                RentalStatus = r.RentalStatus
            })
            .FirstOrDefault();

        if (rental == null)
        {
            return NotFound();
        }

        return View(rental);
    }

    // Kiralama Düzenleme (POST)
    [HttpPost]
    public IActionResult EditRental(RentalDTO rentalDto)
    {
        var rental = _rentalDbContext.Rentals.FirstOrDefault(r => r.RentalID == rentalDto.RentalID);

        if (rental == null)
        {
            return NotFound();
        }

        rental.CarID = rentalDto.CarID;
        rental.UserID = rentalDto.UserID;
        rental.PickupOffice = rentalDto.PickupOffice;
        rental.ReturnOffice = rentalDto.ReturnOffice;
        rental.RentalDate = rentalDto.RentalDate;
        rental.ReturnDate = rentalDto.ReturnDate;
        rental.RentalTime = rentalDto.RentalTime;
        rental.ReturnTime = rentalDto.ReturnTime;
        rental.RentalStatus = rentalDto.RentalStatus;

        _rentalDbContext.SaveChanges();

        return RedirectToAction("Rentals");
    }

    // Kiralama Silme
    public IActionResult DeleteRental(int id)
    {
        ViewData["HideTopbar"] = false;
        ViewData["HideNavbar"] = false;
        ViewData["HideFooter"] = false;
        var rental = _rentalDbContext.Rentals.FirstOrDefault(r => r.RentalID == id);

        if (rental == null)
        {
            return NotFound();
        }

        _rentalDbContext.Rentals.Remove(rental);
        _rentalDbContext.SaveChanges();

        return RedirectToAction("Rentals");
    }
}
