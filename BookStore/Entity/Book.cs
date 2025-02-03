using System.ComponentModel.DataAnnotations;

namespace black_follow.Entity;

public record Book(
    Guid Id,
    DateTime? CreateionDate,
    string Name,
    string Details,
    string? Description,
    string? Image,
    Guid AuthorId,
    Author Author,
    Genre Genre,
    Guid GenreId,
    string? teaser,
    double Price,
    bool IsAvailable
) : BaseEntity<Guid>(Id,CreateionDate);