using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Chronicle.Managers;
using NSubstitute;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Shouldly;
using Xunit;

namespace Chronicle.Tests.Managers
{
    public class SagaSeekerTests
    {
        [Fact]
        public void Seek_Returns_Saga_Actions_Distinc_By_Type()
        {
            _serviceProvider
                .GetService<IEnumerable<ISagaAction<Message1>>>()
                .Returns(new[] { new CustomSaga() });

            _serviceProvider
                .GetService<IEnumerable<ISagaStartAction<Message1>>>()
                .Returns(new[] { new CustomSaga() });

            var result = _sagaSeeker.Seek<Message1>();

            result.ToList().Count().ShouldBe(1);
            result.First().ShouldBeOfType<CustomSaga>();
        }

        #region ARRANGE
        private readonly IServiceProvider _serviceProvider;
        private readonly ISagaSeeker _sagaSeeker;

        class Message1 { }
        class CustomSagaData { }

        class CustomSaga :
            Saga<CustomSaga>,
            ISagaStartAction<Message1>
        {
            public Task CompensateAsync(Message1 message, ISagaContext context)
            {
                throw new NotImplementedException();
            }
            
            public Task HandleAsync(Message1 message, ISagaContext context)
            {
                throw new NotImplementedException();
            }
        }


        public SagaSeekerTests()
        {
            _serviceProvider = Substitute.For<IServiceProvider>();
            _sagaSeeker = new SagaSeeker(_serviceProvider);
        }
        #endregion
    }
}
