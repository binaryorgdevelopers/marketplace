using System.ComponentModel.DataAnnotations;

namespace Basket.Entities;

public class ShoppingCartItem
{
    public int Quantity { get; set; }
    public string Color { get; set; }
    public decimal Price { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        var results = new List<ValidationResult>();
        if (Quantity < 1) results.Add(new ValidationResult("Invalid number of units", new[] { "Quantity" }));
        return results;
    }
}