using System;

namespace Chronicle
{
    public class SagaContextError
    {
        public Exception Exception { get; }

        public SagaContextError(Exception e)
        {
            Exception = e;
        }
    }
}
