using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KuaforUygulamasi.Context;
using Microsoft.AspNetCore.Mvc;
using KuaforUygulamasi.Models;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;


namespace KuaforUygulamasi.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthenticationController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User model)
        {
            if (ModelState.IsValid)
            {
                bool emailExists = _context.Users.Any(u => u.Email == model.Email);
                bool passwordExists = _context.Users.Any(u => u.Password == model.Password);

                if (emailExists)
                {
                    ViewBag.ErrorMsg = "Kayıtlı Email Bulunmaktadır !!!";
                    return View(model);
                }
                else if (passwordExists)
                {
                    ViewBag.ErrorMsg = "Şifrenizi Değiştiriniz !!!";
                    return View(model);
                }

                model.Password = HashPassword(model.Password); // Şifreyi hashleme
                model.CreatedTime = DateTime.UtcNow;
                model.RoleId = 2; // RoleId'yi kontrol et

                try
                {
                    _context.Users.Add(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata: {ex.Message}");
                    ViewBag.ErrorMsg = "Kayıt sırasında bir hata oluştu. Lütfen tekrar deneyiniz.";
                    return View(model);
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage); // Hata mesajlarını kontrol et
                }
                return View(model);
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            var hashedPassword = HashPassword(password);
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == hashedPassword);

            if (user != null)
            {
                // Kullanıcı rolünü RoleId kullanarak al
                var role = _context.Roles.FirstOrDefault(r => r.Id == user.RoleId)?.Name;

                // Kullanıcı kimliğini oluştur
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.FullName), // Kullanıcının adı
                    new Claim(ClaimTypes.Email, user.Email),  // Kullanıcının email adresi
                    new Claim(ClaimTypes.Role, role),         // Kullanıcının rolü
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) // Kullanıcının ID'si
                };

                var claimsIdentity = new ClaimsIdentity(claims, "CookieAuthentication");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // Cookie ile oturum başlat
                HttpContext.SignInAsync("CookieAuthentication", claimsPrincipal);

                if (role == "Admin")
                {
                    return RedirectToAction("Home", "Admin");
                }
                else {
                    // Kullanıcıyı yönlendir
                    return RedirectToAction("Home", "User");
                }

            }

            ViewBag.ErrorMessage = "Geçersiz email veya şifre.";
            return View();
        }

        // Logout işlemi
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        // Şifreyi hashlemek için bir yardımcı fonksiyon
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }

    }
}

