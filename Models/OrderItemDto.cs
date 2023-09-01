using System;
namespace CandleApi.Models
{
	public class OrderItemDto
	{
        public Guid OrderItemId { get; set; }
        public int Quantity { get; set; }
        public ItemDto Item { get; set; }
    }
}

