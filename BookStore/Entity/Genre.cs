using System.ComponentModel.DataAnnotations;

namespace black_follow.Entity;

public record Genre(
    Guid Id,
    DateTime? CreateionDate,
    string Name,
    string? Color,
    List<Book> Books
) : BaseEntity<Guid>(Id, CreateionDate);