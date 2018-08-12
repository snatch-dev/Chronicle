using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Chronicle.Sagas;

namespace Chronicle
{
    public interface ISagaCoordinator<TSaga, TData> where TSaga : ISaga<TData> where TData : class, new()
    {
        Task DispatchAsync<TMessage>(Guid id, TMessage message) where TMessage : class;
    }
}
