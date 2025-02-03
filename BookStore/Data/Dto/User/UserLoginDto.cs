using BookStore.Data.Dto.Base;

namespace BookStore.Data.User;

public class UserLoginDto: BaseDto<Guid>
{
    public string Username { get; set; }
    public string Token { get; set; }
}