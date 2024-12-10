using System.ComponentModel.DataAnnotations.Schema;

namespace BackendShop.Core.Dto.Product
{
    public class ProductDescImageIdViewModel
    {
        public required int Id { get; set; }
        public required string Image { get; set; }
    }
}
