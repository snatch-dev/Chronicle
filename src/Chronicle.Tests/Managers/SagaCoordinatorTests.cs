using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chronicle.Managers;
using NSubstitute;
using Xunit;

namespace Chronicle.Tests.Managers
{
    public class SagaCoordinatorTests
    {
        [Fact]
        public async Task ProcessAsync_Resolves_Id_When_Its_Not_Given()
        {
            _sagaSeeker.Seek<Message1>().Returns(new [] {_saga});

            await _sagaCoordinator.ProcessAsync(new Message1());

            _saga
                .Received(1)
                .ResolveId(
                    Arg.Is<object>(o => o is Message1), 
                    Arg.Is<ISagaContext>(sc => sc.CorrelationId == Guid.Empty && sc.Originator == string.Empty ));
        }
        
        
        #region ARRANGE
        
        private readonly ISagaLog _sagaLog;
        private readonly ISagaStateRepository _repository;
        private readonly ISagaSeeker _sagaSeeker;
        private readonly CustomSaga _saga;
        private readonly ISagaCoordinator _sagaCoordinator;
        
        public class Message1 { }
        public class CustomSagaData { }

        public class CustomSaga :
            Saga<CustomSagaData>,
            ISagaStartAction<Message1>
        {
            public Task HandleAsync(Message1 message, ISagaContext context)
                => Task.CompletedTask;
            
            public Task CompensateAsync(Message1 message, ISagaContext context)
                => Task.CompletedTask;
        }
        
        public SagaCoordinatorTests()
        {
            _sagaLog = Substitute.For<ISagaLog>();
            _repository = Substitute.For<ISagaStateRepository>();
            _sagaSeeker = Substitute.For<ISagaSeeker>();
            _saga = Substitute.For<CustomSaga>();
            _sagaCoordinator = new SagaCoordinator(_sagaLog, _repository, _sagaSeeker);
        }
        #endregion
    }
}
