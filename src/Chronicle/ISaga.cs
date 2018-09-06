using System;

namespace Chronicle
{
    public interface ISaga
    {
        Guid Id { get; }
        SagaStates State { get; }
        void Complete();
        void Reject();
    }

    public interface ISaga<TData> : ISaga where TData : class
    {
        TData Data { get; }
        void Initialize(Guid id, SagaStates state, TData data);
    }
}
