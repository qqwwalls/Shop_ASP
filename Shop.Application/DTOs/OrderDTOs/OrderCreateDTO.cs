using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shop.Application.DTOs.OrderDTOs
{
    public class OrderCreateDTO
    {
        [Required]
        public Guid UserId { get; set; }

        public string Status { get; set; } = "New";

        public bool Paid { get; set; } = false;

        [Required]
        [MinLength(1, ErrorMessage = "Order must contain at least one product")]
        public List<OrderItemDTO> Items { get; set; } = new List<OrderItemDTO>();
    }
}
