using System;

namespace Chronicle.Sagas
{
    internal interface ISaga
    {
        Guid Id { get; }
        SagaStates State { get; }
        void Complete();
        void Reject();
    }

    internal interface ISaga<TData> : ISaga where TData : class
    {
        TData Data { get; }
        void Initialize(Guid id, SagaStates state, TData data);
    }
}
