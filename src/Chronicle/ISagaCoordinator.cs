using System;
using System.Threading.Tasks;

namespace Chronicle
{
    public interface ISagaCoordinator<TSaga, TData> where TSaga : ISaga<TData> where TData : class, new()
    {
        Task ProcessAsync<TMessage>(Guid id, TMessage message) where TMessage : class;
    }
}
