using System;
using System.Threading.Tasks;

namespace Chronicle
{
    public interface ISagaDataRepository
    {
        Task<ISagaData> ReadAsync(Guid sagaId, Type sagaType);
        Task WriteAsync(ISagaData sagaData);
    }
}
