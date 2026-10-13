using Microsoft.AspNetCore.SignalR;
using ProductCatalogApp.Services;

namespace ProductCatalogApp.Hubs
{
    // Хаб SignalR для мониторинга датчиковой аппаратуры в реальном времени.
    // Клиенты подключаются по адресу /hubs/monitoring (см. Program.cs).
    public class MonitoringHub : Hub
    {
        // Ключ в Context.Items, под которым хранится текущая группа подключения.
        private const string CurrentGroupKey = "CurrentGroup";

        private readonly OnlineUserTracker _onlineUsers;
        private readonly SensorReadingService _readingService;

        public MonitoringHub(
            OnlineUserTracker onlineUsers,
            SensorReadingService readingService)
        {
            _onlineUsers = onlineUsers;
            _readingService = readingService;
        }

        // Подключение клиента: учитываем его среди пользователей онлайн
        // и сообщаем всем клиентам новое количество подключённых.
        public override async Task OnConnectedAsync()
        {
            string? userName = Context.User?.Identity?.IsAuthenticated == true
                ? Context.User.Identity.Name
                : null;

            _onlineUsers.Connect(Context.ConnectionId, userName);

            await Clients.All.SendAsync(
                MonitoringClientMethods.OnlineChanged,
                _onlineUsers.GetSummary());

            await base.OnConnectedAsync();
        }

        // Отключение клиента: убираем его из списка онлайн.
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _onlineUsers.Disconnect(Context.ConnectionId);

            await Clients.All.SendAsync(
                MonitoringClientMethods.OnlineChanged,
                _onlineUsers.GetSummary());

            await base.OnDisconnectedAsync(exception);
        }

        // Серверный метод 1 (доступен всем, включая гостей).
        // Переводит подключение в группу выбранной производственной площадки
        // (null — все площадки) и отправляет вызывающему клиенту снимок
        // текущего состояния: датчики, журнал показаний, активные тревоги.
        public async Task JoinSite(int? productionSiteId)
        {
            if (productionSiteId.HasValue &&
                !await _readingService.ProductionSiteExistsAsync(
                    productionSiteId.Value))
            {
                throw new HubException(
                    "Производственная площадка не найдена.");
            }

            string newGroup = productionSiteId.HasValue
                ? MonitoringGroups.ForSite(productionSiteId.Value)
                : MonitoringGroups.AllSites;

            if (Context.Items.TryGetValue(CurrentGroupKey, out var value) &&
                value is string currentGroup &&
                currentGroup != newGroup)
            {
                await Groups.RemoveFromGroupAsync(
                    Context.ConnectionId,
                    currentGroup);
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, newGroup);
            Context.Items[CurrentGroupKey] = newGroup;

            var snapshot = await _readingService.GetSnapshotAsync(
                productionSiteId);

            await Clients.Caller.SendAsync(
                MonitoringClientMethods.ReceiveSnapshot,
                snapshot);
        }
    }
}
