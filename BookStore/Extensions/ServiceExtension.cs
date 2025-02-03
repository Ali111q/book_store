using black_follow.Entity;
using BookStore.Helper;
using BookStore.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Extensions;

static public class ServiceExtension
{

    public static IServiceCollection AddCustomScopes(this IServiceCollection services)
    {
        
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        services.AddAutoMapper(typeof(MappinProfile).Assembly);

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}