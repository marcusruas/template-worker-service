using WorkerTemplate.SharedKernel.Handlers;

namespace WorkerTemplate
{
    public class Worker : WorkerProcess
    {
        public Worker(ILogger<Worker> logger, IConfiguration configuration) : base(logger, configuration) { }

        protected override async Task ExecuteProcess(CancellationToken stoppingToken)
        {
            Logger.LogInformation($"Worker running at: {CurrentDate}");
            await Task.Delay(1000, stoppingToken);
        }
    }
}