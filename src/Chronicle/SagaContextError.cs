using System;

namespace Chronicle
{
  public class SagaContextError
  {
    public SagaContextError(Exception e)
    {
      Exception = e;
    }

    public Exception Exception { get; }
  }
}
