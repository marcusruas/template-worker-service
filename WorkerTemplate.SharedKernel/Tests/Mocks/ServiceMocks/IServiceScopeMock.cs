using System;
using Microsoft.Extensions.DependencyInjection;
using WorkerTemplate.SharedKernel.Tests.Models;


namespace WorkerTemplate.SharedKernel.Tests.Mocks.ServiceMocks
{
    public class IServiceScopeMock : MockBuilder<IServiceScope>
    {
        public void GetServiceProvider(IServiceProvider serviceProvider)
            => _mock.Setup(x => x.ServiceProvider).Returns(serviceProvider);
    }
}