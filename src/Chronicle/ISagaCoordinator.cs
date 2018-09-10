using System;
using System.Threading.Tasks;

namespace Chronicle
{
    public interface ISagaCoordinator
    {
        Task ProcessAsync<TMessage>(Guid id, TMessage message) where TMessage : class;
    }
}
