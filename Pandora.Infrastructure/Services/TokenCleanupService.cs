using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pandora.Application.Interfaces.Repositories;

namespace Pandora.Infrastructure.Services;

/// <summary>
/// Background service to periodically clean up expired refresh tokens
/// </summary>
public class TokenCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TokenCleanupService> _logger;
    private readonly TimeSpan _cleanupInterval = TimeSpan.FromHours(24); // Run daily

    public TokenCleanupService(IServiceProvider serviceProvider, ILogger<TokenCleanupService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Token cleanup service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var refreshTokenRepository = scope.ServiceProvider.GetRequiredService<IRefreshTokenRepository>();

                _logger.LogInformation("Starting expired token cleanup");
                await refreshTokenRepository.DeleteExpiredTokensAsync(stoppingToken);
                _logger.LogInformation("Expired token cleanup completed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during token cleanup");
            }

            // Wait for the next cleanup interval
            await Task.Delay(_cleanupInterval, stoppingToken);
        }

        _logger.LogInformation("Token cleanup service stopped");
    }
} 