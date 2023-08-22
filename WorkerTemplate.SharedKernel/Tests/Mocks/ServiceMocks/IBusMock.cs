using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Moq;
using WorkerTemplate.SharedKernel.Tests.Models;

namespace WorkerTemplate.SharedKernel.Tests.Mocks.ServiceMocks
{
    public class IBusMock : MockBuilder<IBus>
    {
        public void MockSend<T>() where T : class
        {
            var sendEndpointMock = new Mock<ISendEndpoint>();
            sendEndpointMock.Setup(x => x.Send(It.IsAny<T>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mock.Setup(x => x.GetSendEndpoint(It.IsAny<Uri>())).Returns(Task.FromResult(sendEndpointMock.Object));
        }
    }
}