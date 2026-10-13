namespace ProductCatalogApp.Models
{
    // Текущее состояние мониторинга, которое получает клиент
    // после выбора производственной площадки.
    public class MonitoringSnapshot
    {
        // Выбранная площадка (null — все площадки).
        public int? ProductionSiteId { get; set; }

        public List<SensorStateDto> Sensors { get; set; } = new();

        // Последние показания выбранной площадки (новые — первыми).
        public List<SensorReadingDto> RecentReadings { get; set; } = new();

        // Неквитированные тревоги по всем площадкам.
        public List<SensorReadingDto> ActiveAlarms { get; set; } = new();
    }
}
