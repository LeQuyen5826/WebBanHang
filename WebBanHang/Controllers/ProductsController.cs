using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Data;
using WebBanHang.Models;

namespace WebBanHang.Controllers;

public class ProductsController : Controller
{
    private readonly AppDbContext _db;
    public ProductsController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index() =>
        View(await _db.Products.Include(p => p.Category).AsNoTracking().OrderBy(p => p.Name).ToListAsync());

    public IActionResult Create()
    {
        LoadCategories();
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product p)
    {
        if (!ModelState.IsValid)
        {
            LoadCategories(p.CategoryId);
            return View(p);
        }
        _db.Products.Add(p);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var p = await _db.Products.FindAsync(id);
        if (p == null) return NotFound();
        LoadCategories(p.CategoryId);
        return View(p);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product p)
    {
        if (id != p.Id) return BadRequest();
        if (!ModelState.IsValid)
        {
            LoadCategories(p.CategoryId);
            return View(p);
        }
        _db.Update(p);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var p = await _db.Products.FindAsync(id);
        if (p != null)
        {
            _db.Products.Remove(p);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private void LoadCategories(int? selected = null) =>
        ViewBag.CategoryId = new SelectList(_db.Categories.AsNoTracking().OrderBy(c => c.Name), "Id", "Name", selected);
}
