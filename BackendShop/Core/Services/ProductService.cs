using AutoMapper;
using AutoMapper.QueryableExtensions;
using BackendShop.Core.Dto.Product;
using BackendShop.Core.Interfaces;
using BackendShop.Data.Data;
using BackendShop.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendShop.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly ShopDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageHulk _imageHulk;

        public ProductService(ShopDbContext context, IMapper mapper, IImageHulk imageHulk)
        {
            _context = context;
            _mapper = mapper;
            _imageHulk = imageHulk;
        }

        public async Task<List<ProductDto>> GetProductsAsync()
        {
            return await _context.Products
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<ProductDto>> GetProductsBySubCategoryIdAsync(int subCategoryId)
        {
            return await _context.Products
                .Where(p => p.SubCategoryId == subCategoryId)
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task CreateAsync(CreateProductDto model)
        {
            var entity = _mapper.Map<Product>(model);
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
                var priority = 1;
                foreach (var image in model.Images)
                {
                    var productImage = new ProductImageEntity
                    {
                        Image = await _imageHulk.Save(image),
                        Priority = priority,
                        ProductId = entity.ProductId
                    };
                    priority++;
                    _context.ProductImageEntity.Add(productImage);
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task EditAsync(EditProductDto model)
        {
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.ProductId == model.Id);

            if (product == null) throw new Exception("Product not found.");

            _mapper.Map(model, product);

            var oldImageNames = model.Images?
                .Where(x => x.ContentType.Contains("old-image"))
                .Select(x => x.FileName)
                ?? Enumerable.Empty<string>();

            var imagesToDelete = product.ProductImages?
                .Where(x => !oldImageNames.Contains(x.Image))
                ?? Enumerable.Empty<ProductImageEntity>();

            foreach (var img in imagesToDelete)
            {
                _context.ProductImageEntity.Remove(img);
                _imageHulk.Delete(img.Image);
            }

            if (model.Images != null)
            {
                int index = 0;
                foreach (var image in model.Images)
                {
                    if (image.ContentType == "old-image")
                    {
                        var existingImage = product.ProductImages
                            .FirstOrDefault(x => x.Image == image.FileName);
                        if (existingImage != null)
                        {
                            existingImage.Priority = index;
                        }
                    }
                    else
                    {
                        var newImagePath = await _imageHulk.Save(image);
                        _context.ProductImageEntity.Add(new ProductImageEntity
                        {
                            Image = newImagePath,
                            Priority = index,
                            ProductId = product.ProductId
                        });
                    }
                    index++;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductDescImages)
                .SingleOrDefaultAsync(p => p.ProductId == id);

            if (product == null) throw new Exception("Product not found.");

            if (product.ProductImages != null)
            {
                foreach (var img in product.ProductImages)
                {
                    _imageHulk.Delete(img.Image);
                }
            }

            if (product.ProductDescImages != null)
            {
                foreach (var img in product.ProductDescImages)
                {
                    _imageHulk.Delete(img.Image);
                }
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<ProductDescImageIdViewModel?> UploadDescImageAsync(ProductDescImageUploadViewModel model)
        {
            if (model.Image == null) throw new Exception("No image provided.");

            var descImage = new ProductDescImageEntity
            {
                Image = await _imageHulk.Save(model.Image),
            };

            _context.ProductDescImages.Add(descImage);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductDescImageIdViewModel>(descImage);
        }
    }
}
