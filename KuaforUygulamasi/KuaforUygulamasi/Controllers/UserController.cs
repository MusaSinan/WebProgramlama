using KuaforUygulamasi.Context;
using KuaforUygulamasi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KuaforUygulamasi.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserController(IHttpClientFactory httpClientFactory, ApplicationDbContext context)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
        }

        public async Task<IActionResult> GetAppointments()
        {
            // API endpoint URL'sini belirtiyoruz
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetStringAsync("https://localhost:7181/api/UserApi");  // Buradaki URL'yi kendi API URL'inizle değiştirdiğinizden emin olun.

            // JSON verisini deserialize ediyoruz
            var appointments = JsonConvert.DeserializeObject<List<Appointment>>(response);

            // Verileri View'a gönderiyoruz
            return View(appointments);
        }

        // Randevu durumunu güncellemek için
        [HttpPost]
        public async Task<IActionResult> TakeAppointment(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(GetAppointments));
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage); // Hata mesajlarını kontrol et
                }
            }
            return View(appointment);
        }


        public IActionResult Home() {
            return View();
        }
    }
}
