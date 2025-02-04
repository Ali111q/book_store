using black_follow.Entity;

namespace BookStore.Controllers;

public record OrderItem:BaseEntity<Guid>
{
    public Book Book { get; set; }
    public Guid BookId { get; set; }
    public double Price { get; set; }
    public Order Order { get; set; }
    public Guid OrderId { get; set; }
    public int Count { get; set; }
}