
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalogApp.Data;
using ProductCatalogApp.Models;

public class ProductionSitesController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductionSitesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: PRODUCTIONSITES
    public async Task<IActionResult> Index()    
    {
        return View(await _context.ProductionSites.ToListAsync());
    }

    // GET: PRODUCTIONSITES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var productionSite = await _context.ProductionSites
            .Include(site => site.EquipmentItems)
            .FirstOrDefaultAsync(site => site.Id == id);

        if (productionSite == null)
        {
            return NotFound();
        }

        return View(productionSite);
    }

    // GET: PRODUCTIONSITES/Create
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: PRODUCTIONSITES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([Bind("Id,Name,Description,EquipmentItems")] ProductionSite productionsite)
    {
        if (ModelState.IsValid)
        {
            _context.Add(productionsite);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(productionsite);
    }

    // GET: PRODUCTIONSITES/Edit/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var productionsite = await _context.ProductionSites.FindAsync(id);
        if (productionsite == null)
        {
            return NotFound();
        }
        return View(productionsite);
    }

    // POST: PRODUCTIONSITES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Description,EquipmentItems")] ProductionSite productionsite)
    {
        if (id != productionsite.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(productionsite);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductionSiteExists(productionsite.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(productionsite);
    }

    // GET: PRODUCTIONSITES/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var productionsite = await _context.ProductionSites
            .FirstOrDefaultAsync(m => m.Id == id);
        if (productionsite == null)
        {
            return NotFound();
        }

        return View(productionsite);
    }

    // POST: PRODUCTIONSITES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var productionsite = await _context.ProductionSites.FindAsync(id);
        if (productionsite != null)
        {
            _context.ProductionSites.Remove(productionsite);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProductionSiteExists(int? id)
    {
        return _context.ProductionSites.Any(e => e.Id == id);
    }
}
