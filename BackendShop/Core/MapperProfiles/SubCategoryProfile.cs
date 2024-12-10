using AutoMapper;
using BackendShop.Core.Dto.SubCategory;
using BackendShop.Data.Entities;

public class SubCategoryProfile : Profile
{
    public SubCategoryProfile()
    {
        // Mapping SubCategory Entity to SubCategoryDto
        CreateMap<SubCategory, SubCategoryDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SubCategoryId)).ReverseMap();
        //.ForMember(dest => dest.ImageSubCategory, opt => opt.MapFrom(src =>
        //    string.IsNullOrEmpty(src.ImageSubCategoryPath) ? "/images/noimage.jpg" : $"/images/{src.ImageSubCategoryPath}"))
        //.ReverseMap();

        // Mapping SubCategoryCreateViewModel to SubCategory Entity
        CreateMap<CreateSubCategoryDto, SubCategory>()
            .ForMember(dest => dest.ImageSubCategoryPath, opt => opt.Ignore())
            .ReverseMap();

        // Mapping SubCategoryEditViewModel to SubCategory Entity
        //CreateMap<CreateSubCategoryDto, SubCategory>()
        //    .ForMember(dest => dest.ImageSubCategoryPath, opt => opt.Ignore())
        //    .ReverseMap();
    }
}

