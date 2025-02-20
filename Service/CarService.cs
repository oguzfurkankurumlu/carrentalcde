public class CarService : ICarService
{
    private readonly ICarRepository _carRepository;

    public CarService(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    public List<CarDTO> GetAllCars()
    {
        var cars = _carRepository.GetAllCars();
        return cars.Select(car => new CarDTO
        {
            CarId = car.CarId ?? 0,
            Model = car.Model,
            Make = car.Make,
            Year = car.Year,
            PricePerDay = car.PricePerDay,
            IsAvailable = car.IsAvailable,
            Description = car.Description ?? "Açıklama Yok",
            Color = car.Color ?? "Bilinmiyor",
            ImageUrl = car.ImageUrl ?? "/images/default.png",
            Mileage = car.Mileage,
            IsAirConditioned = car.IsAirConditioned,
            IsAutomatic = car.IsAutomatic,
            CarType = car.CarType ?? "Bilinmiyor",
            FuelType = car.FuelType ?? "Bilinmiyor"
        }).ToList();
    }

    public CarDTO? GetCarById(int id)
    {
        var car = _carRepository.GetCarById(id);
        if (car == null)
            return null;

        return new CarDTO
        {
            CarId = car.CarId ?? 0,
            Model = car.Model,
            Make = car.Make,
            Year = car.Year,
            PricePerDay = car.PricePerDay,
            IsAvailable = car.IsAvailable,
            Description = car.Description ?? "Açıklama Yok",
            Color = car.Color ?? "Bilinmiyor",
            ImageUrl = car.ImageUrl ?? "/images/default.png",
            Mileage = car.Mileage,
            IsAirConditioned = car.IsAirConditioned,
            IsAutomatic = car.IsAutomatic,
            CarType = car.CarType ?? "Bilinmiyor",
            FuelType = car.FuelType ?? "Bilinmiyor"
        };
    }

    public void AddCar(CarDTO carDto)
    {
        // Resim URL'sini almak ve varsayılan bir değer atamak
        var imagePath = carDto.ImageUrl ?? "/images/default.png";  // Resim URL'si ya carDto'dan alınacak ya da varsayılan olacak

        var car = new Car
        {
            Make = carDto.Make ?? "Bilinmiyor",  // Default value if null
            Model = carDto.Model ?? "Bilinmiyor",  // Default value if null
            Year = carDto.Year ?? 2000,  // Default to 2000 if null
            PricePerDay = string.IsNullOrEmpty(carDto.PricePerDay?.ToString()) ? 100 : carDto.PricePerDay,  // Null or empty kontrolü
            IsAvailable = carDto.IsAvailable ?? false,  // Default to false if null
            Description = carDto.Description ?? "Açıklama Yok",
            Color = carDto.Color ?? "Bilinmiyor",
            Mileage = carDto.Mileage ?? 0,  // Default to 0 if null
            IsAirConditioned = carDto.IsAirConditioned ?? false,  // Default to false if null
            IsAutomatic = carDto.IsAutomatic ?? false,  // Default to false if null
            CarType = carDto.CarType ?? "Bilinmiyor",
            FuelType = carDto.FuelType ?? "Bilinmiyor",
            ImageUrl = imagePath  // URL resmini kullanıyoruz
        };

        _carRepository.AddCar(car);
    }

    public void UpdateCar(int id, CarDTO carDto)
    {
        var car = _carRepository.GetCarById(id);
        if (car == null)
            return;

        car.Make = carDto.Make ?? car.Make;
        car.Model = carDto.Model ?? car.Model;
        car.Year = carDto.Year ?? car.Year;
        car.PricePerDay = carDto.PricePerDay ?? car.PricePerDay;
        car.IsAvailable = carDto.IsAvailable ?? car.IsAvailable;
        car.Description = carDto.Description ?? car.Description;
        car.Color = carDto.Color ?? car.Color;
        car.ImageUrl = carDto.ImageUrl ?? car.ImageUrl;
        car.Mileage = carDto.Mileage ?? car.Mileage;
        car.IsAirConditioned = carDto.IsAirConditioned ?? car.IsAirConditioned;
        car.IsAutomatic = carDto.IsAutomatic ?? car.IsAutomatic;
        car.CarType = carDto.CarType ?? car.CarType;
        car.FuelType = carDto.FuelType ?? car.FuelType;

        _carRepository.UpdateCar(car);
    }

    public void DeleteCar(int id)
    {
        _carRepository.DeleteCar(id);
    }
}
