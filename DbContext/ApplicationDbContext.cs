using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    { }

    // Mevcut veritabanındaki Cars tablosunu DbSet olarak ekliyoruz
    public DbSet<Car> Cars { get; set; }
}
