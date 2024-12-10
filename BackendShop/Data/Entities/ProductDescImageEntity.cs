using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendShop.Data.Entities
{
    public class ProductDescImageEntity
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(255)]
        public string Image { get; set; } = string.Empty;

        [ForeignKey("Product")]
        public int? ProductId { get; set; }
        public virtual Product? Product { get; set; }
    }
}
