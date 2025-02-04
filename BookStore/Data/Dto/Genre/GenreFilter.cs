using BookStore.Data.Dto.Base;

namespace BookStore.Data.Dto.Genre;

public class GenreFilter:BaseFilter
{
    public string? Name { get; set; }
}