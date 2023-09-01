using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace CandleApi.Models
{
    public class Item
    {
        [Key]
        public Guid ItemId { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Required]
        public string? ImageUrl { get; set; }


        // Navigation properties
        // An Item can appear in multiple order items
        public required ICollection<OrderItem> OrderItems { get; set; }
    }
}