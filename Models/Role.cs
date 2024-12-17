namespace Zentech.Models
{
    public class Role
    {
        public int RoleID { get; set; } 
        public string RoleName { get; set; } // Nom du r�le (e.g., Admin, User)

        public List<User> Users { get; set; } // List of users associated with this role
    }
}
