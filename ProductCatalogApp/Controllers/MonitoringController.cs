using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductCatalogApp.Data;

namespace ProductCatalogApp.Controllers
{
    // Страница мониторинга датчиков в реальном времени.
    // Доступна всем, включая гостей: данные страница получает
    // через хаб SignalR (/hubs/monitoring), а не из этого контроллера.
    [AllowAnonymous]
    public class MonitoringController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MonitoringController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Monitoring
        public async Task<IActionResult> Index()
        {
            // В списке выбора — только площадки, на оборудовании
            // которых установлены датчики.
            var productionSites = await _context.ProductionSites
                .Where(site => _context.Sensors.Any(sensor =>
                    sensor.Equipment!.ProductionSiteId == site.Id))
                .OrderBy(site => site.Name)
                .ToListAsync();

            ViewData["ProductionSiteId"] = new SelectList(
                productionSites,
                "Id",
                "Name");

            return View();
        }
    }
}
