using Microsoft.AspNetCore.SignalR;
using ProductCatalogApp.Services;

namespace ProductCatalogApp.Hubs
{
    // Хаб SignalR для мониторинга датчиковой аппаратуры в реальном времени.
    // Клиенты подключаются по адресу /hubs/monitoring (см. Program.cs).
    public class MonitoringHub : Hub
    {
        private readonly OnlineUserTracker _onlineUsers;

        public MonitoringHub(OnlineUserTracker onlineUsers)
        {
            _onlineUsers = onlineUsers;
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
    }
}
