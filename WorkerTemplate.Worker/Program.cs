global using static WorkerTemplate.Infrastructure.DependencyInjection;
using WorkerTemplate.Worker.Workers;
using MassTransit;
using WorkerTemplate.SharedKernel.Common.Entities;
using WorkerTemplate.SharedKernel.Handlers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddInfrastructure();

        services.AddMassTransit(reg =>
        {
            AddQueueConsumerIfEnabled<ExampleQueueConsumer>(reg, hostContext);

            reg.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(hostContext.Configuration.GetConnectionString("RabbitMQ"));
                cfg.ConfigureEndpoints(context);
            });
        });

        AddHostedServiceIfEnabled<ExampleWorker>(services, hostContext);
    })
    .Build();

await host.RunAsync();

static void AddHostedServiceIfEnabled<T>(IServiceCollection services, HostBuilderContext context) where T : WorkerProcess
{
    var workerSchedule = context.Configuration.GetSection($"Schedules:{nameof(T)}").Get<WorkerSchedule>();

    if (workerSchedule.Enabled)
        services.AddHostedService<T>();
}

static void AddQueueConsumerIfEnabled<T>(IBusRegistrationConfigurator registrator, HostBuilderContext context) where T : class, IConsumer
{
    var schedule = context.Configuration.GetSection($"Schedules:{nameof(T)}").Get<QueueSchedule>();

    if (schedule.Enabled)
        registrator.AddConsumer<T>();
}