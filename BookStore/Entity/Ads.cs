namespace black_follow.Entity;

public record Ads:BaseEntity<Guid>
{
    public Guid? RefId { get; set; }
    public AdType Type { get; set; }
    public string? AdLink { get; set; }
    public string? Image { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public int Priority { get; set; } = 2;
}

public enum AdType
{
    NEW_BOOK = 1,
    NEW_AUTHOR=2,
    RANDOM_ADD = 3,
}