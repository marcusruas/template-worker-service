using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkerTemplate.SharedKernel.Common.Entities;
using WorkerTemplate.SharedKernel.Handlers.Workers;
using WorkerTemplate.SharedKernel.Tests.Mocks.ServiceMocks;

namespace WorkerTemplate.SharedKernel.Tests.Handlers
{
    public class WorkerProcessUnitTestsClass<T> : UnitTestsClass where T : WorkerProcess
    {
        public WorkerProcessUnitTestsClass()
        {
            ServiceProviderMock = new();
            IBusMock = new();
            LoggerMock = new();
            IConfigurationMock = new();

            var schedule = new WorkerSchedule(true, 1000000);

            for (int i = 1; i <= 24; i++)
            {
                if (schedule.Monday != null)
                    schedule.Monday[i - 1] = i;

                if (schedule.Tuesday != null)
                    schedule.Tuesday[i - 1] = i;

                if (schedule.Wednesday != null)
                    schedule.Wednesday[i - 1] = i;

                if (schedule.Thursday != null)
                    schedule.Thursday[i - 1] = i;

                if (schedule.Friday != null)
                    schedule.Friday[i - 1] = i;

                if (schedule.SaturDay != null)
                    schedule.SaturDay[i - 1] = i;

                if (schedule.Sunday != null)
                    schedule.Sunday[i - 1] = i;
            }

            IConfigurationMock.GetCastedSection(schedule);
        }

        protected IServiceProviderMock ServiceProviderMock;
        protected IBusMock IBusMock;
        protected ILoggerMock<T> LoggerMock;
        protected IConfigurationMock IConfigurationMock;
    }
}