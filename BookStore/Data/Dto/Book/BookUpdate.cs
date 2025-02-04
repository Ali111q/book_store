using BookStore.Data.Dto.Base;

namespace BookStore.Data.Dto.Book;

public class BookUpdate:BaseUpdate
{
    public string? Name {get; set;}
    public string? Details {get; set;}
    public string? Description {get; set;}
    public string? Image {get; set;}
    public string? Teaser {get; set;}
    public double? Price {get; set;}
    public bool? IsAvailable {get; set;}
}