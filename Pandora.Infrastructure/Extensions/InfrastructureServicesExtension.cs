using Microsoft.Extensions.DependencyInjection;
using Pandora.Application.Interfaces;
using Pandora.Application.Interfaces.Repositories;
using Pandora.Application.Interfaces.Security;
using Pandora.Application.Security;
using Pandora.Infrastructure.Repositories;
using Pandora.Infrastructure.Services;

namespace Pandora.Infrastructure.Extensions;

public static class InfrastructureServicesExtension
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IPasswordVaultService, PasswordVaultService>();
        services.AddScoped<IPersonalVaultService, PersonalVaultService>();

        // AESService için Key ve IV kontrolü
        var aesKey = AESEnvironmentHelper.GetOrCreateAesKey();
        services.AddSingleton<IHasher>(new SecurityService(aesKey));
        services.AddSingleton<IEncryption>(new SecurityService(aesKey));

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IPasswordVaultRepository, PasswordVaultRepository>();
        services.AddScoped<IPersonalVaultRepository, PersonalVaultRepository>();

        return services;
    }
}