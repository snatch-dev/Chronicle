using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Linq;

namespace Chronicle.Integrations.EFCore.Persistence
{
    internal class EFCoreSagaStateData : ISagaState
    {
        public string SagaId { get; set; }
        [NotMapped]
        public SagaId Id
        {
            get
            {
                var currId = (SagaId)SagaId.ToString();
                return currId;
            }
        }

        public string SagaType { get; set; }
        [NotMapped]
        Type ISagaState.Type => Assembly.GetEntryAssembly()?.GetType(SagaType);

        public SagaStates State { get; private set; }

        // public object Data => JsonConvert.DeserializeObject(MessagePayload);
        public object Data
        {
            get
            {
                var currType = Assembly.GetEntryAssembly()?.GetType(SagaType);
                var sagaInterface = currType.GetInterfaces()
                                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISaga<>));
                var sagaGenericDataType = sagaInterface.GetGenericArguments().FirstOrDefault();
                var currPayLoad = JsonConvert.DeserializeObject(MessagePayload, sagaGenericDataType);
                return currPayLoad;
            }
        }

        public string MessagePayload { get; set; }

        public Type DataType { get; }

        public EFCoreSagaStateData(string sagaId, string sagaType, SagaStates state, string messagePayload)
        {
            SagaId = sagaId;
            SagaType = sagaType;
            State = state;
            MessagePayload = messagePayload;
        }

        public void Update(SagaStates state, object data = null)
        {
            State = state;
            MessagePayload = JsonConvert.SerializeObject(data);
        }

    }
}
