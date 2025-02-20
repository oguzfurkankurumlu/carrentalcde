
using Microsoft.AspNetCore.Mvc;

public class AboutController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}