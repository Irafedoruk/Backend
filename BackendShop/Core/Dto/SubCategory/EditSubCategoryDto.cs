namespace BackendShop.Core.Dto.SubCategory
{
    public class EditSubCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public IFormFile? ImageSubCategory { get; set; } // Optional new image
        public int CategoryId { get; set; }

        public string? CurrentImage { get; set; }
    }
}
