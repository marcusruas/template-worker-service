using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace WorkerTemplate.SharedKernel.Handlers.Repositories
{
    public abstract class StandardSqlRepository
    {
        public StandardSqlRepository(IConfiguration configuration, ILogger<StandardSqlRepository> logger)
        {
            Logger = logger;
            _configuration = configuration;
            _sqlFolderPath = string.Empty;

            SetSqlFolderPath();
        }

        protected readonly ILogger<StandardSqlRepository> Logger;

        private readonly IConfiguration _configuration;
        private string _sqlFolderPath { get; set; }

        private const string REPOSITORY_LAYER_NAME = "Repositories";
        private const string SQL_FILES_STANDARD_FOLDER = "SQL";

        protected string GetSQLCommandByFile(string fileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_sqlFolderPath))
                    throw new Exception("There is no SQL Folder Path.");

                string nameWithExtension = fileName.EndsWith(".sql") ? fileName : fileName + ".sql";
                string filePath = Path.Combine(_sqlFolderPath, nameWithExtension);
                string fileContent = string.Empty;

                filePath = filePath.Replace("file:\\", "");

                if (!File.Exists(filePath))
                    throw new FileNotFoundException();

                fileContent = string.Join(" ", File.ReadAllLines(filePath));

                if (string.IsNullOrWhiteSpace(fileContent))
                    throw new ArgumentNullException();

                return fileContent;
            }
            catch (ArgumentNullException)
            {
                Logger.LogInformation(string.Format(KernelMessages.SQLFileEmpty, fileName));
            }
            catch (FileNotFoundException)
            {
                Logger.LogInformation(string.Format(KernelMessages.SQLFileNotFound, fileName));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, string.Format(KernelMessages.SQLFileError, fileName));
            }

            return string.Empty;
        }

        protected async Task ExecuteSQLCommand(Func<SqlConnection, Task> sqlOperation, string databaseName)
        {
            var connection = CreateSqlConnection(databaseName);

            try
            {
                connection.Open();
                await sqlOperation(connection);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, KernelMessages.SQLCommandFailed);
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Gets a connection string existing in the application config files with the specified key as the databaseName value. 
        /// </summary>
        protected SqlConnection CreateSqlConnection(string databaseName)
        {
            var connectionString = _configuration.GetConnectionString(databaseName);

            if (string.IsNullOrWhiteSpace(connectionString))
                Logger.LogInformation(KernelMessages.ConnectionStringNotFound);

            return new SqlConnection(connectionString);
        }

        private void SetSqlFolderPath()
        {
            string projectRoot = Path.GetDirectoryName(GetType().Assembly.Location) ?? "";
            var currentNamespaces = GetType().Namespace?.Split('.').ToArray() ?? new string[0];

            var repositoryLayerIndex = Array.IndexOf(currentNamespaces, REPOSITORY_LAYER_NAME) - 1;
            _sqlFolderPath = projectRoot;

            if (currentNamespaces.Length > repositoryLayerIndex + 1)
                for (int index = repositoryLayerIndex + 1; index < currentNamespaces.Length; index++)
                    _sqlFolderPath = Path.Combine(_sqlFolderPath, currentNamespaces[index]);

            _sqlFolderPath = Path.Combine(_sqlFolderPath, SQL_FILES_STANDARD_FOLDER);
            _sqlFolderPath = _sqlFolderPath.Replace("file:\\", "");
        }
    }
}
