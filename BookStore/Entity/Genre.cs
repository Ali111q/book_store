using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace black_follow.Entity;

public record Genre : BaseEntity<Guid>
{
    public string Name { get; set; }
    public string? Color { get; set; }
    public List<Book> Books { get; init; } = new();
}