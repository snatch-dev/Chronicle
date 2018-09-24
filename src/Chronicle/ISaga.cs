using System;

namespace Chronicle
{
    public interface ISaga
    {
        Guid Id { get; }
        SagaStates State { get; }
        void Complete();
        void Reject();
        object Data { get; }
        void Initialize(Guid id, SagaStates state, object data);
    }

    public interface ISaga<TData> : ISaga where TData : class
    {
        new TData Data { get; }
        void Initialize(Guid id, SagaStates state, TData data);
    }
}
