using System;
using System.Threading.Tasks;

namespace Chronicle
{
    public interface ISagaCoordinator
    {
        Task ProcessAsync<TMessage>(TMessage message, ISagaContext context = null) where TMessage : class;
        Task ProcessAsync<TMessage>(Guid id, TMessage message, ISagaContext context = null) where TMessage : class;
    }
}
