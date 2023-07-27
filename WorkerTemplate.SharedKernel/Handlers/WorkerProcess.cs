using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerTemplate.SharedKernel.Handlers
{
    public abstract class WorkerProcess : BackgroundService
    {
        public WorkerProcess(ILogger<WorkerProcess> logger)
        {
            Logger = logger;
            WorkerName = GetType().Name;
        }

        public readonly ILogger<WorkerProcess> Logger;
        public DateTime CurrentDate { get; private set; } = DateTime.UtcNow;

        private readonly string WorkerName;

        protected sealed override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    Logger.LogInformation(string.Format(KernelMessages.ProcessStarted, WorkerName, CurrentDate));
                    await ExecuteProcess(stoppingToken);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, string.Format(KernelMessages.ErrorAtProcess, WorkerName, CurrentDate));
                }
                finally
                {
                    Logger.LogInformation(string.Format(KernelMessages.ProcessEnded, WorkerName, CurrentDate));
                }
            }
        }

        protected abstract Task ExecuteProcess(CancellationToken cancellationToken);
    }
}