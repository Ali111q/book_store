



public record BaseEntity<TId>
{

    public TId Id { get; init; }
    public DateTime? CreateionDate { get; } = DateTime.UtcNow;
}