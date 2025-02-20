using Microsoft.EntityFrameworkCore;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }

    // Veritabanı tablolarını temsil eden DbSet'ler
    public DbSet<User> Users { get; set; }  // User modeline karşılık gelen tablo
}