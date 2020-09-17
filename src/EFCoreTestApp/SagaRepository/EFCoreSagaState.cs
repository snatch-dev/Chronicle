using Chronicle;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCoreTestApp.Persistence;

namespace EFCoreTestApp.SagaRepository
{
    public class EFCoreSagaState : ISagaStateRepository
    {

        public ISagaUnitOfWork SagaUnitOfWork { get; }

        public EFCoreSagaState(ISagaUnitOfWork _sagaUnitOfWork)
        {
            SagaUnitOfWork = _sagaUnitOfWork ?? throw new ArgumentNullException(nameof(_sagaUnitOfWork));
        }

        public async Task<ISagaState> ReadAsync(SagaId id, Type type)
        {
            var _currSagaState = await SagaUnitOfWork
                       .SagaStateDBRepository
                       .ReadAsync(id, type);
            return  _currSagaState;
        }

        public async Task WriteAsync(ISagaState state)
        {
            await SagaUnitOfWork
                .SagaStateDBRepository
                .WriteAsync(new EFCoreSagaStateData(state.Id.Id, state.Type.ToString(), state.State, JsonConvert.SerializeObject(state.Data)));
        }
    }
}
