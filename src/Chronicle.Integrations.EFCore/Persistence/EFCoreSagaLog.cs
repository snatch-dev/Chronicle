using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Chronicle.Integrations.EFCore.Repositories;

namespace Chronicle.Integrations.EFCore.Persistence
{
    internal class EFCoreSagaLog : ISagaLog
    {
        private readonly ISagaLogRepository SagaLogRepository;

        public EFCoreSagaLog(ISagaLogRepository _sagaLogRepository)
        {
            SagaLogRepository = _sagaLogRepository ?? throw new ArgumentNullException(nameof(_sagaLogRepository));
        }

        public async Task<IEnumerable<ISagaLogData>> ReadAsync(SagaId id, Type type)
           => await SagaLogRepository.ReadAsync(id, type);

        public async Task WriteAsync(ISagaLogData message)
        {
            await SagaLogRepository
                .WriteAsync(new EFCoreSagaLogData(message.Id.Id, message.Type.ToString(), message.CreatedAt, JsonConvert.SerializeObject(message.Message)));
        }
    }

}
