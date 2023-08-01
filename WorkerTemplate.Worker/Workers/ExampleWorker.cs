using Microsoft.Extensions.Options;
using WorkerTemplate.Infrastructure.Repositories.ExampleContext;
using WorkerTemplate.SharedKernel.Handlers;

namespace WorkerTemplate.Workers
{
    public class Worker : WorkerProcess
    {
        public Worker(ILogger<WorkerProcess> logger, IConfiguration configuration, IServiceProvider services)
        : base(logger, configuration, services) { }

        protected override async Task ExecuteProcess(CancellationToken stoppingToken)
        {
            Logger.LogInformation($"Worker running at: {DateTime.UtcNow}");
            await Task.Delay(1000, stoppingToken);
        }
    }
}