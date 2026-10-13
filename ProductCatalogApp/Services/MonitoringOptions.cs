namespace ProductCatalogApp.Services
{
    // Настройки мониторинга из раздела "Monitoring" файла appsettings.json.
    public class MonitoringOptions
    {
        public const string SectionName = "Monitoring";

        // Включён ли имитатор датчиков при запуске приложения.
        public bool SimulationEnabled { get; set; } = true;

        // Период опроса датчиков имитатором, секунд.
        public int SimulationIntervalSeconds { get; set; } = 5;

        // Вероятность выброса значения за границы нормы
        // при каждом опросе датчика (от 0 до 1).
        public double AlarmProbability { get; set; } = 0.01;
    }
}
