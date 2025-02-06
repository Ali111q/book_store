



public record BaseEntity<TId>
{

    public TId Id { get; init; }
    public DateTime? CreateionDate { get; init; } = DateTime.UtcNow;
}