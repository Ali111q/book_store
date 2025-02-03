using BookStore.Data.Dto.Base;

namespace BookStore.Data.Dto.Author;

public class AuthorFilter:BaseFilter
{
    public string? Name { get; set; }
}