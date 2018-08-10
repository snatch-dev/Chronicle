using System;
using System.Threading.Tasks;

namespace Chronicle.Sagas
{
    internal interface ISaga
    {
        SagaStates GetState();
        Task ReadAsync(Guid id);
        Task WriteAsync();
        Task CompleteAsync();
        Task RejectAsync();
    }

    internal interface ISaga<TData> : ISaga where TData : ISagaData
    {
        TData Data { get; }
    }
}
