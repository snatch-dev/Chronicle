using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chronicle;
using Newtonsoft.Json;
using EFCoreTestApp.Persistence;

namespace EFCoreTestApp.SagaRepository
{
    public class EFCoreSagaLog: ISagaLog
    {
        public ISagaUnitOfWork SagaUnitOfWork { get; }

        public EFCoreSagaLog(ISagaUnitOfWork _sagaUnitOfWork)
        {
            SagaUnitOfWork = _sagaUnitOfWork ?? throw new ArgumentNullException(nameof(_sagaUnitOfWork));
        }

        public async Task<IEnumerable<ISagaLogData>> ReadAsync(SagaId id, Type type)
           => await SagaUnitOfWork.SagaLogRepository
               .ReadAsync(id, type);

        public async Task WriteAsync(ISagaLogData message)
        {
            await SagaUnitOfWork.SagaLogRepository
                .WriteAsync(new EFCoreSagaLogData(message.Id.Id, message.Type.ToString(), message.CreatedAt, JsonConvert.SerializeObject(message.Message)));
        }
    }
}
