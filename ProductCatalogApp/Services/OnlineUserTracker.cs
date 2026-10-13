using System.Collections.Concurrent;
using ProductCatalogApp.Models;

namespace ProductCatalogApp.Services
{
    // Хранит список активных подключений к хабу мониторинга.
    // Регистрируется как Singleton, т.к. экземпляры хаба создаются
    // заново на каждый вызов.
    public class OnlineUserTracker
    {
        // Идентификатор подключения -> имя пользователя (null — гость).
        private readonly ConcurrentDictionary<string, string?> _connections =
            new();

        public void Connect(string connectionId, string? userName)
        {
            _connections[connectionId] = userName;
        }

        public void Disconnect(string connectionId)
        {
            _connections.TryRemove(connectionId, out _);
        }

        public OnlineSummary GetSummary()
        {
            var names = _connections.Values.ToList();

            // Пользователь может открыть несколько вкладок,
            // поэтому имена считаются без повторов.
            var userNames = names
                .OfType<string>()
                .Distinct()
                .OrderBy(name => name)
                .ToList();

            int guestCount = names.Count(name => name == null);

            return new OnlineSummary
            {
                UserNames = userNames,
                GuestCount = guestCount,
                TotalCount = userNames.Count + guestCount
            };
        }
    }
}
