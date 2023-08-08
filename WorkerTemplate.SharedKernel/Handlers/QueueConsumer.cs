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
        public QueueConsumer(ILogger<QueueConsumer<T>> logger, IBus bus, IConfiguration configuration, IServiceProvider services)
        {
            Services = services;
            Logger = logger;
            BusControl = bus;
            Configuration = configuration;
            QueueName = GetType().Name;
            QueueSchedule = configuration.GetSection($"Schedules:{QueueName}").Get<QueueSchedule>();

        }

        protected readonly IServiceProvider Services;
        protected readonly ILogger<QueueConsumer<T>> Logger;

        private readonly IBus BusControl;
        private readonly IConfiguration Configuration;
        private readonly string QueueName;
        private readonly QueueSchedule QueueSchedule;

        public async Task Consume(ConsumeContext<T> context)
        {
            if (!QueueSchedule.Enabled)
                return;

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

        protected async Task SendMessage<C, M>(M message, string messageQueueName) where C : IConsumer<M> where M : class
        {
            var connectionString = Configuration.GetConnectionString(messageQueueName);

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                Logger.LogError(string.Format(KernelMessages.ConnectionStringNotFound, QueueName, DateTime.UtcNow));
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
                Logger.LogError(ex, string.Format(KernelMessages.FailedToSendMessage, typeof(M).Name, DateTime.UtcNow, serializedMessage));
            }
        }

        public abstract Task ProcessMessage(T message);
    }
}