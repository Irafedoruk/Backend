using BackendShop.Core.Dto.Category;
using BackendShop.Data.Entities;

namespace BackendShop.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetListAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task CreateAsync(CategoryCreateViewModel model);
        Task EditAsync(CategoryEditViewModel model);
        Task DeleteAsync(int id);
        Task<List<SubCategory>> GetSubCategoriesByCategoryIdAsync(int categoryId);
    }
}
