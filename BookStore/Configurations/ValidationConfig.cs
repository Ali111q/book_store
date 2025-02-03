using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

public static class ValidationConfig
{
    public static IServiceCollection AddValidationServices(this IServiceCollection services)
    {
        // Register all validators automatically
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }
}