using WebBanHang.Models;
using Xunit;

namespace WebBanHang.Tests;

public class OrderTests
{
    [Fact]
    public void CalculateTotal_ReturnsSumOfItems()
    {
        var order = new Order();
        order.Items.Add(new OrderItem { Price = 100, Quantity = 2 });
        order.Items.Add(new OrderItem { Price = 50, Quantity = 1 });

        Assert.Equal(250, order.CalculateTotal());
    }

    [Fact]
    public void CalculateTotal_EmptyOrder_ReturnsZero()
    {
        Assert.Equal(0, new Order().CalculateTotal());
    }
}
