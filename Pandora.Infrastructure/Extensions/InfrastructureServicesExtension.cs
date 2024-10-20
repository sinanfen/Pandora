using Microsoft.Extensions.DependencyInjection;
using Pandora.Application.Interfaces.Repositories;
using Pandora.Infrastructure.Repositories;

namespace Pandora.Infrastructure.Extensions;

public static class InfrastructureServicesExtension
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IPasswordVaultRepository, PasswordVaultRepository>();
        services.AddScoped<IPersonalVaultRepository, PersonalVaultRepository>();

        return services;
    }
}