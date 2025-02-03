using black_follow.Entity;
using BookStore.Data.Dto.Base;

namespace BookStore.Data.Dto.Genre;

public class GenreDto:BaseDto<Guid>
{
    public string Name {get; set;}
    public string? Color {get; set;}
}