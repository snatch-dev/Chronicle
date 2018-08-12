using System;
using System.Threading.Tasks;

namespace Chronicle.Persistance
{
    internal interface ISagaDataRepository<TData> where TData : class
    {
        Task<ISagaData<TData>> ReadAsync(Guid sagaId);
        Task WriteAsync(ISagaData<TData> sagaData);
    }
}
