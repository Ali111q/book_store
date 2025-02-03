using System.ComponentModel.DataAnnotations;

namespace black_follow.Entity;

public record Author(
    Guid Id,
    DateTime? CreateionDate,
    string Name,
    string? Image,
    List<Book> Books
) : BaseEntity<Guid>(Id, CreateionDate);