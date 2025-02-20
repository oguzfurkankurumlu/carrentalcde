public class RentalService : IRentalService
{
    private readonly RentalDbContext _context;

    public RentalService(RentalDbContext context)
    {
        _context = context;
    }

    public void AddRental(Rental rental)
    {
        _context.Rentals.Add(rental); // Rental'ı ekle
        _context.SaveChanges(); // Değişiklikleri kaydet
    }

    public void DeleteRental(int rentalId)
    {
        var rental = _context.Rentals.Find(rentalId);
        if (rental != null)
        {
            _context.Rentals.Remove(rental);
            _context.SaveChanges();
        }
    }

    public Rental GetRentalById(int rentalId)
    {
        return _context.Rentals.Find(rentalId);
    }

    public IEnumerable<Rental> GetRentalsByUserId(int userId)
    {
        return _context.Rentals.Where(r => r.UserID == userId).ToList();
    }

    public IEnumerable<Rental> GetRentalsByUserId(string userId)
    {
        throw new NotImplementedException();
    }

    public void UpdateRental(Rental rental)
    {
        _context.Rentals.Update(rental);
        _context.SaveChanges();
    }
}