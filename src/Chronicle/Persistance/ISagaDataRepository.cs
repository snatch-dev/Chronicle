using System;
using System.Threading.Tasks;

namespace Chronicle.Persistance
{
    internal interface ISagaDataRepository<TData> where TData : class
    {
        Task<ISagaData<TData>> ReadAsync(Guid id);
        Task WriteAsync(ISagaData<TData> state);
    }
}
