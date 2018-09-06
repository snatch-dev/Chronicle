using System;

namespace Chronicle
{
    public interface ISagaData<TData> where TData : class
    {
        Guid SagaId { get; }
        SagaStates State { get; }
        TData Data { get; }
    }
}
