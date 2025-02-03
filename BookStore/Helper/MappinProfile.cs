using AutoMapper;
using black_follow.Entity;
using BookStore.Data.User;

namespace BookStore.Helper;

public class MappinProfile:Profile
{
    public MappinProfile()
    {
        CreateMap<AppUser, UserLoginDto>();
        CreateMap<RegisterForm, AppUser>();
    }
}