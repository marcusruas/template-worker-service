using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WorkerTemplate.SharedKernel.Common.Entities;

namespace WorkerTemplate.SharedKernel.Handlers.Workers
{
    public abstract class WorkerProcess : BackgroundService
    {
        public WorkerProcess(IServiceProvider services, IBus bus, ILogger<WorkerProcess> logger, IConfiguration configuration)
        {
            Services = services;
            Logger = logger;
            BusControl = bus;
            Configuration = configuration;
            WorkerName = GetType().Name;
            WorkerSchedule = Configuration.GetSection($"Schedules:{WorkerName}").Get<WorkerSchedule>();
        }

        protected readonly IServiceProvider Services;
        protected readonly ILogger<WorkerProcess> Logger;

        private readonly IBus BusControl;
        private readonly IConfiguration Configuration;
        private readonly WorkerSchedule WorkerSchedule;
        private readonly string WorkerName;

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

        protected async Task SendMessage<C, T>(T message, string messageQueueName) where C : IConsumer<T> where T : class
        {
            var connectionString = Configuration.GetConnectionString(messageQueueName);

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                Logger.LogError(string.Format(KernelMessages.ConnectionStringNotFound, WorkerName, DateTime.UtcNow));
                throw new ArgumentException(KernelMessages.ConnectionStringNotFound);
            }

            try
            {
                var endpointUri = new Uri($"{connectionString}/{typeof(C).Name}");
                var endpoint = await BusControl.GetSendEndpoint(endpointUri);

                await endpoint.Send(message);
            }
            catch (Exception ex)
            {
                var serializedMessage = JsonConvert.SerializeObject(message);
                Logger.LogError(ex, string.Format(KernelMessages.FailedToSendMessage, typeof(T).Name, DateTime.UtcNow, serializedMessage));
            }
        }

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
                default:
                    return true;
            }
        }

        private bool CurrentDateIsInHours(int[]? enabledHours)
            => enabledHours != null && enabledHours.Length > 0 && enabledHours.Contains(DateTime.UtcNow.Hour);
    }
}