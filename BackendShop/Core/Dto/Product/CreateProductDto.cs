using Microsoft.AspNetCore.Mvc;

namespace BackendShop.Core.Dto.Product
{
    public class CreateProductDto
    {

        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }



        public string? Manufacturer { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }

        public string? Type { get; set; }


        public string? Form { get; set; }
        public decimal Price { get; set; }
        public int? QuantityInPack { get; set; }
        public int QuantityInStock { get; set; }
        //////public bool IsAvailable { get; set; }

        //public string? Model { get; set; }
        [BindProperty(Name = "images[]")]
        public List<IFormFile>? Images { get; set; }
        public List<int> ImagesDescIds { get; set; } = [];
        public int SubCategoryId { get; set; }
    }
}
