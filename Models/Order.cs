using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CandleApi.Models
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }


        // Navigation properties
        // There can be only one User per Order
        [ForeignKey("UserId")]
        public required User User { get; set; }
        // An order can have multiple OrderItems
        public required ICollection<OrderItem> OrderItems { get; set; }
    }
}