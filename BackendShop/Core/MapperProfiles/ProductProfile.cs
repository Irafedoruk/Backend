using AutoMapper;
using BackendShop.Core.Dto.Product;
using BackendShop.Data.Entities;

namespace BackendShop.Core.MapperProfiles
{
    public class ProductProfile: Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.SubCategory.Name))
                .ForMember(x => x.Images, opt => opt.MapFrom(x => x.ProductImages.OrderBy(x => x.Priority)
                    .Select(p => p.Image).ToArray()));

            CreateMap<ProductDto, Product>();

            
            CreateMap<ProductDescImageEntity, ProductDescImageIdViewModel>();

            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.ProductImages, opt => opt.Ignore()) // Ігноруємо Images, якщо вони додаються окремо
                .ForMember(dest => dest.SubCategory, opt => opt.Ignore()); // Ігноруємо зв'язок SubCategory

            CreateMap<EditProductDto, Product>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SubCategory, opt => opt.Ignore()) // Залежно від вашої логіки
                .ForMember(dest => dest.ProductImages, opt => opt.Ignore()); // Якщо ви не хочете автоматично змінювати зображення в продукті
            //CreateMap<ProductEditViewModel, ProductEntity>()
            //    .ForMember(x => x.ProductImages, opt => opt.Ignore());

            //CreateMap<Product, ProductDto>()
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId))
            //    .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.SubCategory.Name))
            //    .ForMember(x => x.Images, opt => opt.MapFrom(x => x.Images.OrderBy(x => x.Priority)
            //        .Select(p => p.Image).ToArray()));

            //CreateMap<CreateProductDto, Product>();

            //CreateMap<EditProductDto, Product>()
            //    .ForMember(x => x.ProductImages, opt => opt.Ignore());

            ////CreateMap<IFormFile, ProductImageEntity>()
            ////  .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.FileName)) // Це базове значення, якщо Image — це шлях
            ////  .ForMember(dest => dest.Priority, opt => opt.Ignore());
            //CreateMap<IFormFile, ProductImageEntity>()
            //    .ForMember(dest => dest.Image, opt => opt.Ignore())
            //    .ForMember(dest => dest.Priority, opt => opt.Ignore());


            //CreateMap<ProductDescImageEntity, ProductDescImageIdViewModel>();
        }
    }
}
