using System.ComponentModel.DataAnnotations;

namespace Shop.Application.DTOs
{
    public class CategoryReadDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int? ParentId { get; set; }
        public List<int> Products { get; set; } = new List<int>();
    }

    public class CategoryCreateDTO
    {
        [System.ComponentModel.DefaultValue("Телефони")]
        public string Name { get; set; } = string.Empty;

        [System.ComponentModel.DefaultValue("phones")]
        public string Slug { get; set; } = string.Empty;

        [System.ComponentModel.DefaultValue("phones")]
        public string Url { get; set; } = string.Empty;

        [System.ComponentModel.DefaultValue(null)]
        public int? ParentId { get; set; } = null;
    }

    public class UpdateCategoryDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Slug is required")]
        [MaxLength(100, ErrorMessage = "Slug cannot exceed 100 characters")]
        public string Slug { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;
        public int? ParentId { get; set; }
        public bool IsActive { get; set; }
    }
}
