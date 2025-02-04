using BookStore.Data.Dto.Base;

namespace BookStore.Data.Dto.Order;

public class OrderDto:BaseDto<Guid>{
    public List<OrderItemDto> Items { get; set; }
    public string? Notes { get; set; }
    public double TotalPrice { get; set; }
    
}

public class OrderItemDto : BaseDto<Guid>
{
    public string BookName { get; set; }
    public string BookImage { get; set; }
    public int Count { get; set; }
    public double Price { get; set; }
    
}