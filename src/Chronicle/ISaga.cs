using System;
using System.Threading.Tasks;

namespace Chronicle
{
    public interface ISaga
    {
        SagaId Id { get; }
        SagaStates State { get; }
        void Complete();
        Task CompleteAsync();
        void Reject(Exception innerException = null);
        Task RejectAsync(Exception innerException = null);
        void Initialize(SagaId id, SagaStates state);
        SagaId ResolveId(object message, ISagaContext context);
    }

    public interface ISaga<TData> : ISaga where TData : class
    {
        TData Data { get; }
        void Initialize(SagaId id, SagaStates states, TData data);
    }
}
