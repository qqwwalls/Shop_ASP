using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shop.Application.DTOs.OrderDTOs
{
    public class OrderCreateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MinLength(1)]
        public List<OrderItemDTO> Products { get; set; } = new List<OrderItemDTO>();

        [Required]
        public AddressDTO Address { get; set; } = null!;
    }

    public class AddressDTO
    {
        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string Street { get; set; } = string.Empty;
    }
}
