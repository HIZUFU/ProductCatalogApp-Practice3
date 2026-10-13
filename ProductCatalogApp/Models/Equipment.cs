namespace ProductCatalogApp.Models
{
    public class Equipment
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public string InventoryNumber { get; set; } = string.Empty;

        public int ProductionSiteId { get; set; }

        public ProductionSite? ProductionSite { get; set; }
    }
}
