using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using WorkerTemplate.Domain.QueueContracts;
using WorkerTemplate.SharedKernel.Handlers;

namespace WorkerTemplate.Worker.Workers
{
    public class ExampleQueueHandler : QueueConsumer<Person>
    {
        public ExampleQueueHandler(ILogger<WorkerProcess> logger, IConfiguration configuration, IServiceProvider services) : base(logger, configuration, services)
        {
        }

        public override async Task ProcessMessage(Person message)
        {
            using (var scope = Services.CreateScope())
            {
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                using (var connection = new SqlConnection("Server=.;Database=master;User Id=sa;Password=IHeartRainbows44;"))
                {
                    var command = $"INSERT INTO WORKER_LOGS VALUES (@INSTANCE, @MESSAGE, GETDATE())";
                    await connection.ExecuteAsync(command, new
                    {
                        Instance = Environment.GetEnvironmentVariable("INSTANCE_NAME") ?? "N/A",
                        Message = $"Person Received: {message.Name}"
                    });
                }
            }

            await Task.Delay(10000);
        }
    }
}