namespace KuaforUygulamasi.Models
{
    public class Role
    {
        public int Id { get; set; } // Primary Key
        public string Name { get; set; } // Role Name (e.g., Admin, User, etc.)

        // Navigation Properties
        public ICollection<User> Users { get; set; } // Role can have multiple Users
    }
}