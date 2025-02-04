using black_follow.Entity;

namespace BookStore.Data.Dto.Order;

public class ChangeOrderStateForm
{
    public Guid OrderId { get; set; }
    public OrderStatus State { get; set; }
}