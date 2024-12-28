using KuaforUygulamasi.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// PostgreSQL bağlantısı için DbContext'i yapılandırıyoruz
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cookie Authentication'ı ekliyoruz
builder.Services.AddAuthentication("CookieAuthentication")
    .AddCookie("CookieAuthentication", options =>
    {
        options.LoginPath = "/Authentication/Login"; // Kullanıcı giriş sayfası
        options.LogoutPath = "/Authentication/Logout"; // Kullanıcı çıkış sayfası
        options.AccessDeniedPath = "/Authentication/AccessDenied"; // Yetkisiz erişim için yönlendirme
    });

builder.Services.AddAuthorization(); // Yetkilendirme hizmetini ekliyoruz
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Yönlendirme işlemi
app.UseRouting();

// Authentication ve Authorization middleware'lerini doğru sırayla ekliyoruz
app.UseAuthentication(); // Kimlik doğrulama işlemi
app.UseAuthorization();  // Yetkilendirme işlemi

// Bu sırada yetkilendirme middleware'inin doğru bir şekilde eklenmiş olması gerekir.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();