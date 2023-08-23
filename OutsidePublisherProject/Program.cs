using MassTransit;
using WorkerTemplate.QueueContracts;

var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri("rabbitmq://localhost"), h =>
                {
                    h.Username("worker_service");
                    h.Password("IHeartRainbows44");
                });
            });

busControl.Start();


var uri = new Uri("amqp://worker_service:IHeartRainbows44@localhost:5672/ExampleQueueHandler");
var endPoint = await busControl.GetSendEndpoint(uri);

for (int i = 0; i < 100; i++)
{

    var message = new WorkerTemplate.QueueContracts.ExampleContract();
    await endPoint.Send(message);
    Console.WriteLine("Menssage sent: {0}", message.Value);

}

busControl.Stop();

//Message Contract type
namespace WorkerTemplate.QueueContracts
{
    //An example of a message to be consumed by the queue consumer
    public class ExampleContract
    {
        public ExampleContract()
        {
            Value = $"I am {new Bogus.Faker().Person.FirstName} {new Bogus.Faker().Person.LastName}";
        }

        public string? Value { get; set; }
    }
}