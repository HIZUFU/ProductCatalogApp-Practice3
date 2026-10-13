using System.ComponentModel.DataAnnotations;

namespace ProductCatalogApp.Models
{
    public class AppUser
    {
        public int Id { get; set; }

        [Required] public string Username { get; set; } = string.Empty;

        [Required] public string PasswordHash { get; set; } = string.Empty;

        public int RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
