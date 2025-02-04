using BookStore.Data.Dto.Base;

namespace BookStore.Data.Dto.Author;

public class AuthorUpdate:BaseUpdate
{
    public string? Name {get; set;}
    public string? Image {get; set;}
}