using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Logging;
using Moq;
using WorkerTemplate.SharedKernel.Tests.Models;

namespace WorkerTemplate.SharedKernel.Tests.Mocks.ServiceMocks
{
    public class ILoggerMock<T> : MockBuilder<ILogger<T>>
    {
        public ILoggerMock()
        {
            _mock.Setup(x => x.LogInformation(It.IsAny<string>(), It.IsAny<object?>())).Verifiable();
            _mock.Setup(x => x.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object?>())).Verifiable();
            _mock.Setup(x => x.LogWarning(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object?>())).Verifiable();
        }
    }
}