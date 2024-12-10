using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendShop.Data.Entities
{
    [Table("tblCategories")]
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required, StringLength(255)]
        public string Name { get; set; } = string.Empty;
        [StringLength(255)]
        public string? Description { get; set; } = string.Empty;
        public string? ImageCategoryPath { get; set; } = string.Empty;
        public List<SubCategory>? SubCategories { get; set; }
    }
}
