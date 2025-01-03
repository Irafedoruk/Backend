using AutoMapper;
using AutoMapper.QueryableExtensions;
using BackendShop.Core.Dto.Category;
using BackendShop.Core.Interfaces;
using BackendShop.Core.Services;
using BackendShop.Data.Data;
using BackendShop.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendShop.BackShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController (ShopDbContext _context, IMapper mapper, IImageHulk imageHulk, IConfiguration configuration) : ControllerBase
    {
    [HttpGet]
    public IActionResult GetList()
    {
        Thread.Sleep(1000);
        var list = _context.Categories
            .ProjectTo<CategoryDto>(mapper.ConfigurationProvider)
            .ToList();
        return Ok(list);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromForm] CategoryCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var imageName = await imageHulk.Save(model.ImageCategory);
            var entity = mapper.Map<Category>(model);
            entity.ImageCategoryPath = imageName;

            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Category created successfully!" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, "An error occurred while creating the category.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = _context.Categories.SingleOrDefault(x => x.CategoryId == id);
        if (entity == null)
            return NotFound();
        if (!string.IsNullOrEmpty(entity.ImageCategoryPath))
            imageHulk.Delete(entity.ImageCategoryPath);
        _context.Categories.Remove(entity);
        _context.SaveChanges();
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Edit([FromForm] CategoryEditViewModel model, [FromForm] string? currentImage)
    {
        var category = _context.Categories.SingleOrDefault(x => x.CategoryId == model.Id);
        if (category == null) return NotFound();

        category.Name = model.Name;

        // Якщо завантажено нове зображення
        if (model.ImageCategory != null)
        {
            if (!string.IsNullOrEmpty(category.ImageCategoryPath))
            {
                imageHulk.Delete(category.ImageCategoryPath);
            }
            category.ImageCategoryPath = await imageHulk.Save(model.ImageCategory);
        }
        else if (string.IsNullOrEmpty(category.ImageCategoryPath))
        {
            // Якщо нема зображення і поточне значення відсутнє
            category.ImageCategoryPath = "noimage.jpg";
        }
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var item = _context.Categories
            .ProjectTo<CategoryDto>(mapper.ConfigurationProvider)
            .SingleOrDefault(x => x.Id == id);
        return Ok(item);
    }

    // Для отримання підкатегорій категорії
    [HttpGet("subcategories")]
    public async Task<IActionResult> GetSubCategoriesByCategoryId(int categoryId)
    {
        var subCategories = await _context.SubCategories
                                            .Where(sc => sc.CategoryId == categoryId)
                                            .ToListAsync();
        return Ok(subCategories);
    }
    }
}
