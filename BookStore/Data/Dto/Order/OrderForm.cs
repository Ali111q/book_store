namespace BookStore.Data.Dto.Order;

public class OrderForm
{
    public List<OrderItemForm> Items { get; set; }
    public string? Notes { get; set; }
}

public class OrderItemForm
{
    public Guid BookId { get; set; }
    public int Count { get; set; }
}