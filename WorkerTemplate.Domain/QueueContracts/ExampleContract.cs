using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//The ideal would be to keep the namespace simple, as it needs to be matched by external apps to send a message properly.
//For more information, read https://masstransit.io/documentation/concepts/messages
namespace WorkerTemplate.QueueContracts
{
    //An example of a message to be consumed by the queue consumer
    public class ExampleContract
    {
        public ExampleContract()
        {
            Value = "Obi-Wan Kenobi: Hello there!\nGeneral Grievous: General Kenobi!";
        }

        public string? Value { get; set; }
    }
}