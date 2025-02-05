using black_follow.Entity;
using BookStore.Data.Dto.Base;

namespace BookStore.Data.Dto.Ad;

public class AdFilter:BaseFilter
{
    public string? Search { get; set; }
    public AdType? Type { get; set; }
}