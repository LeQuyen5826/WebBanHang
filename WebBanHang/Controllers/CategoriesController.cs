using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Data;
using WebBanHang.Models;

namespace WebBanHang.Controllers;

public class CategoriesController : Controller
{
    private readonly AppDbContext _db;
    public CategoriesController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index() =>
        View(await _db.Categories.AsNoTracking().OrderBy(c => c.Name).ToListAsync());

    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category c)
    {
        if (!ModelState.IsValid) return View(c);
        _db.Categories.Add(c);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var c = await _db.Categories.FindAsync(id);
        return c == null ? NotFound() : View(c);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Category c)
    {
        if (id != c.Id) return BadRequest();
        if (!ModelState.IsValid) return View(c);
        _db.Update(c);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var c = await _db.Categories.FindAsync(id);
        if (c != null)
        {
            _db.Categories.Remove(c);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
