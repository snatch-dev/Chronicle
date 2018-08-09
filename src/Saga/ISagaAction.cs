using System.Threading.Tasks;

namespace Saga
{
    public interface ISagaAction { }

    public interface ISagaAction<in TMessage> : ISagaAction where TMessage : class
    {
        Task HandleAsync(TMessage message);
        Task CompensateAsync(TMessage message);
    }
}
