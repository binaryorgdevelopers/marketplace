namespace Shared.Models;

public class Order
{
    public Guid Id { get; set; }
    public DateTime Time { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string User { get; set; }
    public int Quantity { get; set; }
    public OrderStatus Status { get; set; }
}

public enum OrderStatus
{
    New,
    Pending,
    Processed
}