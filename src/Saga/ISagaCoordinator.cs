using System;
using System.Threading.Tasks;

namespace Saga
{
    public interface ISagaCoordinator
    {
        Task DispatchAsync<TSaga,TMessage>(TMessage message, Guid id) where TMessage : class where TSaga : ISaga;
        Task<SagaStates> GetStatusAsync<TSaga>(Guid id) where TSaga : ISaga;
    }
}
