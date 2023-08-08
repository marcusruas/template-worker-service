using Microsoft.Extensions.Options;
using WorkerTemplate.Infrastructure.Repositories.ExampleContext;
using WorkerTemplate.SharedKernel.Handlers;
using MassTransit;
using WorkerTemplate.QueueContracts;

namespace WorkerTemplate.Worker.Workers
{
    public class ExampleWorker : WorkerProcess
    {
        public ExampleWorker(IServiceProvider services, IBus bus, ILogger<WorkerProcess> logger, IConfiguration configuration)
        : base(services, bus, logger, configuration) { }

        protected override async Task ExecuteProcess(CancellationToken stoppingToken)
        {
            await SendMessage<ExampleQueueHandler, ExampleContract>(new ExampleContract(), "RabbitMQ");
            Logger.LogInformation($"Worker running at: {DateTime.UtcNow}");
        }
    }
}