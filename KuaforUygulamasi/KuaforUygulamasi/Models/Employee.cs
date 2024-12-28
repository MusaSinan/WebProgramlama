namespace KuaforUygulamasi.Models
{
    public class Employee
    {
        public int Id { get; set; } // Primary Key
        public string FullName { get; set; } // Çalışan adı
        public int ServiceId { get; set; } // Foreign Key (Hizmet tipi)
        public bool IsAvailable { get; set; } // Çalışanın uygunluğu (boşta mı, çalışıyor mu?)
        public DateTime AvailabilityStart { get; set; } // Çalışanın uygun olduğu başlangıç saati
        public DateTime AvailabilityEnd { get; set; } // Çalışanın uygun olduğu bitiş saati

        // Navigation Properties
        public Service? Service { get; set; } // Employee provides a specific Service
        public ICollection<Appointment>? Appointments { get; set; } // Employee has multiple Appointments
    }
}
