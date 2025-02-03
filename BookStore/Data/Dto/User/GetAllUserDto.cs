using BookStore.Data.Dto.Base;

namespace BookStore.Data.User;

public class GetAllUserDto:BaseDto<Guid>
{
    public string Username { get; set; }
}