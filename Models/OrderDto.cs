using System;
namespace CandleApi.Models
{
	public class OrderDto
	{
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        //public string? CustomerName { get; set; }
        //public List<OrderItemDto> OrderItems { get; set; }
    }
}

