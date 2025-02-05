using black_follow.Entity;
using BookStore.Data.Dto.Base;

namespace BookStore.Data.Dto.Ad;

public class AdUpdate:BaseUpdate
{
    public Guid? RefId { get; set; }
    public AdType? Type { get; set; }
    public string? AdLink { get; set; }
    public string? Image { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? Priority { get; set; } = 2;
}