using black_follow.Entity;
using BookStore.Data.Dto.Base;

namespace BookStore.Data.Dto.Order;

public class OrderFilter:BaseFilter
{
    public OrderStatus? OrderStatus { get; set; }
    public Guid? UserId { get; set; }
}