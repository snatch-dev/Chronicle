using System;
using System.Threading.Tasks;
using Chronicle.Persistence;

namespace Chronicle
{
    public interface ISagaStateRepository
    {
        Task<ISagaState> ReadAsync(SagaId sagaId, Type sagaType, Type dataType);
        Task WriteAsync(ISagaState state);
        Task DeleteAsync(SagaId sagaId, Type sagaType);
    }
}