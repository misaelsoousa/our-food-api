using OurFood.Api.Services;
using OurFood.Api.Infrastructure;

namespace OurFood.Api.Commands;

public class AutoMigrationService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public AutoMigrationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Só executa se a variável de ambiente estiver definida
        if (Environment.GetEnvironmentVariable("AUTO_MIGRATE_IMAGES") == "true")
        {
            using var scope = _serviceProvider.CreateScope();
            var migrateCommand = scope.ServiceProvider.GetRequiredService<MigrateWwwrootToS3Command>();
            
            try
            {
                await migrateCommand.ExecuteAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro na migração automática: {ex.Message}");
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
