global using static WorkerTemplate.Infrastructure.DependencyInjection;
using WorkerTemplate.Workers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddInfrastructure();

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
