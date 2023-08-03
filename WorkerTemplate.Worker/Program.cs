global using static WorkerTemplate.Infrastructure.DependencyInjection;
using WorkerTemplate.Worker.Workers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddInfrastructure();
        services.AddHostedService<ExampleWorker>();
    })
    .Build();

await host.RunAsync();
