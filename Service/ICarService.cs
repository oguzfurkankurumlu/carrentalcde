public interface ICarService
{
    List<CarDTO> GetAllCars();
    CarDTO GetCarById(int id);
    void AddCar(CarDTO carDto); // Sadece CarDTO alacak şekilde güncellendi
    void UpdateCar(int id, CarDTO carDto);
    void DeleteCar(int id);
}
