using System.Threading.Tasks;

namespace Chronicle
{
    public interface ISagaAction<in TMessage>
    {
        Task HandleAsync(TMessage message);
        Task CompensateAsync(TMessage message);
    }
}
