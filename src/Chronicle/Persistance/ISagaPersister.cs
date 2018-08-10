using System;
using System.Threading.Tasks;

namespace Chronicle.Persistance
{
    internal interface ISagaPersister
    {
        Task ReadAsync(Guid id);
        Task WriteAsync();
        Task CompleteAsync();
        Task RejectAsync();
    }

    internal interface ISagaPersister<TData> : ISagaPersister where TData : class, ISagaData
    {
        void SetPersister(ISagaDataRepository<TData> repository);

    }
}
