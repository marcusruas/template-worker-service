using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using WorkerTemplate.SharedKernel.Tests.Models;

namespace WorkerTemplate.SharedKernel.Tests.Mocks.ServiceMocks
{
    public class IConfigurationMock : MockBuilder<IConfiguration>
    {
        public void MockReturnForKey(string key, string value)
            => _mock.Setup(x => x[key]).Returns(value);

        public void GetCastedSection<T>(T value)
            => _mock.Setup(x => x.GetSection(It.IsAny<string>()).Get<T>()).Returns(value);

        public void GetConnectionString(string value)
            => _mock.Setup(x => x.GetConnectionString(It.IsAny<string>())).Returns(value);

    }
}