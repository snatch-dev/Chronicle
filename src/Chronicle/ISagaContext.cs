using System;
using System.Collections.Generic;

namespace Chronicle
{
  public interface ISagaContext
  {
    SagaId SagaId { get; }
    string Originator { get; }
    IReadOnlyCollection<ISagaContextMetadata> Metadata { get; }
    ISagaContextMetadata GetMetadata(string key);
    bool TryGetMetadata(string key, out ISagaContextMetadata metadata);
    SagaContextError SagaContextError { get; set; }
  }
}
