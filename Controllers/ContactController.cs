
using Microsoft.AspNetCore.Mvc;

public class ContactController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }




    
}