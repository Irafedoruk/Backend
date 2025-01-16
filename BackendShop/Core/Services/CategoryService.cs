using AutoMapper;
using AutoMapper.QueryableExtensions;
using BackendShop.Core.Dto.Category;
using BackendShop.Core.Interfaces;
using BackendShop.Data.Data;
using BackendShop.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendShop.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ShopDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageHulk _imageHulk;

        public CategoryService(ShopDbContext context, IMapper mapper, IImageHulk imageHulk)
        {
            _context = context;
            _mapper = mapper;
            _imageHulk = imageHulk;
        }

        public async Task<List<CategoryDto>> GetListAsync()
        {
            return await _context.Categories
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            return await _context.Categories
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task CreateAsync(CategoryCreateViewModel model)
        {
            var imageName = await _imageHulk.Save(model.ImageCategory);
            var entity = _mapper.Map<Category>(model);
            entity.ImageCategoryPath = imageName;

            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(CategoryEditViewModel model)
        {
            var category = _context.Categories.SingleOrDefault(x => x.CategoryId == model.Id);
            if (category == null) 
                throw new Exception("Категорію не знайдено");

            category.Name = model.Name;

            // Якщо завантажено нове зображення
            if (model.ImageCategory != null)
            {
                if (!string.IsNullOrEmpty(category.ImageCategoryPath))
                {
                    _imageHulk.Delete(category.ImageCategoryPath);
                }
                category.ImageCategoryPath = await _imageHulk.Save(model.ImageCategory);
            }
            else if (string.IsNullOrEmpty(category.ImageCategoryPath))
            {
                // Якщо нема зображення і поточне значення відсутнє
                category.ImageCategoryPath = "noimage.jpg";
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Categories.SingleOrDefaultAsync(c => c.CategoryId == id);
            if (entity == null)
                throw new Exception("Категорію не знайдено");

            if (!string.IsNullOrEmpty(entity.ImageCategoryPath))
            {
                _imageHulk.Delete(entity.ImageCategoryPath);
            }

            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SubCategory>> GetSubCategoriesByCategoryIdAsync(int categoryId)
        {
            return await _context.SubCategories
                .Where(sc => sc.CategoryId == categoryId)
                .ToListAsync();
        }
    }
}
