using System.ComponentModel.DataAnnotations;
using BookStore.Data.Dto.Base;

namespace black_follow.Entity;

public class BookDto:BaseDto<Guid>
{
    
    public string Name { get; set; }
    
    public string Details { get; set; }
    
    public string? Description { get; set; }
    
    public string? Image { get; set; }
    
    public double Price { get; set; }
    
    public bool IsAvailable { get; set; }
    public string? teaser { get; set; }
}