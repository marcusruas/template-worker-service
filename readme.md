# Worker Template Documentation

This documentation serves as a template for creating worker applications in .NET. The template is structured into different layers, each with a specific purpose, to help you build robust, scalable and maintainable worker applications efficiently.

## Table of Contents

1. [Introduction](#introduction)
2. [Application Architecture](#application-architecture)
   - [Domain Layer](#domain-layer)
   - [Infrastructure Layer](#infrastructure-layer)
   - [SharedKernel Layer](#sharedkernel-layer)
   - [Worker Layer](#worker-layer)
3. [Getting Started](#getting-started)
4. [Usage](#usage)
   - [Workers](#workers)
   - [Queue Consumers](#queue-consumers)
   - [About publishing a message in a consumer's queue](#publishing-messages)

## Introduction

This template provides a structured approach for building worker applications in .NET. The architecture is designed to separate concerns and promote maintainability and scalability, following the S.O.L.I.D principles.

## Application Architecture

The application is divided into several layers, each responsible for specific aspects of the application's functionality.

### Domain Layer

The Domain layer contains the core business logic of the application. It defines the entities, value objects, and business rules that govern the application's behavior.

### Infrastructure Layer

The Infrastructure layer handles external service integration and repository access. It provides implementations for data storage, communication with external APIs, and other infrastructure-related concerns. The repositories in this layer are currently using Dapper to execute the commands.

### SharedKernel Layer

The SharedKernel layer contains abstract classes, utility functions, and application-wide handlers. It promotes code reuse and encapsulates common functionalities across the application.

### Worker Layer

The Worker layer contains the workers responsible for executing background tasks and processing asynchronous jobs. It coordinates with the other layers to perform specific tasks based on the business logic.

## Getting Started

To create a new worker application using this template, follow these steps:

1. Clone this repository: `git clone https://github.com/marcusruas/template-worker-service.git`
2. Navigate to the cloned directory: `cd template-worker-service`
3. Build and run the application: `dotnet run`

## Usage

The Worker layer implements two types of handlers:

### Workers

Workers are jobs that are executed periodically. To create and manage workers:

1. Create a new class in the `WorkerTemplate.Worker.Workers` namespace.
2. Configure the worker's schedule and behavior in the appsettings.json file under the "Schedule" key.

Example of a worker configuration:

```json
{
  "Schedules": {
    "ExampleWorker": {
      "Enabled": true, //If the service is not enabled, it will not be injected.
      "WorkerFrequencyInMinutes": 15, //The service will execute at the specified time in a frequency of x minutes
      "Monday": [
        9 //it will execute every monday at 9am
      ],
      "Tuesday": [10],
      "Wednesday": [11],
      "Thursday": [12],
      "Friday": [13, 14]
    }
  }
}
```

3. Make your class inherit from the `WorkerTemplate.SharedKernel.Handlers.Workers.WorkerProcess` abstract class. This way it will be injected automatically (as long as the "Enabled" property is true in the configs).
4. Implement your logic as needed.

Here is an example of a ready-to-go worker:

```csharp
public class ExampleWorker : WorkerProcess
{
    public ExampleWorker(IServiceProvider services, IBus bus, ILogger<WorkerProcess> logger, IConfiguration configuration)
    : base(services, bus, logger, configuration) { }

    protected override async Task ExecuteProcess(CancellationToken stoppingToken)
    {
        //Implement the worker's logic here.
    }
}
```

### Queue Consumers

Queue Consumers are classes that consume messages from a RabbitMQ messaging queue using MassTransit. To create and manage queue consumers:

1. Create a new class in the `WorkerTemplate.Worker.Consumers` namespace.
2. Configure the consumer's schedule and behavior in the appsettings.json file under the "Schedule" key.

Example of a worker configuration:

```json
{
  "Schedules": {
    "ExampleQueueHandler": {
      "Enabled": true
    }
  }
}
```

3. Make your class inherit from the `WorkerTemplate.SharedKernel.Handlers.Workers.QueueConsumer` abstract class. This way it will be injected automatically (as long as the "Enabled" property is true in the configs).
4. Implement your logic as needed. Once you create the consumer, **a messaging queue will be created automatically by MassTransit with the name of the class**

Example of a ready-to-go consumer:

```csharp
public class ExampleQueueHandler : QueueConsumer<ExampleContract>
{
    public ExampleQueueHandler(ILogger<ExampleQueueHandler> logger, IBus bus, IConfiguration configuration, IServiceProvider services)
    : base(logger, bus, configuration, services) { }

    public override Task ProcessMessage(ExampleContract message)
    {
        //Implement the worker's logic here.
    }
}
```

### About publishing a message in a consumer's queue

In order for a publisher to publish a message, you have two ways:

- You can use the `SendMessage Method` in the `WorkerTemplate.SharedKernel.Handlers.Workers.WorkerProcess` abstract class. This method requires the consumer and the message type.
- You can implement a different publisher inside the WorkerTemplate project, **But you need to use MassTransit**
- You can implement a different publisher in a different project that doesn't have access to the WorkerTemplate assembly, but here are the requirements
  - You must implement it in a .NET Application (It can be an API, a Worker etc)
  - You must implement it using MassTransit
  - The message contract type in the outside application must have the same namespace as the one inside the consumer's application

Example of a console app publisher:

```csharp
namespace RandomNamespace
{
    public class Main
    {
        public async Task Teste()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri("rabbitmq://localhost"), h =>
                {
                    h.Username("username");
                    h.Password("password");
                });
            });

            busControl.Start();


            var uri = new Uri("rabbitmq://{connectionStringg}/ExampleQueueHandler");
            var endPoint = await busControl.GetSendEndpoint(uri);

            var message = new WorkerTemplate.Domain.QueueContracts.Person();
            await endPoint.Send(message);
            Console.WriteLine("Menssage sent: {0}", message.Name);

            busControl.Stop();
        }
    }
}

//Message Contract type
namespace WorkerTemplate.Domain.QueueContracts //This namespace must be the same as the existing message class in the consumer's project
{
    public class Person
    {
        public Person()
        {
            Name = "Random value for demonstration";
        }

        public string? Name { get; set; }
    }
}
```
