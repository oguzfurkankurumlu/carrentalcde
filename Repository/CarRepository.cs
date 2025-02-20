using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

public class CarRepository : ICarRepository
{
    private readonly ApplicationDbContext _context;

    public CarRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Car> GetAllCars()
    {
        return _context.Cars.ToList();
    }

    public Car GetCarById(int id)
    {
        return _context.Cars.Find(id);
    }

    public void AddCar(Car car)
    {
        _context.Cars.Add(car);
        _context.SaveChanges();
    }

    public void UpdateCar(Car car)
    {
        _context.Cars.Update(car);
        _context.SaveChanges();
    }

    public void DeleteCar(int id)
    {
        var car = _context.Cars.Find(id);
        if (car != null)
        {
            _context.Cars.Remove(car);
            _context.SaveChanges();
        }
    }
}
