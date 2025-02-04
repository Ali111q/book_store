using BookStore.Controllers;

namespace black_follow.Entity;

public record Order:BaseEntity<Guid>
{
    public Guid UserId { get; set; }
    public AppUser User  { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.PENDING;
    public List<OrderItem> Items { get; set; }
    public string? Notes { get; set; }

}

public enum OrderStatus
{
    PENDING = 0,
    ACCEPTED = 1,
    DONE = 2,
    REJECTED = 3,
    CANCELLED = 4,
}