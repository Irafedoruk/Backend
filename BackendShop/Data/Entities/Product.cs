using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendShop.Data.Entities
{
    [Table("tblProducts")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string Code { get; set; } = string.Empty;

        [Required, StringLength(255)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Manufacturer { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }

        public string? Type { get; set; }


        public string? Form { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public int? QuantityInPack { get; set; }
        [Required]
        public int QuantityInStock { get; set; }
       // public bool IsAvailable { get; set; }

        public string? Model { get; set; }
        [ForeignKey("SubCategory")]
        public int SubCategoryId { get; set; }
        public SubCategory? SubCategory { get; set; }
        public virtual ICollection<ProductImageEntity>? Images { get; set; }
        public virtual ICollection<ProductDescImageEntity>? ProductDescImages { get; set; }
    }
}
