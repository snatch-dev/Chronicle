using System;

namespace Saga
{
    public interface ISagaData
    {
        Guid Id { get; set; }
        SagaStates State { get; set; }
    }
}
