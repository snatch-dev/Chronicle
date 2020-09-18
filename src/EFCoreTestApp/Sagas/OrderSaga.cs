using System;
using Chronicle;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using EFCoreTestApp.Commands;
using EFCoreTestApp.Events;
using Newtonsoft.Json.Linq;

namespace EFCoreTestApp.Sagas
{
    public class OrderSaga : Saga<CreatingOrderData>,
        ISagaStartAction<CreateOrder>, ISagaAction<OrderCreated>
    {
        private const string SagaHeader = "Saga";
        private readonly ILogger<OrderSaga> _logger;

        public OrderSaga(ILogger<OrderSaga> logger)
        {
            _logger = logger;
        }

        /*
            This  extra function is added to make the Saga & SagaLog repositroies to be ignorant of any data type.
            and since Newtonsoft is being used to cast Data as JObject, therefore a method must be present that
            matches the given parameter and Initialize the data.
            See this line: public object Data => JsonConvert.DeserializeObject(MessagePayload);
            in EFCoreSagaStateData file.

            See this line in Chronicle repositry:
            file name: src/Chronicle/Utils/SagaExtensions.cs
            line number: 16
            saga.GetType().GetMethod(method, args.Select(arg => arg.GetType()).ToArray())?.Invoke(saga, args);
        */

        public void Initialize(SagaId id, SagaStates state, JObject data)
        {
            base.Initialize(id, state);
            Data = data.ToObject<CreatingOrderData>();
        }

        public override SagaId ResolveId(object message, ISagaContext context)
        => message switch
        {
            CreateOrder m => (SagaId)m.OrderId.ToString(),
            OrderCreated m => (SagaId)m.OrderId.ToString(),
            _ => base.ResolveId(message, context)
        };

        public async Task HandleAsync(CreateOrder message, ISagaContext context)
        {
            _logger.LogInformation($"[CreateOrder] Started a saga for order: '{message.OrderId}'.");
            Data.ParcelId = message.ParcelId;
            Data.OrderId = message.OrderId;
            Data.CustomerId = message.CustomerId;
        }

        public Task CompensateAsync(CreateOrder message, ISagaContext context)
            => Task.CompletedTask;

        public async Task HandleAsync(OrderCreated message, ISagaContext context)
        {
            _logger.LogInformation($"[OrderCreated] Event for order: '{message.OrderId}'.");
            Data.OrderId = message.OrderId;
            await CompleteAsync();
        }

        public Task CompensateAsync(OrderCreated message, ISagaContext context)
            => Task.CompletedTask;
    }
}
