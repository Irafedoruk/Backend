using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class ProductController(ShopDbContext _context, IMapper mapper, IImageHulk imageHulk, IConfiguration configuration) : ControllerBase
    {
        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var model = await _context.Products
                .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
                .ToListAsync();
            return Ok(model);
        }

        //Post: api/Product
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductDto model)
        {
            var entity = mapper.Map<Product>(model);
            _context.Products.Add(entity);
            _context.SaveChanges();

            if (model.ImagesDescIds.Any())
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
            return Created();
        }

        // GET: api/Product/2
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
        //[HttpPut("{id}")]
        //public async Task<IActionResult> EditProduct(int id, [FromForm] ProductEditModel model)
        //{
        //    var product = await _context.Products
        //        .Include(p => p.Images) // Завантажуємо всі зображення продукту
        //        .FirstOrDefaultAsync(p => p.Id == id);

        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    // Мапимо основні поля з моделі редагування на сутність продукту
        //    mapper.Map(model, product);

        //    var dir = configuration["ImageDir"];
        //    var dirPath = Path.Combine(Directory.GetCurrentDirectory(), dir);

        //    // Додаємо нові зображення, якщо вони надані
        //    if (model.Images != null && model.Images.Any())
        //    {
        //        foreach (var imageFile in model.Images)
        //        {
        //            string imageName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
        //            var fileSave = Path.Combine(dirPath, imageName);

        //            using (var stream = new FileStream(fileSave, FileMode.Create))
        //                await imageFile.CopyToAsync(stream);

        //            var productImage = new ProductImageEntity
        //            {
        //                ProductId = product.Id,
        //                Image = imageName,
        //                Priority = model.Priority // Можна передавати пріоритет для зображень
        //            };
        //            _context.ProductImages.Add(productImage);
        //        }
        //    }

        // Видаляємо старі зображення, якщо їх потрібно замінити
        //    if (model.RemoveImageIds != null && model.RemoveImageIds.Any())
        //    {
        //        var imagesToRemove = product.Images
        //            .Where(img => model.RemoveImageIds.Contains(img.Id))
        //            .ToList();

        //        foreach (var image in imagesToRemove)
        //        {
        //            var imagePath = Path.Combine(dirPath, image.Image);
        //            if (System.IO.File.Exists(imagePath))
        //            {
        //                System.IO.File.Delete(imagePath);
        //            }
        //            _context.ProductImages.Remove(image);
        //        }
        //    }

        //    _context.Products.Update(product);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

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

        //DELETE: api/Product/2
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _context.Products
                .Include(x => x.Images)
                .Include(x => x.ProductDescImages)
                .SingleOrDefault(x => x.ProductId == id);
            if (product == null) return NotFound();

            if (product.Images != null)
                foreach (var p in product.Images)
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
