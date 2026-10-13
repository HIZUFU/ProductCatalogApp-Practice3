using System.ComponentModel.DataAnnotations;

namespace ProductCatalogApp.Models
{
    // Показание датчика — полезная нагрузка, которая передаётся
    // клиентам в реальном времени и сохраняется в базе данных.
    public class SensorReading
    {
        public int Id { get; set; }

        [Display(Name = "Датчик")]
        public int SensorId { get; set; }

        public Sensor? Sensor { get; set; }

        [Display(Name = "Значение")]
        public double Value { get; set; }

        // Время получения показания (UTC).
        [Display(Name = "Время")]
        public DateTime Timestamp { get; set; }

        // Источник показания: имитатор датчиков или имя оператора.
        [StringLength(100)]
        [Display(Name = "Источник")]
        public string Source { get; set; } = string.Empty;

        // Значение вышло за границы нормы датчика.
        [Display(Name = "Тревога")]
        public bool IsAlarm { get; set; }

        [Display(Name = "Квитировано")]
        public bool IsAcknowledged { get; set; }

        [StringLength(100)]
        [Display(Name = "Квитировал")]
        public string? AcknowledgedBy { get; set; }

        // Время квитирования тревоги (UTC).
        [Display(Name = "Время квитирования")]
        public DateTime? AcknowledgedAt { get; set; }
    }
}
