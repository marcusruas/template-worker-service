using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace WorkerTemplate.SharedKernel.Handlers.Repositories
{
    public abstract class StandardNoSqlRepository : IRepository
    {
        public StandardNoSqlRepository(IConfiguration configuration, string nomeBanco)
        {
            _configuration = configuration;

            var connectionString = ObterConnectionString(nomeBanco);
            _client = new MongoClient(connectionString);
            Database = _client.GetDatabase(nomeBanco);
        }

        protected readonly IMongoDatabase Database;

        private readonly IConfiguration _configuration;
        private readonly IMongoClient _client;

        private string ObterConnectionString(string nomeBanco)
        {
            var connectionString = _configuration.GetConnectionString(nomeBanco);

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new Exception(KernelMessages.ConnectionStringNotFound);

            return connectionString;
        }
    }
}