using BookStore.Data.Dto.Author;
using BookStore.Data.Dto.Base;

public class AuthorDto:BaseDto<Guid>
{
    public string Name { get; set; }
    public string? Image { get; set; }
    
}