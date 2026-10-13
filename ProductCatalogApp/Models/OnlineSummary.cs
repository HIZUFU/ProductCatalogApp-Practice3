namespace ProductCatalogApp.Models
{
    // Сведения о подключённых к хабу мониторинга клиентах.
    public class OnlineSummary
    {
        // Всего онлайн: авторизованные пользователи и гостевые подключения.
        public int TotalCount { get; set; }

        public int GuestCount { get; set; }

        public List<string> UserNames { get; set; } = new();
    }
}
