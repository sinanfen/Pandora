using Microsoft.EntityFrameworkCore;
using Pandora.Core.Domain.Entities;
using Pandora.Infrastructure.Data.Configurations;
using System.Reflection;

// Infrastructure/Data/PandoraDbContext.cs
namespace Pandora.Infrastructure.Data.Contexts;

public class PandoraDbContext : DbContext
{
    public PandoraDbContext(DbContextOptions<PandoraDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<PasswordVault> passwordVaults { get; set; }
    public DbSet<PersonalVault> PersonalVaults { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        try
        {
            // Apply all entity configurations
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new PasswordVaultConfiguration());
            modelBuilder.ApplyConfiguration(new PersonalVaultConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
            modelBuilder.ApplyConfiguration(new EmailVerificationTokenConfiguration());
            base.OnModelCreating(modelBuilder);
        }
        catch (Exception ex)
        {

            throw;
        }
    }
}