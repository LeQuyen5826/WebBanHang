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

    [Fact]
    public void CalculateTotal_SingleItem_MultipliesPriceByQuantity()
    {
        var order = new Order();
        order.Items.Add(new OrderItem { Price = 1_500_000, Quantity = 3 });

        Assert.Equal(4_500_000, order.CalculateTotal());
    }

    [Theory]
    [InlineData(10, 1, 10)]
    [InlineData(10, 5, 50)]
    [InlineData(0, 5, 0)]
    public void CalculateTotal_VariousInputs(decimal price, int qty, decimal expected)
    {
        var order = new Order();
        order.Items.Add(new OrderItem { Price = price, Quantity = qty });

        Assert.Equal(expected, order.CalculateTotal());
    }
}
