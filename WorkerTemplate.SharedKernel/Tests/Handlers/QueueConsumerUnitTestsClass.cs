using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkerTemplate.SharedKernel.Common.Entities;
using WorkerTemplate.SharedKernel.Handlers.Workers;
using WorkerTemplate.SharedKernel.Tests.Mocks.ServiceMocks;

namespace WorkerTemplate.SharedKernel.Tests.Handlers
{
    public class QueueConsumerUnitTestsClass<T, C> : UnitTestsClass where T : QueueConsumer<C> where C : class
    {
        public QueueConsumerUnitTestsClass()
        {
            ServiceProviderMock = new();
            IBusMock = new();
            LoggerMock = new();
            IConfigurationMock = new();

            QueueSchedule schedule = new();
            schedule.Enabled = true;

            IConfigurationMock.GetCastedSection(schedule);
        }

        protected IServiceProviderMock ServiceProviderMock;
        protected IBusMock IBusMock;
        protected ILoggerMock<T> LoggerMock;
        protected IConfigurationMock IConfigurationMock;
    }
}