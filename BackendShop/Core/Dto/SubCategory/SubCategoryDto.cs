using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackendShop.Core.Dto.SubCategory
{
    public class SubCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        //public string? Description { get; set; } = string.Empty;
        public string? ImageSubCategory { get; set; } // Шлях до зображення
        public int CategoryId { get; set; }
        //public string? CategoryName { get; set; }
    }
}
