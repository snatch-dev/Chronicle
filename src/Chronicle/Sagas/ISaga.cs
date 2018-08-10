using System;
using System.Threading.Tasks;

namespace Chronicle.Sagas
{
    internal interface ISaga
    {
        SagaStates GetState();
    }

    internal interface ISaga<TData> : ISaga where TData : ISagaData
    {
        TData Data { get; }
    }
}
