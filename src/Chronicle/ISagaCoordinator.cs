using System;
using System.Threading.Tasks;

namespace Chronicle
{
    public interface ISagaCoordinator
    {
        Task ProcessAsync<TMessage>(TMessage message, ISagaContext context = null) where TMessage : class;

        Task ProcessAsync<TMessage>(TMessage message, Func<TMessage, ISagaContext, Task> onCompleted = null,
            Func<TMessage, ISagaContext, Task> onRejected = null, ISagaContext context = null) where TMessage : class;       
    }
}
