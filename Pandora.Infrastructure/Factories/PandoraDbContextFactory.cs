using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pandora.Infrastructure.Data.Contexts;

namespace Pandora.Infrastructure.Factories;

public class PandoraDbContextFactory : IDesignTimeDbContextFactory<PandoraDbContext>
{
    public PandoraDbContext CreateDbContext(string[] args)
    {
        // Set the base path to the API project directory where appsettings.json is located
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Pandora.API");

        // Build configuration
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)  // Point to the API's directory
            .AddJsonFile("appsettings.json")
            .Build();

        // Get the connection string from the appsettings.json
        var connectionString = configuration.GetConnectionString("PandoraBoxDatabase");

        // Set up DbContextOptions
        var optionsBuilder = new DbContextOptionsBuilder<PandoraDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new PandoraDbContext(optionsBuilder.Options);
    }
}