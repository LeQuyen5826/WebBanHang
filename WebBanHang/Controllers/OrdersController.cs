using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Data;
using WebBanHang.Models;

namespace WebBanHang.Controllers;

public class OrdersController : Controller
{
    private readonly AppDbContext _db;
    public OrdersController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index() =>
        View(await _db.Orders.Include(o => o.Items).AsNoTracking().OrderByDescending(o => o.OrderDate).ToListAsync());

    public async Task<IActionResult> Details(int id)
    {
        var o = await _db.Orders.Include(x => x.Items).ThenInclude(i => i.Product)
                          .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return o == null ? NotFound() : View(o);
    }

    public IActionResult Create()
    {
        LoadProducts();
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string customerName, int productId, int quantity)
    {
        var product = await _db.Products.FindAsync(productId);
        if (string.IsNullOrWhiteSpace(customerName) || product == null || quantity < 1)
        {
            ModelState.AddModelError("", "Vui long nhap day du thong tin hop le.");
            LoadProducts(productId);
            return View();
        }

        var order = new Order { CustomerName = customerName.Trim(), OrderDate = DateTime.Now };
        order.Items.Add(new OrderItem { ProductId = product.Id, Price = product.Price, Quantity = quantity });
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = order.Id });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var o = await _db.Orders.FindAsync(id);
        if (o != null)
        {
            _db.Orders.Remove(o);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private void LoadProducts(int? selected = null) =>
        ViewBag.ProductId = new SelectList(_db.Products.AsNoTracking().OrderBy(p => p.Name), "Id", "Name", selected);
}
