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

            [HttpPost]
            public async Task<IActionResult> Create([FromForm] CategoryCreateViewModel model)
            {
                var imageName = await imageHulk.Save(model.ImageCategory);
                var entity = mapper.Map<Category>(model);
                entity.ImageCategoryPath = imageName;
                _context.Categories.Add(entity);
                _context.SaveChanges();
                return Ok();
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
        public async Task<IActionResult> Edit([FromForm] CategoryDto model)
        {
            if (model == null) return NotFound();
            var category = _context.Categories.SingleOrDefault(x => x.CategoryId == model.Id);
            category = mapper.Map(model, category);
            if (model.ImageCategory != null)
            {
                imageHulk.Delete(category.ImageCategoryPath);
                string fname = await imageHulk.Save(model.ImageCategory);
                category.ImageCategoryPath = fname;
            }
            _context.SaveChanges();
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

            //[HttpGet("names")]
            //public async Task<IActionResult> GetCategoriesNames()
            //{
            //    var result = await context.Categories
            //    .ProjectTo<SelectItemViewModel>(mapper.ConfigurationProvider)
            //    .ToListAsync();
            //    return Ok(result);
            //}
        }
}
