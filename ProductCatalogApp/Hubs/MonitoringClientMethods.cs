namespace ProductCatalogApp.Hubs
{
    // Имена методов, которые сервер вызывает у клиентов.
    // На стороне клиента им соответствуют обработчики hubConnection.on(...).
    public static class MonitoringClientMethods
    {
        // Количество и имена пользователей онлайн (всем клиентам).
        public const string OnlineChanged = "OnlineChanged";
    }
}
