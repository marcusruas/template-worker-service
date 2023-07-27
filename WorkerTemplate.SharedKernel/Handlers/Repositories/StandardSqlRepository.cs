using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace WorkerTemplate.SharedKernel.Handlers.Repositories
{
    public abstract class StandardSqlRepository : IRepository
    {
        public StandardSqlRepository(IConfiguration configuration, ILogger<StandardSqlRepository> logger)
        {
            Logger = logger;
            _configuration = configuration;
            _sqlFolderPath = string.Empty;

            DefinirSqlPath();
        }

        protected readonly ILogger<StandardSqlRepository> Logger;

        private readonly IConfiguration _configuration;
        private string _sqlFolderPath { get; set; }

        private const string REPOSITORY_LAYER_NAME = "Repositories";
        private const string SQL_FILES_STANDARD_FOLDER = "SQL";

        protected string? GetSQLCommand(string nomeArquivo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_sqlFolderPath))
                    return null;

                string nomeCorrigidoArquivo = nomeArquivo.EndsWith(".sql") ? nomeArquivo.Replace(".sql", string.Empty) : nomeArquivo;
                string[] linhas;
                string conteudoArquivo = string.Empty;

                string pathArquivo = Path.Combine(_sqlFolderPath, nomeCorrigidoArquivo);
                pathArquivo = pathArquivo.Replace("file:\\", "");

                if (!File.Exists(pathArquivo))
                {
                    Logger.LogError(string.Format(KernelMessages.SQLFileNotFound, nomeCorrigidoArquivo));
                    return null;
                }

                linhas = File.ReadAllLines(pathArquivo);

                foreach (var linha in linhas)
                    conteudoArquivo += $"{linha} ";

                return conteudoArquivo;
            }
            catch (ArgumentNullException)
            {
                Logger.LogInformation(KernelMessages.SQLFileEmpty);
                return null;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, KernelMessages.SQLFileError);
                return null;
            }
        }

        protected async Task ExecuteSQLCommand(Func<SqlConnection, Task> funcao, string nomeBanco)
        {
            var conexao = ObterConexaoSql(nomeBanco);

            try
            {
                conexao.Open();
                await funcao(conexao);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, KernelMessages.SQLCommandFailed);
            }
            finally
            {
                conexao.Close();
            }

        }

        private SqlConnection ObterConexaoSql(string nomeBanco)
        {
            var connectionString = _configuration.GetConnectionString(nomeBanco);

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new Exception(KernelMessages.ConnectionStringNotFound);

            return new SqlConnection(connectionString);
        }

        private void DefinirSqlPath()
        {
            string projectRoot = Path.GetDirectoryName(GetType().Assembly.Location) ?? "";
            List<string> namespaces = GetType().Namespace?
                .Split(".")
                .Where(ns => ns != REPOSITORY_LAYER_NAME)
                .ToList() ?? new List<string>();

            _sqlFolderPath = Path.Combine(projectRoot, string.Join("\\", namespaces), SQL_FILES_STANDARD_FOLDER);
            _sqlFolderPath = _sqlFolderPath.Replace("file:\\", "");
        }
    }
}
