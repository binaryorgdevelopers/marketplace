using System.Globalization;

namespace Basket.Entities;

public class ShoppingCart
{
    public ShoppingCart(Guid username)
    {
        Username = username;
    }

    public Guid Username { get; set; }
    public string Date => DateTime.Today.ToString(CultureInfo.InvariantCulture);
    public List<ShoppingCartItem> Items { get; set; } = new();

    public decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);
}