using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CandleApi.Models
{
    public class OrderItem
    {
        [Key]
        public Guid OrderItemId { get; set; }

        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public Guid ItemId { get; set; }

        [Required]
        public int Quantity { get; set; }


        // Navigation Properties
        // A unique OrderItem can only be a part of one Order
        [ForeignKey("OrderId")]
        public required Order Order { get; set; }
        // An OrderItem can have only one Item
        [ForeignKey("ItemId")]
        public required Item Item { get; set; }
    }
}