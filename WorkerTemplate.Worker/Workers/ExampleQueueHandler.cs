using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkerTemplate.SharedKernel.Handlers;

namespace WorkerTemplate.Worker.Workers
{
    public class ExampleQueueHandler : QueueConsumer<Person>
    {
        public ExampleQueueHandler(ILogger<WorkerProcess> logger, IConfiguration configuration, IServiceProvider services) : base(logger, configuration, services)
        {
        }

        public override Task ProcessMessage(Person message)
        {
            Console.WriteLine($"Person received: {message.Name}");
            return Task.CompletedTask;
        }
    }

    public class Person
    {
        public string? Name { get; set; }
    }
}