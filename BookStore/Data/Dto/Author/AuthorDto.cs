using BookStore.Data.Dto.Author;
using BookStore.Data.Dto.Base;

class AuthorDto:BaseDto<Guid>
{
    public string Name { get; set; }
    public string? Image { get; set; }
    public List<BookAuthorDto> Books { get; set; }
}