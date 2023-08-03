using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WorkerTemplate.SharedKernel.Common.Entities;
using System.Text;
using MassTransit;
using Newtonsoft.Json;

namespace WorkerTemplate.SharedKernel.Handlers
{
    public abstract class QueueConsumer<T> : IConsumer<T> where T : class
    {
        public QueueConsumer(ILogger<WorkerProcess> logger, IConfiguration configuration, IServiceProvider services)
        {
            Services = services;
            Logger = logger;
            QueueName = GetType().Name;
            QueueSchedule = configuration.GetSection($"Schedules:{QueueName}").Get<QueueSchedule>();

        }

        protected readonly IServiceProvider Services;
        protected readonly ILogger<WorkerProcess> Logger;

        private readonly string QueueName;
        private readonly QueueSchedule QueueSchedule;

        public async Task Consume(ConsumeContext<T> context)
        {
            try
            {
                Logger.LogInformation(string.Format(KernelMessages.MessageReceived, QueueName, context.MessageId, context.SourceAddress, DateTime.UtcNow));
                await ProcessMessage(context.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, string.Format(KernelMessages.ErrorInMessage, QueueName, DateTime.UtcNow, JsonConvert.SerializeObject(context)));
            }
        }

        public abstract Task ProcessMessage(T message);
    }
}