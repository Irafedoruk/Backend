using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackendShop.Core.Dto.Product
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; } 

        public List<string>? Images { get; set; }

        public string? Manufacturer { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }

        public string? Type { get; set; }


        public string? Form { get; set; }
        public decimal Price { get; set; }
        public int? QuantityInPack { get; set; }
        public int QuantityInStock { get; set; }
        //public bool IsAvailable { get; set; }

        public string? Model { get; set; }
        public int SubCategoryId { get; set; }
        public string? SubCategoryName { get; set; }
    }
}
