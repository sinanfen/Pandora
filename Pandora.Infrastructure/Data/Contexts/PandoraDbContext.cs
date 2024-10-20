using Microsoft.EntityFrameworkCore;
using Pandora.Core.Domain.Entities;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        try
        {
            // Automatically apply all configurations from the current assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
        catch (Exception ex)
        {

            throw;
        }
    }
}