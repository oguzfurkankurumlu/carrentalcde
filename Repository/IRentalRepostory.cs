public interface IRentalRepository
{
    Rental GetRentalById(int rentalId);
    IEnumerable<Rental> GetRentalsByUserId(string userId);
    void AddRental(Rental rental);
    void UpdateRental(Rental rental);
    void DeleteRental(int rentalId);
}