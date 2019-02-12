using System;
using System.Threading.Tasks;

namespace Chronicle
{
    public interface ISagaStateRepository
    {
        Task<ISagaState> ReadAsync(Guid id, Type type);
        Task WriteAsync(ISagaState state);
    }
}
