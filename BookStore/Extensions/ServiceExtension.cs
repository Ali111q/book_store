using black_follow.Entity;
using BookStore.Helper;
using BookStore.Services;
using BookStore.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Extensions;

static public class ServiceExtension
{

    public static IServiceCollection AddCustomScopes(this IServiceCollection services, IConfiguration config)
    {
        
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection")));
        services.AddSingleton<MongoDbDataContext>(sp =>
        {
            return new MongoDbDataContext(Util.MongoDbConnectionString, Util.MongoDbDataBaseName, sp);
        });



// Configure Identity
        services.AddIdentity<AppUser, ApplicationRole>(op =>
            {
                op.Password.RequireDigit = false;
                op.Password.RequiredLength = 8;
                op.Password.RequiredUniqueChars = 0;
                op.Password.RequireLowercase = false;
                op.Password.RequireNonAlphanumeric = false;
                op.Password.RequireUppercase = false;
            })
    
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

        services.AddAutoMapper(typeof(MappinProfile).Assembly);

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<SeedExtension>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddSingleton<IUserConnectionManager, UserConnectionManager>();
        // var serviceProvider = services.BuildServiceProvider();
        // var permissionSeeder = serviceProvider.GetService<SeedExtension>();
        // permissionSeeder.Initialize();
        return services;
    }
}