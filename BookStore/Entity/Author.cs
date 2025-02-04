using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace black_follow.Entity;

public record Author : BaseEntity<Guid>
{
    public Author()
    {
    }

    public string Name { get; set; }
    public string? Image { get; set; }
    public List<Book> Books { get; init; } 


}