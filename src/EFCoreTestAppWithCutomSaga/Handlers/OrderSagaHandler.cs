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
        /*
            NOTE: Remove ISagaUnitOfWork if the Chronicles Internal EFCore/SQL Server implementation is being used.
            If implementing with a custom Solution enable it.
        */

        private readonly ISagaCoordinator _coordinator;
        private ISagaUnitOfWork SagaUnitOfWork { get; }

        public OrderSagaHandler(ISagaCoordinator coordinator, ISagaUnitOfWork _sagaUnitOfWork)
        {
            _coordinator = coordinator;
            SagaUnitOfWork = _sagaUnitOfWork;
        }

        public async Task<Unit> Handle(CreateOrder command, CancellationToken cancellationToken)
        {
            await _coordinator.ProcessAsync(command, SagaContext.Empty);
            // once every thing is processed in the handler, commit changes to DB.
            // This can be moved else where depending on the user needs.
            await SagaUnitOfWork.CommitAsync();
            return Unit.Value;
        }

        public async Task Handle(OrderCreated notification, CancellationToken cancellationToken)
        {
            IEnumerable<ISagaContextMetadata> metadata = new List<ISagaContextMetadata>();
            await _coordinator.ProcessAsync(notification, 
                SagaContext.Create((SagaId)notification.OrderId.ToString(), notification.GetType().Name, metadata));
            // once every thing is processed in the handler, commit changes to DB.
            // This can be moved else where depending on the user needs.
            await SagaUnitOfWork.CommitAsync();
        }
    }
}
