using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Veritabanı bağlantısını burada yapılandırıyoruz.
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cars tablosu için yeni bir DbContext ekliyoruz
builder.Services.AddDbContext<ApplicationDbContext>(options =>
     options.UseSqlServer(builder.Configuration.GetConnectionString("CarDbContext")));

// Rental için yeni bir DbContext ekliyoruz
builder.Services.AddDbContext<RentalDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("RentalDbContextConnection")));

// IUserService servisini ekliyoruz
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<ICarService, CarService>();
// Repository ve Service katmanlarını DI container'a ekliyoruz
builder.Services.AddScoped<IRentalRepository, RentalRepository>();
builder.Services.AddScoped<IRentalService, RentalService>();

// Session'ı yapılandırıyoruz
builder.Services.AddDistributedMemoryCache(); // Bellek tabanlı cache kullanıyoruz
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session süresi
});
builder.Services.AddHttpContextAccessor(); // HttpContextAccessor ekliyoruz

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddControllersWithViews();
var app = builder.Build();

app.UseSession(); // Session middleware'ini ekliyoruz

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Statik dosyalar için kullanılır
app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();