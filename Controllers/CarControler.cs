using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;


public class CarController : Controller
{
    private readonly ICarService _carService;
    private readonly IWebHostEnvironment _webHostEnvironment; // Resim kaydetme işlemi için

    public CarController(ICarService carService, IWebHostEnvironment webHostEnvironment)
    {
        _carService = carService;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Index()
    {
        ViewData["HideTopbar"] = false;
        ViewData["HideNavbar"] = false;
        ViewData["HideFooter"] = false;
        var cars = _carService.GetAllCars();
        return View(cars);
    }

    public IActionResult Details(int id)
    {
        ViewData["HideTopbar"] = false;
        ViewData["HideNavbar"] = false;
        ViewData["HideFooter"] = false;
        var car = _carService.GetCarById(id);
        if (car == null)
            return NotFound();

        return View(car);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewData["HideTopbar"] = false;
        ViewData["HideNavbar"] = false;
        ViewData["HideFooter"] = false;
        return View(new CarDTO()); // Boş bir model nesnesi döndür
    }

    [HttpPost]
    public IActionResult AddCar(CarDTO carDTO)
    {
        if (ModelState.IsValid)
        {
            // CarService'e resim URL'sini göndermek için
            _carService.AddCar(carDTO); // Sadece carDTO alıyoruz, imageUrl'i dahil etmiyoruz

            return RedirectToAction("Index");
        }

        return View(carDTO);
    }


    [HttpGet]
    public IActionResult Edit(int id)
    {
        ViewData["HideTopbar"] = false;
        ViewData["HideNavbar"] = false;
        ViewData["HideFooter"] = false;
        var car = _carService.GetCarById(id);
        if (car == null)
            return NotFound();

        return View(car);
    }

    [HttpPost]
    public IActionResult Edit(int id, CarDTO carDto)
    {
        if (ModelState.IsValid)
        {
            // Resim URL'si, eğer boşsa mevcut URL'yi koruyor
            if (string.IsNullOrEmpty(carDto.ImageUrl))
            {
                carDto.ImageUrl = "/images/default.png"; // Varsayılan resim URL'si
            }

            _carService.UpdateCar(id, carDto);
            return RedirectToAction("Index");
        }

        return View(carDto);
    }

    public IActionResult Delete(int id)
    {
        ViewData["HideTopbar"] = false;
        ViewData["HideNavbar"] = false;
        ViewData["HideFooter"] = false;
        _carService.DeleteCar(id);
        return RedirectToAction("Index");
    }

   








}
