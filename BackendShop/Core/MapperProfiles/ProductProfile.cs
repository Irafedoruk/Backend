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
                .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.SubCategory.Name))
                .ForMember(x => x.Images, opt => opt.MapFrom(x => x.Images.OrderBy(x => x.Priority)
                    .Select(p => p.Image).ToArray()));

            CreateMap<CreateProductDto, Product>();

            CreateMap<EditProductDto, Product>()
                .ForMember(x => x.Images, opt => opt.Ignore());

           CreateMap<IFormFile, ProductImageEntity>()
             .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.FileName)) // Це базове значення, якщо Image — це шлях
             .ForMember(dest => dest.Priority, opt => opt.Ignore());

            CreateMap<ProductDescImageEntity, ProductDescImageIdViewModel>();
            //CreateMap<Product, ProductDto>()
            //    .ForMember(dest => dest.Images, opt => opt.MapFrom(src =>
            //        src.Images != null ? src.Images.Select(img => $"/images/{img.Image}").ToList() : new List<string>()))
            //    .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.SubCategory != null ? src.SubCategory.Name : string.Empty));

            //CreateMap<CreateProductDto, Product>().ReverseMap();

            ////CreateMap<ProductEditViewModel, ProductEntity>()
            //// .ForMember(x => x.ProductImages, opt => opt.Ignore());

            //CreateMap<ProductDescImageEntity, ProductDescImageIdViewModel>();
        }
    }
}
