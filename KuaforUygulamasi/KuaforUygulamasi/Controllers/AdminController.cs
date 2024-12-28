using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KuaforUygulamasi.Models;
using KuaforUygulamasi.Context;

namespace KuaforUygulamasi.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
            
        }

        public async Task<IActionResult> Home() {
            return View();
        }

        // Çalışanlar Listeleme
        public async Task<IActionResult> ListEmployees()
        {
            var employees = await _context.Employees.Include(e => e.Service).ToListAsync();
            return View(employees);
        }

        // Çalışan Ekleme
        [HttpGet]
        public IActionResult AddEmployee()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.AvailabilityStart = TimeZoneInfo.ConvertTimeToUtc(employee.AvailabilityStart);
                employee.AvailabilityEnd = TimeZoneInfo.ConvertTimeToUtc(employee.AvailabilityEnd);
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListEmployees));
            }
            else {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage); // Hata mesajlarını kontrol et
                }
            }
            return View(employee);
        }

        // Çalışan Güncelleme
        [HttpGet]
        public async Task<IActionResult> EditEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.AvailabilityStart = TimeZoneInfo.ConvertTimeToUtc(employee.AvailabilityStart);
                employee.AvailabilityEnd = TimeZoneInfo.ConvertTimeToUtc(employee.AvailabilityEnd);
                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListEmployees));
            }
            return View(employee);
        }

        // Çalışan Silme
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ListEmployees));
        }

        // Hizmetler Listeleme
        public async Task<IActionResult> ListServices()
        {
            var services = await _context.Services.ToListAsync();

            return View(services);
        }

        // Hizmet Ekleme
        [HttpGet]
        public IActionResult AddService()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddService(Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Services.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListServices));
            }
            else {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage); // Hata mesajlarını kontrol et
                }
            }
            return View(service);
        }

        // Hizmet Güncelleme
        [HttpGet]
        public async Task<IActionResult> EditService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        [HttpPost]
        public async Task<IActionResult> EditService(Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Services.Update(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListServices));
            }
            return View(service);
        }

        // Hizmet Silme
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ListServices));
        }

        // Randevu Listeleme
        public async Task<IActionResult> ListAppointments()
        {
            var appointments = await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Employee)
                .Include(a => a.Service)
                .Include(a => a.Status)
                .ToListAsync();
            return View(appointments);
        }

        // Randevu Durum Güncelleme
        [HttpGet]
        public async Task<IActionResult> ChangeAppointmentStatus(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return View(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeAppointmentStatus(int id, int statusId)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                appointment.StatusId = statusId;
                _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListAppointments));
            }
            return NotFound();
        }
    }
}