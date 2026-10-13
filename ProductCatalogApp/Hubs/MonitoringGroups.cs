namespace ProductCatalogApp.Hubs
{
    // Группы SignalR: клиент состоит в группе выбранной площадки
    // либо в группе «все площадки».
    public static class MonitoringGroups
    {
        public const string AllSites = "site-all";

        public static string ForSite(int productionSiteId)
        {
            return $"site-{productionSiteId}";
        }
    }
}
