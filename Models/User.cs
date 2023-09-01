using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CandleApi.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool IsAdmin { get; set; }


        // Navigation Properties
        // A user can have many Orders
        [JsonIgnore]
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}