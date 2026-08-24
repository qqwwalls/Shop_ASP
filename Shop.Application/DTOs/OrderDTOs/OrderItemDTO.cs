using System.ComponentModel.DataAnnotations;

namespace Shop.Application.DTOs.OrderDTOs
{
    public class OrderItemDTO
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Count must be at least 1")]
        public int Count { get; set; }
    }
}
