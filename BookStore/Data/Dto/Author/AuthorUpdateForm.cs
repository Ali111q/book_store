using BookStore.Data.Dto.Base;

namespace BookStore.Data.Dto.Author;

public class AuthorUpdateForm:BaseUpdate<Guid>
{
    public string? Name {get; set;}
    public string? Image {get; set;}
}