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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {            
            var list = await _categoryService.GetListAsync();
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
                await _categoryService.CreateAsync(model);
                return Ok();
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
            await _categoryService.DeleteAsync(id);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromForm] CategoryEditViewModel model, [FromForm] string? currentImage)
        {
            await _categoryService.EditAsync(model);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound(new { message = "Категорію не знайдено" });
            return Ok(category);
        }

        // Для отримання підкатегорій категорії
        [HttpGet("subcategories")]
        public async Task<IActionResult> GetSubCategoriesByCategoryId(int categoryId)
        {
            var subCategories = await _categoryService.GetSubCategoriesByCategoryIdAsync(categoryId);
            return Ok(subCategories);
        }
    }
}
