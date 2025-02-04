using BookStore.Data.Dto.Base;

namespace BookStore.Data.Dto.Book;

public class BookFilter:BaseFilter
{
    public string? Search { get; set; }
    public Guid? GenreId { get; set; }
    public Guid AuthorId { get; set; }
    public bool IsAvailable { get; set; } = true;
}