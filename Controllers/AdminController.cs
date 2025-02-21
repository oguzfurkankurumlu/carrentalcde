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

    // Kullanıcı Düzenleme
    public IActionResult EditUser(int id)
    {

        ViewData["HideTopbar"] = false;
        ViewData["HideNavbar"] = false;
        ViewData["HideFooter"] = false;

        var user = _userService.GetUserById(id);
        if (user == null)
            return NotFound();
        return View(user);  // Kullanıcıyı düzenlemeye gönderiyoruz
    }

    [HttpPost]
    public IActionResult EditUser(UserDTO userDto)
    {
        ViewData["HideTopbar"] = false;
        ViewData["HideNavbar"] = false;
        ViewData["HideFooter"] = false;
        if (ModelState.IsValid)
        {
            Console.WriteLine("Gelen UserId: " + userDto.UserId); // ID'nin gelip gelmediğini kontrol et
            _userService.UpdateUser(userDto);
            return RedirectToAction("Index");
        }
        return View(userDto);
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
}
