namespace KuaforUygulamasi.Models
{
    public class Service
    {
        public int Id { get; set; } // Primary Key
        public string Name { get; set; } // Hizmet adı (örneğin, saç kesimi, sakal tıraşı vb.)
        public int DurationMinutes { get; set; } // Süre (dakika cinsinden)
        public decimal Price { get; set; } // Fiyat

        // Navigation Properties
        public ICollection<Employee>? Employees { get; set; } // Service can be provided by multiple Employees
        public ICollection<Appointment>? Appointments { get; set; } // Service can have multiple Appointments
    }
}