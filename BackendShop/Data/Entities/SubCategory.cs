using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendShop.Data.Entities
{
    [Table("tblSubCategories")]
    public class SubCategory
    {
        [Key]
        public int SubCategoryId { get; set; }
        [Required, StringLength(255)]
        public string Name { get; set; } = string.Empty;
        [StringLength(255)]
        public string? Description { get; set; } = string.Empty;
        public string? ImageSubCategoryPath { get; set; } = string.Empty;
        public List<Product>? Products { get; set; }
        //[ForeignKey("Category")]
        public int CategoryId { get; set; }
        public  Category? Category { get; set; }
    }
}
