using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WorkerTemplate.SharedKernel.Common.Entities;

namespace WorkerTemplate.SharedKernel.Handlers
{
    public abstract class WorkerProcess : BackgroundService
    {
        public WorkerProcess(ILogger<WorkerProcess> logger, IConfiguration configuration)
        {
            Logger = logger;
            _workerName = GetType().Name;
            _workerSchedule = configuration.GetSection($"Schedules:{_workerName}").Get<Schedule>();
        }

        public readonly ILogger<WorkerProcess> Logger;
        public DateTime CurrentDate { get; private set; } = DateTime.UtcNow;

        private readonly string _workerName;
        private Schedule _workerSchedule;

        protected sealed override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (!CanRunAtTheMoment())
                    continue;

                try
                {
                    Logger.LogInformation(string.Format(KernelMessages.ProcessStarted, _workerName, CurrentDate));
                    await ExecuteProcess(stoppingToken);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, string.Format(KernelMessages.ErrorAtProcess, _workerName, CurrentDate));
                }
                finally
                {
                    Logger.LogInformation(string.Format(KernelMessages.ProcessEnded, _workerName, CurrentDate));
                }
            }
        }

        protected abstract Task ExecuteProcess(CancellationToken cancellationToken);

        public bool CanRunAtTheMoment()
        {
            if (!_workerSchedule.Enabled)
                return false;

            switch (CurrentDate.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return CurrentDateIsInHours(_workerSchedule.Monday);
                case DayOfWeek.Tuesday:
                    return CurrentDateIsInHours(_workerSchedule.Tuesday);
                case DayOfWeek.Wednesday:
                    return CurrentDateIsInHours(_workerSchedule.Wednesday);
                case DayOfWeek.Thursday:
                    return CurrentDateIsInHours(_workerSchedule.Thursday);
                case DayOfWeek.Friday:
                    return CurrentDateIsInHours(_workerSchedule.Friday);
                case DayOfWeek.Saturday:
                    return CurrentDateIsInHours(_workerSchedule.SaturDay);
                case DayOfWeek.Sunday:
                    return CurrentDateIsInHours(_workerSchedule.Sunday);
            }

            return true;
        }

        private bool CurrentDateIsInHours(int[]? enabledHours)
            => enabledHours != null && enabledHours.Length > 0 && enabledHours.Contains(CurrentDate.Hour);
    }
}