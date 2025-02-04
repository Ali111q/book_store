using System.ComponentModel.DataAnnotations;
using BookStore.Controllers;

namespace black_follow.Entity;

public record Book:BaseEntity<Guid>{

    public string Name {get; set;}
    public string Details {get; set;}
    public string? Description {get; set;}
    public string? Image {get; set;}
    public Guid AuthorId {get; set;}
    public Guid GenreId{get; set;}
    public string? Teaser {get; set;}
    public Author? Author { get; init; }
    public Genre? Genre { get; init; }
    public double Price {get; set;}
    public bool IsAvailable {get; set;}

    public List<OrderItem> OrderItems { get; set; }
} 
