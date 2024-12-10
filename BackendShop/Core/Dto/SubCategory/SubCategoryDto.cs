using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackendShop.Core.Dto.SubCategory
{
    public class SubCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        //public string? Description { get; set; } = string.Empty;
        public IFormFile? ImageSubCategory { get; set; } 
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}
