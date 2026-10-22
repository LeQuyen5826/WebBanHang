using System.ComponentModel.DataAnnotations;

namespace WebBanHang.Models;

public class Order
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string CustomerName { get; set; } = "";

    public DateTime OrderDate { get; set; } = DateTime.Now;

    public List<OrderItem> Items { get; set; } = new();

    // Ham tinh tien don hang - dung cho Unit Test (tinh huong 3, Yeu cau 5)
    public decimal CalculateTotal() => Items.Sum(i => i.Price * i.Quantity);
}

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
