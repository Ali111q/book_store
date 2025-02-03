namespace BookStore.Data.Dto.Base;

public class BaseFilter
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}