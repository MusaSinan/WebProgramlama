using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KuaforUygulamasi.Models
{
    public class User
    {
        public int Id { get; set; } // Primary Key
        [Required]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        public int RoleId { get; set; } // Foreign Key referring to Roles Table
        public DateTime CreatedTime { get; set; } // Created Time

        // Navigation Properties
        public Role? Role { get; set; } // User belongs to a Role
        public ICollection<Appointment>? Appointments { get; set; } // User can have multiple Appointments
    }
}