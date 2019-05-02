using System;

namespace Chronicle
{
  public struct SagaId
  {
    public string Id { get; set; }

    public static implicit operator string(SagaId sagaId) => sagaId.Id;

    public static implicit operator SagaId(string sagaId) => new SagaId { Id = sagaId };

    public static SagaId NewSagaId()
    {
      return new SagaId { Id = Guid.NewGuid().ToString() };
    }

    public override string ToString() => Id;
  }
}
