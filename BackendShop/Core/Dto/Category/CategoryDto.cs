using System.ComponentModel.DataAnnotations;

namespace BackendShop.Core.Dto.Category
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        //public string? Description { get; set; }
        public string ImageCategory { get; set; } = string.Empty;

        //public virtual ICollection<SubCategoryDto>? SubCategories { get; set; }
    }
}
