

using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private readonly UserDbContext _context;

    // DbContext'i constructor üzerinden alıyoruz
    public AccountController(UserDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Register()
    {
        ViewData["HideTopbar"] = false;
        ViewData["HideNavbar"] = false;
        ViewData["HideFooter"] = false;

        return View();
    }

    // Kayıt (Register) işlemi için POST metodu
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(string email, string password, string firstName, string lastName, string city)
    {

        // Eğer kullanıcı zaten varsa kayıt olmasına izin verme
        var existingUser = _context.Users.FirstOrDefault(u => u.Email == email);
        if (existingUser != null)
        {
            ViewBag.Error = "Bu e-posta adresi zaten kullanılıyor.";
            return View();
        }

        // Admin rolü eklemek için kontrol (sadece belirli bir e-posta için admin olacak)
        var role = "User"; // Varsayılan olarak "User" rolü atıyoruz
        if (email == "oguzfurkankurumlu@gmail.com")  // Admin olarak tanımlanacak e-posta
        {
            role = "Admin";  // Eğer bu e-posta ise, admin olarak kaydedilecek
        }


        // Yeni kullanıcı oluştur
        var newUser = new User
        {
            Email = email,
            Password = password,
            FirstName = firstName,
            LastName = lastName,
            City = city,
            Role = role // Rolü atıyoruz
        };

        _context.Users.Add(newUser);
        _context.SaveChanges();

        return RedirectToAction("Login");
    }

    [HttpGet]
    // Giriş (Login) sayfası için GET metodu
    public IActionResult Login()
    {
        ViewData["HideTopbar"] = false;
        ViewData["HideNavbar"] = false;
        ViewData["HideFooter"] = false;
        return View();
    }


    public IActionResult ChangePassword()
    {
        ViewData["HideTopbar"] = false;
        ViewData["HideNavbar"] = false;
        ViewData["HideFooter"] = false;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ChangePassword(ChangePasswordModel model)
    {
        if (ModelState.IsValid)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            if (user.Password != model.OldPassword)
            {
                ModelState.AddModelError("", "Eski şifre yanlış.");
                return View(model);
            }

            user.Password = model.NewPassword;
            _context.SaveChanges();

            ViewBag.Message = "Şifre başarıyla değiştirildi.";
            return View();
        }

        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(string email, string password)
    {
        var user = _context.Users.FirstOrDefault(s => s.Email == email);

        // Email bulunamazsa
        if (user == null)
        {
            ModelState.AddModelError("", "Geçersiz email.");
        }
        // Şifre eşleşmezse
        else if (user.Password != password)
        {
            ModelState.AddModelError("", "Geçersiz şifre.");
        }

        // Hata varsa formu tekrar göster
        if (!ModelState.IsValid)
        {
            // Hatalı giriş olduğu için navbar, topbar ve footer'ı gizliyoruz
            ViewData["HideTopbar"] = false;
            ViewData["HideNavbar"] = false;
            ViewData["HideFooter"] = false;
            return View();
        }

        if (user != null && user.Password == password)
        {
            // Kullanıcı ID'sini ve adını session içinde sakla
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("UserFullName", user.FirstName + " " + user.LastName);
            return RedirectToAction("Index", "Home");
        }

        return View();
    }


    public IActionResult UserPanel()
    {
        ViewData["HideTopbar"] = false;
        ViewData["HideNavbar"] = false;
        ViewData["HideFooter"] = false;

        var userFullName = HttpContext.Session.GetString("UserFullName");
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userFullName == null || userId == null)
        {
            return RedirectToAction("Login");
        }

        var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
        if (user == null)
        {
            return RedirectToAction("Login");
        }

        var userModel = new UserDTO
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            City = user.City,
            Role = user.Role
            // Diğer kullanıcı bilgilerini buraya ekleyin
        };

        return View(userModel);
    }
    public IActionResult Logout()
    {
        // Oturumu sonlandır
        HttpContext.Session.Clear();

        // Giriş sayfasına yönlendir
        return RedirectToAction("Login", "Account");
    }



}

