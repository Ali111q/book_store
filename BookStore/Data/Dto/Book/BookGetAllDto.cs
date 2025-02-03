using BookStore.Data.Dto.Base;

namespace black_follow.Entity;

public class BookGetAllDto:BaseDto<Guid>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public double Price { get; set; }
    public string? AuthorName { get; set; }
    public string? Color { get; set; }

    public string? Genre { get; set; }
}