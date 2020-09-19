using System;

namespace EFCoreTestApp.Sagas
{
    public class CreatingOrderData
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ParcelId { get; set; }
    }
}
