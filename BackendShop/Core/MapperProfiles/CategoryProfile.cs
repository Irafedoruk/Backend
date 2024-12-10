using AutoMapper;
using BackendShop.Core.Dto.Category;
using BackendShop.Data.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BackendShop.Core.MapperProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId));
            //.ForMember(x => x.ImageCategory, opt => opt.MapFrom(x =>
            // string.IsNullOrEmpty(x.ImageCategoryPath) ? "/images/noimage.jpg" : $"/images/{x.ImageCategoryPath}"));
            CreateMap<CategoryCreateViewModel, Category>()
                .ForMember(x => x.ImageCategoryPath, opt => opt.Ignore());
            CreateMap<CategoryEditViewModel, Category>()
                .ForMember(x => x.ImageCategoryPath, opt => opt.Ignore());
        }
    }
}
