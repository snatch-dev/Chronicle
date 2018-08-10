using System;
using System.Threading.Tasks;

namespace Chronicle.Persistance
{
    internal interface ISagaDataRepository<TData> where TData : class, ISagaData
    {
        Task<TData> ReadAsync(Guid id);
        Task WriteAsync(TData state);
    }
}
