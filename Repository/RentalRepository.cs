public class RentalRepository : IRentalRepository
{
    private readonly RentalDbContext _context;

    public RentalRepository(RentalDbContext context)
    {
        _context = context;
    }

    public Rental GetRentalById(int rentalId)
    {
        return _context.Rentals.FirstOrDefault(r => r.RentalID == rentalId);
    }

    public IEnumerable<Rental> GetRentalsByUserId(string userId)
    {
        int userIdInt = int.Parse(userId);
        return _context.Rentals.Where(r => r.UserID == userIdInt).ToList();
    }

    public void AddRental(Rental rental)
    {
        _context.Rentals.Add(rental);
        _context.SaveChanges();
    }

    public void UpdateRental(Rental rental)
    {
        _context.Rentals.Update(rental);
        _context.SaveChanges();
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
}
