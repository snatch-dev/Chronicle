using System;
using System.Threading.Tasks;

namespace Chronicle
{
    public interface ISaga
    {
        Guid Id { get; }
        SagaStates State { get; }
        void Complete();
        Task CompleteAsync();
        void Reject();
        Task RejectAsync();
        void Initialize(Guid id, SagaStates state);
        Guid ResolveId(object message, ISagaContext context);
    }

    public interface ISaga<TData> : ISaga where TData : class
    {
        TData Data { get; }
        void Initialize(Guid id, SagaStates states, TData data);
    }
}
