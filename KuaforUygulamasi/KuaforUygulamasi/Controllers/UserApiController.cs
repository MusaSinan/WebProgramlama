using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KuaforUygulamasi.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KuaforUygulamasi.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KuaforUygulamasi.Controllers
{
    [ApiController]  // API Controller olarak işaretler
    [Route("api/[controller]")]
    public class UserApiController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public UserApiController(ApplicationDbContext context)
        {
            _context = context;

        }

        [HttpGet]
        public IActionResult Get()
        {
            var appointments = _context.Appointments
                .Include(e => e.User)
                .Include(e => e.Employee)
                .Include(e => e.Service)
                .Include(e => e.Status)
                .Where(e=> e.UserId == 3)
                .ToListAsync();
            return Ok(appointments);
        }
    }
}

