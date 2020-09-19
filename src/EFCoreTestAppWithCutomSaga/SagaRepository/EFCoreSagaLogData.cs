using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Chronicle;
using Newtonsoft.Json;

namespace EFCoreTestApp.SagaRepository
{
    public class EFCoreSagaLogData : ISagaLogData
    {
        public int logId { get; set; }
        public string SagaId { get; set; }
        [NotMapped]
        public SagaId Id => SagaId;
        public string SagaType { get; set; }
        public long CreatedAt { get; set; }
        [NotMapped]
        public object Message => JsonConvert.DeserializeObject(MessagePayload);
        public string MessagePayload { get; set; }

    Type ISagaLogData.Type => Assembly.GetEntryAssembly()?.GetType(SagaType);

        public EFCoreSagaLogData(string sagaId, string sagaType, long createdAt, string messagePayload)
        {
            SagaId = sagaId;
            SagaType = sagaType;
            CreatedAt = createdAt;
            MessagePayload = messagePayload;
        }
    }
}
