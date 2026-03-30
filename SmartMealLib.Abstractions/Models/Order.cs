namespace SmartMealLib.Abstractions.Models;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public List<OrderItem> Items { get; set; } = new();
}