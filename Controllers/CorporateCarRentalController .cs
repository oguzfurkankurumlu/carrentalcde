using Microsoft.AspNetCore.Mvc;

public class CorporateCarRentalController : Controller
{
    public IActionResult Index()
    {
        ViewData["HideNavbar"] = false;

        ViewData["HideFooter"] = false;
        return View(); // Bu, Kurumsal Araç Kiralama sayfanızı gösterecek
    }
}
