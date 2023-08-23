using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MassTransit;
using WorkerTemplate.Infrastructure.Repositories.ExampleContext;
using WorkerTemplate.QueueContracts;
using WorkerTemplate.SharedKernel.Handlers.Workers;

namespace WorkerTemplate.Worker.Consumers
{
    public class ExampleQueueHandler : QueueConsumer<ExampleContract>
    {
        public ExampleQueueHandler(
            ILogger<ExampleQueueHandler> logger,
            IBus bus,
            IConfiguration configuration,
            IServiceProvider services,
            IExampleContextRepository repository
        )
        : base(logger, bus, configuration, services)
        {
            _repository = repository;
        }

        private readonly IExampleContextRepository _repository;

        public override async Task ProcessMessage(ExampleContract message)
        {
            Console.WriteLine($"Message received: {message.Value}");
            await _repository.InsertMessageIntoDB(message.Value ?? "N/A");
        }
    }
}