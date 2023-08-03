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
        public WorkerProcess(ILogger<WorkerProcess> logger, IConfiguration configuration, IServiceProvider services)
        {
            Services = services;
            Logger = logger;
            WorkerName = GetType().Name;
            WorkerSchedule = configuration.GetSection($"Schedules:{WorkerName}").Get<WorkerSchedule>();
        }

        protected readonly IServiceProvider Services;
        protected readonly ILogger<WorkerProcess> Logger;

        private readonly string WorkerName;
        private WorkerSchedule WorkerSchedule;

        protected sealed override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (!CanRunAtTheMoment())
                    continue;

                try
                {
                    Logger.LogInformation(string.Format(KernelMessages.ProcessStarted, WorkerName, DateTime.UtcNow));
                    await ExecuteProcess(stoppingToken);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, string.Format(KernelMessages.ErrorAtProcess, WorkerName, DateTime.UtcNow));
                }
                finally
                {
                    Logger.LogInformation(string.Format(KernelMessages.ProcessEnded, WorkerName, DateTime.UtcNow));
                }

                await Task.Delay(WorkerSchedule.WorkerFrequencyInMinutes * 60000, stoppingToken);
            }
        }

        protected abstract Task ExecuteProcess(CancellationToken cancellationToken);

        private bool CanRunAtTheMoment()
        {
            if (!WorkerSchedule.Enabled)
                return false;

            switch (DateTime.UtcNow.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return CurrentDateIsInHours(WorkerSchedule.Monday);
                case DayOfWeek.Tuesday:
                    return CurrentDateIsInHours(WorkerSchedule.Tuesday);
                case DayOfWeek.Wednesday:
                    return CurrentDateIsInHours(WorkerSchedule.Wednesday);
                case DayOfWeek.Thursday:
                    return CurrentDateIsInHours(WorkerSchedule.Thursday);
                case DayOfWeek.Friday:
                    return CurrentDateIsInHours(WorkerSchedule.Friday);
                case DayOfWeek.Saturday:
                    return CurrentDateIsInHours(WorkerSchedule.SaturDay);
                case DayOfWeek.Sunday:
                    return CurrentDateIsInHours(WorkerSchedule.Sunday);
            }

            return false;
        }

        private bool CurrentDateIsInHours(int[]? enabledHours)
            => enabledHours != null && enabledHours.Length > 0 && enabledHours.Contains(DateTime.UtcNow.Hour);
    }
}