using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chronicle;
using MediatR;
using EFCoreTestApp.Commands;
using EFCoreTestApp.Events;
using EFCoreTestApp.Persistence;

namespace EFCoreTestApp.Handlers
{
    public class OrderSagaHandler: IRequestHandler<CreateOrder>, INotificationHandler<OrderCreated>
    {
        private readonly ISagaCoordinator _coordinator;
        private readonly SagaDbContext _dbContext;

        public OrderSagaHandler(ISagaCoordinator coordinator, SagaDbContext dbContext)
        {
            _coordinator = coordinator;
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(CreateOrder command, CancellationToken cancellationToken)
        {
            // DO WORK HERE
            await _coordinator.ProcessAsync(command, SagaContext.Empty);
            return Unit.Value;
        }

        public async Task Handle(OrderCreated notification, CancellationToken cancellationToken)
        {
            // DO WORK HERE
            IEnumerable<ISagaContextMetadata> metadata = new List<ISagaContextMetadata>();
            await _coordinator.ProcessAsync(notification, 
                SagaContext.Create((SagaId)notification.OrderId.ToString(), notification.GetType().Name, metadata));
        }
    }
}
