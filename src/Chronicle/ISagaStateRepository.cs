using System;
using System.Threading.Tasks;

namespace Chronicle
{
    public interface ISagaStateRepository
    {
        Task<ISagaState> ReadAsync(SagaId id, Type type);
        Task WriteAsync(ISagaState state);
        Task DeleteAsync(SagaId sagaId, Type sagaType);
    }
}
