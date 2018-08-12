using System;

namespace Chronicle
{
    internal interface ISagaData<TData> where TData : class
    {
        Guid SagaId { get; }
        SagaStates State { get; }
        TData Data { get; }
    }
}
