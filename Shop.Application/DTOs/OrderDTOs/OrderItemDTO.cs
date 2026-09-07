using System.ComponentModel.DataAnnotations;

namespace Shop.Application.DTOs.OrderDTOs
{
    public class OrderItemDTO
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Qty { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
