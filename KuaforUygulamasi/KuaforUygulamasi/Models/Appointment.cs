namespace KuaforUygulamasi.Models
{
    public class Appointment
    {
        public int Id { get; set; } // Primary Key
        public int UserId { get; set; } // Foreign Key - Kullanıcı
        public int EmployeeId { get; set; } // Foreign Key - Çalışan
        public int ServiceId { get; set; } // Foreign Key - Hizmet
        public int StatusId { get; set; } // Foreign Key - Durum
        public DateTime AppointmentTime { get; set; } // Randevu Zamanı
        public int Duration { get; set; } // Süre (dakika)
        public decimal Price { get; set; } // Fiyat

        // Navigation Properties
        public User ? User { get; set; } // Appointment belongs to a User
        public Employee ? Employee { get; set; } // Appointment is assigned to an Employee
        public Service ? Service { get; set; } // Appointment is for a specific Service
        public Status ? Status { get; set; } // Appointment has a Status
    }
}
