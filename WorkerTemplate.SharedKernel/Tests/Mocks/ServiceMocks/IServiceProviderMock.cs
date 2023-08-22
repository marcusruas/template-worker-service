using Microsoft.Extensions.DependencyInjection;
using System;
using WorkerTemplate.SharedKernel.Tests.Models;

namespace WorkerTemplate.SharedKernel.Tests.Mocks.ServiceMocks
{
    public class IServiceProviderMock : MockBuilder<IServiceProvider>
    {
        public void GetServiceMock<T>(T retorno)
            => _mock.Setup(x => x.GetService(typeof(T))).Returns(retorno);

        public void SetScopeToSelf()
        {
            var serviceScopeMock = new IServiceScopeMock();
            serviceScopeMock.GetServiceProvider(Build());

            _mock.Setup(x => x.CreateScope()).Returns(serviceScopeMock.Build());
        }
    }
}
