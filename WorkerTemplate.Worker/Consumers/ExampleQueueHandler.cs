using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MassTransit;
using WorkerTemplate.QueueContracts;
using WorkerTemplate.SharedKernel.Handlers.Workers;

namespace WorkerTemplate.Worker.Consumers
{
    public class ExampleQueueHandler : QueueConsumer<ExampleContract>
    {
        public ExampleQueueHandler(ILogger<ExampleQueueHandler> logger, IBus bus, IConfiguration configuration, IServiceProvider services)
        : base(logger, bus, configuration, services) { }

        public override Task ProcessMessage(ExampleContract message)
        {
            Console.WriteLine($"Message received: {message.Value}");
            return Task.CompletedTask;
        }
    }
}