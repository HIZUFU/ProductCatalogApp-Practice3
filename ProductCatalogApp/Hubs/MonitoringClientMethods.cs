namespace ProductCatalogApp.Hubs
{
    // Имена методов, которые сервер вызывает у клиентов.
    // На стороне клиента им соответствуют обработчики hubConnection.on(...).
    public static class MonitoringClientMethods
    {
        // Количество и имена пользователей онлайн (всем клиентам).
        public const string OnlineChanged = "OnlineChanged";

        // Снимок текущего состояния мониторинга (только отправителю).
        public const string ReceiveSnapshot = "ReceiveSnapshot";

        // Новое показание датчика (группам площадок).
        public const string ReceiveReading = "ReceiveReading";

        // Показание вышло за границы нормы (всем клиентам).
        public const string AlarmRaised = "AlarmRaised";

        // Подтверждение сохранения показания оператора (только отправителю).
        public const string ReadingAccepted = "ReadingAccepted";

        // Администратор квитировал тревогу (всем клиентам).
        public const string AlarmAcknowledged = "AlarmAcknowledged";

        // Имитатор датчиков включён или выключен (всем клиентам).
        public const string SimulationStateChanged = "SimulationStateChanged";
    }
}
