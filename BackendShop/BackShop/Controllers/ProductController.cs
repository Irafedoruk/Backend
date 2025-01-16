using AutoMapper;
using AutoMapper.QueryableExtensions;
using BackendShop.Core.Dto.Category;
using BackendShop.Core.Dto.Product;
using BackendShop.Core.Interfaces;
using BackendShop.Core.Services;
using BackendShop.Data.Data;
using BackendShop.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace BackendShop.BackShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetProductsAsync();
            return Ok(products);
        }

        //public async Task<IActionResult> Create([FromForm] CategoryCreateViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        var imageName = await imageHulk.Save(model.ImageCategory);
        //        var entity = mapper.Map<Category>(model);
        //        entity.ImageCategoryPath = imageName;

        //        _context.Categories.Add(entity);
        //        await _context.SaveChangesAsync();

        //        return Ok(new { message = "Category created successfully!" });
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //        return StatusCode(500, "An error occurred while creating the category.");
        //    }
        //}

        //Post: api/Product/create
        
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CreateProductDto model)
        {
            try
            {
                await _productService.CreateAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Products/2
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        // PUT: api/Product/2
        [HttpPut]
        public async Task<IActionResult> Edit([FromForm] EditProductDto model)
        {
            try
            {
                await _productService.EditAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadDescImage([FromForm] ProductDescImageUploadViewModel model)
        {
            try
            {
                var result = await _productService.UploadDescImageAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Products/bySubCategory/2
        [HttpGet("bySubCategory/{subCategoryId}")]
        public async Task<IActionResult> GetProductsBySubCategoryId(int subCategoryId)
        {
            var products = await _productService.GetProductsBySubCategoryIdAsync(subCategoryId);
            return Ok(products);
        }

        //DELETE: api/Products/2
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            return Ok();
        }

    }
}
