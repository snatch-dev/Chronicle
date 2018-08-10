using System;

namespace Chronicle
{
    public interface ISagaData
    {
        Guid Id { get; set; }
        SagaStates State { get; set; }
    }
}
