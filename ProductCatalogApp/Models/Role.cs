using System.ComponentModel.DataAnnotations;

namespace ProductCatalogApp.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required] public string Name { get; set; } = string.Empty;

        public List<AppUser> Users { get; set; } = new();
    }
}
