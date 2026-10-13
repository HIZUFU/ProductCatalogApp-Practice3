using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ProductCatalogApp.Models
{
    public class ProductionSite
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

         public List<Equipment>? EquipmentItems { get; set; } = new();
    }
}
