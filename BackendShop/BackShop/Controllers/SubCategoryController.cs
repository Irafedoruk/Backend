using AutoMapper;
using AutoMapper.QueryableExtensions;
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

        // POST: api/SubCategory
        [HttpPost]
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
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] SubCategoryDto model)
        {
            var subCategory = _context.SubCategories.SingleOrDefault(x => x.SubCategoryId == id);
            if (subCategory == null) return NotFound();

            mapper.Map(model, subCategory);
            if (model.ImageSubCategory != null)
            {
                imageHulk.Delete(subCategory.ImageSubCategoryPath);
                string fname = await imageHulk.Save(model.ImageSubCategory);
                subCategory.ImageSubCategoryPath = fname;
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

