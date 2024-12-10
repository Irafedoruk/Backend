namespace BackendShop.Core.Dto.SubCategory
{
    public class CreateSubCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        //public string? Description { get; set; }
        public IFormFile? ImageSubCategory { get; set; }
        public int CategoryId { get; set; }
    }
}
