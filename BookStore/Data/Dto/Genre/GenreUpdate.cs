using BookStore.Data.Dto.Base;

namespace BookStore.Data.Dto.Genre;

public class GenreUpdate : BaseUpdate
{
    public string? Name { get; set; }
    public string? Color { get; set; }
}