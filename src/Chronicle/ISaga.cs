using System;

namespace Chronicle
{
    public interface ISaga
    {
        Guid Id { get; }
        SagaStates State { get; }
        void Complete();
        void Reject();
        void Initialize(Guid id, SagaStates state);
        Guid ResolveId(object message, ISagaContext context);
    }

    public interface ISaga<TData> : ISaga where TData : class
    {
        TData Data { get; }
        void Initialize(Guid id, SagaStates state, TData data);
    }
}
