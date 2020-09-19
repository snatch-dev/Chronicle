using System;
using System.ComponentModel.DataAnnotations;

namespace EFCoreTestApp.DTO
{
    public class OrderCreatedDTO
    {
        [Required]
        public Guid OrderId { get; set; }
    }
}
