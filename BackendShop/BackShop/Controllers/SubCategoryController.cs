using AutoMapper;
using AutoMapper.QueryableExtensions;
using BackendShop.Core.Dto.Category;
using BackendShop.Core.Dto.SubCategory;
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
    public class SubCategoryController(ShopDbContext _context, IMapper mapper, IImageHulk imageHulk, IConfiguration configuration) : ControllerBase
    {
        // GET: api/SubCategory
        [HttpGet]
        public IActionResult GetList()
        {
            var list = _context.SubCategories
                .ProjectTo<SubCategoryDto>(mapper.ConfigurationProvider)
                .ToList();
            return Ok(list);
        }

        // GET: api/SubCategory/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _context.SubCategories
                .ProjectTo<SubCategoryDto>(mapper.ConfigurationProvider)
                .SingleOrDefault(x => x.Id == id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        // POST: api/SubCategory/create
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CreateSubCategoryDto model)
        {
            if (!_context.Categories.Any(c => c.CategoryId == model.CategoryId))
            {
                return BadRequest("Invalid CategoryId.");
            }

            var imageName = await imageHulk.Save(model.ImageSubCategory);
            var entity = mapper.Map<SubCategory>(model);
            entity.ImageSubCategoryPath = imageName;
            _context.SubCategories.Add(entity);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // PUT: api/SubCategory/{id}
        [HttpPut]
        public async Task<IActionResult> Edit([FromForm] EditSubCategoryDto model)
        {
            var subCategory = _context.SubCategories.SingleOrDefault(x => x.SubCategoryId == model.Id);
            if (subCategory == null) return NotFound();

            // Update basic fields
            subCategory.Name = model.Name;
            subCategory.CategoryId = model.CategoryId;

            // Handle image update
            if (model.ImageSubCategory != null)
            {
                // Delete the old image if it exists
                if (!string.IsNullOrEmpty(subCategory.ImageSubCategoryPath))
                {
                    imageHulk.Delete(subCategory.ImageSubCategoryPath);
                }

                // Save the new image
                var newImageName = await imageHulk.Save(model.ImageSubCategory);
                subCategory.ImageSubCategoryPath = newImageName;
            }
            else if (string.IsNullOrEmpty(subCategory.ImageSubCategoryPath))
            {
                // Якщо нема зображення і поточне значення відсутнє
                subCategory.ImageSubCategoryPath = "noimage.jpg";
            }
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/SubCategory/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var subCategory = _context.SubCategories.SingleOrDefault(x => x.SubCategoryId == id);
            if (subCategory == null) return NotFound();

            if (!string.IsNullOrEmpty(subCategory.ImageSubCategoryPath))
                imageHulk.Delete(subCategory.ImageSubCategoryPath);

            _context.SubCategories.Remove(subCategory);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}

