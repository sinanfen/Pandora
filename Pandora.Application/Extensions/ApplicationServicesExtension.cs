using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Pandora.Application.BusinessRules;
using System.Reflection;

namespace Pandora.Application.Extensions;

public static class ApplicationServicesExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<UserBusinessRules>();
        services.AddScoped<CategoryBusinessRules>();
        services.AddScoped<PersonalVaultBusinessRules>();
        services.AddScoped<PasswordVaultBusinessRules>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }
}