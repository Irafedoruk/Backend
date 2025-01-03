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
    public class ProductsController(ShopDbContext _context, IMapper mapper, IImageHulk imageHulk, IConfiguration configuration) : ControllerBase
    {
        // GET: api/Products
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var model = await _context.Products
                .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
                .ToListAsync();
            return Ok(model);
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

            var entity = mapper.Map<Product>(model);
            _context.Products.Add(entity);
            await _context.SaveChangesAsync();

            if (model.ImagesDescIds?.Any() == true)
            {
                await _context.ProductDescImages
                    .Where(x => model.ImagesDescIds.Contains(x.Id))
                    .ForEachAsync(x => x.ProductId = entity.ProductId);
            }

            if (model.Images != null)
            {                
                var p = 1;
                foreach (var image in model.Images)
                {
                    var pi = new ProductImageEntity
                    {
                        Image = await imageHulk.Save(image),
                        Priority = p,
                        ProductId = entity.ProductId
                    };
                    p++;
                    _context.ProductImageEntity.Add(pi);
                    await _context.SaveChangesAsync();
                }
            }
            Console.WriteLine($"Received {model.Images?.Count()} images.");
            return Created();
        }

        // GET: api/Products/2
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _context.Products
                .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        // PUT: api/Product/2
        [HttpPut]
        public async Task<IActionResult> Edit([FromForm] EditProductDto model)
        {
            var request = this.Request;
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.ProductId == model.Id);

            mapper.Map(model, product);

            var oldNameImages = model.Images.Where(x => x.ContentType.Contains("old-image"))
                .Select(x => x.FileName) ?? [];

            var imgToDelete = product?.ProductImages?.Where(x => !oldNameImages.Contains(x.Image)) ?? [];
            foreach (var imgDel in imgToDelete)
            {
                _context.ProductImageEntity.Remove(imgDel);
                imageHulk.Delete(imgDel.Image);
            }

            if (model.Images is not null)
            {
                int index = 0;
                foreach (var image in model.Images)
                {
                    if (image.ContentType == "old-image")
                    {
                        var oldImage = product?.ProductImages?.FirstOrDefault(x => x.Image == image.FileName)!;
                        oldImage.Priority = index;
                    }
                    else
                    {
                        var imagePath = await imageHulk.Save(image);
                        _context.ProductImageEntity.Add(new ProductImageEntity
                        {
                            Image = imagePath,
                            Product = product,
                            Priority = index
                        });
                    }
                    index++;
                }
            }
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadDescImage([FromForm] ProductDescImageUploadViewModel model)
        {
            if (model.Image != null)
            {
                var pdi = new ProductDescImageEntity
                {
                    Image = await imageHulk.Save(model.Image),

                };
                _context.ProductDescImages.Add(pdi);
                await _context.SaveChangesAsync();
                return Ok(mapper.Map<ProductDescImageIdViewModel>(pdi));
            }
            return BadRequest();
        }

        //DELETE: api/Products/2
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _context.Products
                .Include(x => x.ProductImages)
                .Include(x => x.ProductDescImages)
                .SingleOrDefault(x => x.ProductId == id);
            if (product == null) return NotFound();

            if (product.ProductImages != null)
                foreach (var p in product.ProductImages)
                    imageHulk.Delete(p.Image);

            if (product.ProductDescImages != null)
                foreach (var p in product.ProductDescImages)
                    imageHulk.Delete(p.Image);

            _context.Products.Remove(product);
            _context.SaveChanges();
            return Ok();
        }

    }
}
