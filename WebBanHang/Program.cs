using Microsoft.EntityFrameworkCore;
using WebBanHang.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// SQLite: khong can cai SQL Server tren may Jenkins/IIS
var dataDir = Path.Combine(builder.Environment.ContentRootPath, "App_Data");
Directory.CreateDirectory(dataDir);
var connStr = builder.Configuration.GetConnectionString("Default")
              ?? $"Data Source={Path.Combine(dataDir, "webbanhang.db")}";
builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlite(connStr));

var app = builder.Build();

// Yeu cau 4: khi chay duoi /webbanhang, dat "PathBase": "/webbanhang"
// trong appsettings.Production.json tren may chu (mac dinh de trong)
var pathBase = app.Configuration["PathBase"];
if (!string.IsNullOrEmpty(pathBase))
{
    app.UsePathBase(pathBase);
}

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
