using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackendShop.Data.Entities
{
    public class ProductImageEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(255)]
        public string Image { get; set; } = string.Empty;

        public int Priority { get; set; } // Пріоритет відображення зображення

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public virtual Product? Product { get; set; }
    }
}
