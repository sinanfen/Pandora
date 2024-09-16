using Microsoft.Extensions.DependencyInjection;
using Pandora.Application.Interfaces;
using Pandora.Application.Services;

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

        return services;
    }
}