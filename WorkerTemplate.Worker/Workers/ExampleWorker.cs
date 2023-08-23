using MassTransit;
using WorkerTemplate.QueueContracts;
using WorkerTemplate.SharedKernel.Handlers.Workers;
using WorkerTemplate.Worker.Consumers;

namespace WorkerTemplate.Worker.Workers
{
    public class ExampleWorker : WorkerProcess
    {
        public ExampleWorker(IServiceProvider services, IBus bus, ILogger<WorkerProcess> logger, IConfiguration configuration)
        : base(services, bus, logger, configuration) { }

        protected override async Task ExecuteProcess(CancellationToken stoppingToken)
        {
            await ExecuteQueryInDb();
            await ProcessResultFromDb();

            await SendMessage<ExampleQueueHandler, ExampleContract>(new ExampleContract(), "RabbitMQ");
            Logger.LogInformation($"Worker running at: {DateTime.UtcNow}");
        }

        public Task ExecuteQueryInDb()
        {
            return Task.CompletedTask;
        }

        public Task ProcessResultFromDb()
        {
            return Task.CompletedTask;
        }
    }
}