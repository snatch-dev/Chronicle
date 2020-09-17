using System;
using System.ComponentModel.DataAnnotations;

namespace EFCoreTestApp.DTO
{
    public class CreateOrderDTO
    {
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public Guid ParcelId { get; set; }
    }
}
