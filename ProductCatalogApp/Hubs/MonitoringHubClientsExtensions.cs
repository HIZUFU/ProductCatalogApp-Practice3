using Microsoft.AspNetCore.SignalR;
using ProductCatalogApp.Models;

namespace ProductCatalogApp.Hubs
{
    // Общая логика рассылки показаний клиентам. Используется и самим хабом
    // (свойство Clients), и фоновыми службами через IHubContext<MonitoringHub>.
    public static class MonitoringHubClientsExtensions
    {
        public static async Task SendReadingAsync(
            this IHubClients<IClientProxy> clients,
            SensorReadingDto reading,
            CancellationToken cancellationToken = default)
        {
            // Показание получают группы: подписчики площадки датчика
            // и подписчики всех площадок.
            await clients
                .Groups(
                    MonitoringGroups.AllSites,
                    MonitoringGroups.ForSite(reading.ProductionSiteId))
                .SendAsync(
                    MonitoringClientMethods.ReceiveReading,
                    reading,
                    cancellationToken);

            // Выход за границы нормы — тревога для всех клиентов.
            if (reading.IsAlarm)
            {
                await clients.All.SendAsync(
                    MonitoringClientMethods.AlarmRaised,
                    reading,
                    cancellationToken);
            }
        }
    }
}
