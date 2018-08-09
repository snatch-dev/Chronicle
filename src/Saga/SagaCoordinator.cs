using Autofac;
using System;
using System.Threading.Tasks;

namespace Saga
{
    public class SagaCoordinator : ISagaCoordinator
    {
        private readonly ILifetimeScope _lifetimeScope;

        public SagaCoordinator(ILifetimeScope lifetimeScope)
            => _lifetimeScope = lifetimeScope;

        public async Task DispatchAsync<TSaga, TMessage>(TMessage message, Guid id)
            where TSaga : ISaga
            where TMessage : class
        {
            var saga = _lifetimeScope.Resolve<TSaga>();
            await saga.ReadAsync(id);
            
            if(saga is ISagaAction<TMessage> sagaAction)
            {
                await sagaAction.HandleAsync(message);
                await saga.WriteAsync();
            }
            else
            {
                throw new InvalidOperationException("Saga does not support given message type");
            }
        }

        public async Task<SagaStates> GetStatusAsync<TSaga>(Guid id) where TSaga : ISaga
        {
            var saga = _lifetimeScope.Resolve<TSaga>();
            await saga.ReadAsync(id);
            return saga.GetState();
        }
    }
}
