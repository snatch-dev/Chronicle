using System.Threading.Tasks;
using Chronicle.Persistence;

namespace Chronicle.Managers
{
    internal interface ISagaInitializer
    {
        Task<(bool isInitialized, ISagaState state)> TryInitializeAsync<TMessage>(ISaga saga, SagaId id, TMessage _);
    }
}
