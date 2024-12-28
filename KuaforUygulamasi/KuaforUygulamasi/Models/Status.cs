namespace KuaforUygulamasi.Models
{
    public class Status
    {
        public int Id { get; set; } // Primary Key
        public string Name { get; set; } // Durum adı (örneğin "Aktif", "Pasif", vb.)

        // Navigation Properties
        public ICollection<Appointment> Appointments { get; set; } // Status can be associated with multiple Appointments
    }
}