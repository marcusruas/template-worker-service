global using static WorkerTemplate.Infrastructure.DependencyInjection;
using WorkerTemplate.Worker.Workers;
using MassTransit;
using WorkerTemplate.SharedKernel.Common.Entities;
using WorkerTemplate.SharedKernel.Handlers.Workers;
using WorkerTemplate.Worker.Consumers;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddInfrastructure();

        services.AddMassTransit(reg =>
        {
            InjectEnabledQueueConsumers(services, reg, hostContext);

            reg.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(hostContext.Configuration.GetConnectionString("RabbitMQ"));
                cfg.ConfigureEndpoints(context);
            });
        });

        InjectEnabledHostedServices(services, hostContext);
    })
    .Build();

await host.RunAsync();

static void InjectEnabledHostedServices(IServiceCollection services, HostBuilderContext context)
{
    var types = Assembly.GetExecutingAssembly().GetExportedTypes();

    foreach (var type in types.Where(x => x.BaseType == typeof(WorkerProcess)))
    {
        var schedule = context.Configuration.GetSection($"Schedules:{type.Name}").Get<WorkerSchedule>();

        if (schedule.Enabled)
            services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IHostedService), type));
    }
}

static void InjectEnabledQueueConsumers(IServiceCollection services, IBusRegistrationConfigurator registrator, HostBuilderContext context)
{
    var types = Assembly.GetExecutingAssembly().GetExportedTypes();

    foreach (var type in types.Where(x => x.GetInterfaces().Contains(typeof(IConsumer))))
    {
        var schedule = context.Configuration.GetSection($"Schedules:{type.Name}").Get<WorkerSchedule>();

        if (schedule.Enabled)
            registrator.AddConsumer(type);
    }
}