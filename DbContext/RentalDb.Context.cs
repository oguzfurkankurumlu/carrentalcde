using Microsoft.EntityFrameworkCore;

public class RentalDbContext : DbContext
{
    public RentalDbContext(DbContextOptions<RentalDbContext> options)
        : base(options)
    { }

    // Veritabanındaki tabloları DbSet olarak ekliyoruz
    public DbSet<Rental> Rentals { get; set; }  // Rental modeline karşılık gelen tablo

}
