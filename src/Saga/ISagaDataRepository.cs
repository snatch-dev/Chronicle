using System;
using System.Threading.Tasks;

namespace Saga
{
    public interface ISagaDataRepository<TData> where TData : class, ISagaData
    {
        Task<TData> ReadAsync(Guid id);
        Task WriteAsync(TData state);
    }
}
