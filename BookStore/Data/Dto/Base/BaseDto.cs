namespace BookStore.Data.Dto.Base;

public class BaseDto<T>
{
    public T Id { get; set; }
    public DateTime? CreateionDate  { get; set; }
}