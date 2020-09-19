using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Chronicle.Integrations.EFCore.Repositories;

namespace Chronicle.Integrations.EFCore.Persistence
{
    internal class EFCoreSagaState : ISagaStateRepository
    {
        public ISagaStateDBRepository SagaStateDBRepository { get; }

        public EFCoreSagaState(ISagaStateDBRepository _sagaStateDBRepository)
        {
            SagaStateDBRepository = _sagaStateDBRepository ?? throw new ArgumentNullException(nameof(_sagaStateDBRepository));
        }

        public async Task<ISagaState> ReadAsync(SagaId id, Type type)
        {
            var _currSagaState = await SagaStateDBRepository.ReadAsync(id, type);
            return _currSagaState;
        }

        public async Task WriteAsync(ISagaState state)
        {
            await SagaStateDBRepository
                .WriteAsync(new EFCoreSagaStateData(state.Id.Id, state.Type.ToString(), state.State, JsonConvert.SerializeObject(state.Data)));
        }
    }
}
