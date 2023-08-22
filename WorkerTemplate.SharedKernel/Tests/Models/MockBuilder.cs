using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkerTemplate.SharedKernel.Tests.Models
{
    public class MockBuilder<T> where T : class
    {
        protected readonly Mock<T> _mock;

        public MockBuilder()
        {
            _mock = new Mock<T>(MockBehavior.Strict);
        }

        public T Build()
            => _mock.Object;
    }
}
