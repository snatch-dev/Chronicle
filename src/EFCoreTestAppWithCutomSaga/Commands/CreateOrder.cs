using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace EFCoreTestApp.Commands
{
    public class CreateOrder: IRequest
    {
        public Guid OrderId { get; }
        public Guid CustomerId { get; }
        public Guid ParcelId { get; }

        public CreateOrder(Guid orderId, Guid customerId, Guid parcelId)
        {
            OrderId = orderId;
            CustomerId = customerId;
            ParcelId = parcelId;
        }
    }
}
