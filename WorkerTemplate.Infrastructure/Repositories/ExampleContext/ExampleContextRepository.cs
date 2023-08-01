using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WorkerTemplate.SharedKernel.Handlers.Repositories;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace WorkerTemplate.Infrastructure.Repositories.ExampleContext
{
    public class ExampleContextRepository : StandardSqlRepository, IExampleContextRepository
    {
        public ExampleContextRepository(IConfiguration configuration, ILogger<StandardSqlRepository> logger) : base(configuration, logger)
        {
        }

        public async Task TestConnection()
        {
            var command = GetSQLCommandByFile("selectTestConnection");
            await ExecuteSQLCommand(async connection => await connection.ExecuteAsync(command), "example");
        }
    }
}