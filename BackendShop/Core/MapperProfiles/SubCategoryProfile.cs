using AutoMapper;
using BackendShop.Core.Dto.Category;
using BackendShop.Core.Dto.SubCategory;
using BackendShop.Data.Entities;

public class SubCategoryProfile : Profile
{
    public SubCategoryProfile()
    {
        // Mapping SubCategory Entity to SubCategoryDto
        CreateMap<SubCategory, SubCategoryDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SubCategoryId))
            //.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.ImageSubCategory, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.ImageSubCategoryPath) ? "noimage.jpg" : src.ImageSubCategoryPath));

        CreateMap<CreateSubCategoryDto, SubCategory>()
            .ForMember(x => x.ImageSubCategoryPath, opt => opt.Ignore());

        CreateMap<EditSubCategoryDto, SubCategory>()
            .ForMember(x => x.ImageSubCategoryPath, opt => opt.Ignore());

    }
}

