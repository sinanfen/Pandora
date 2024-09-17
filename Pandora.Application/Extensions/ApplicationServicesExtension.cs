using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Pandora.Application.BusinessRules;
using Pandora.Application.Interfaces;
using Pandora.Application.Security;
using Pandora.Application.Security.Interfaces;
using Pandora.Application.Services;
using System.Reflection;

namespace Pandora.Application.Extensions;

public static class ApplicationServicesExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Application Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IPasswordVaultService, PasswordVaultService>();
        services.AddScoped<IPandoraBoxService, PandoraBoxService>();

        services.AddScoped<IHasher, SecurityService>();
        services.AddScoped<IEncryption, SecurityService>();
        services.AddScoped<UserBusinessRules>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}