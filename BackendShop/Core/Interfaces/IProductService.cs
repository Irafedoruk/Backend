using BackendShop.Core.Dto.Product;

namespace BackendShop.Core.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<List<ProductDto>> GetProductsBySubCategoryIdAsync(int subCategoryId);
        Task CreateAsync(CreateProductDto model);
        Task EditAsync(EditProductDto model);
        Task DeleteAsync(int id);
        Task<ProductDescImageIdViewModel?> UploadDescImageAsync(ProductDescImageUploadViewModel model);
    }
}
