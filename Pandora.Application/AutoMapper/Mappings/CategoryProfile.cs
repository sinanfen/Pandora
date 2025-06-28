using AutoMapper;
using Pandora.Core.Domain.Entities;
using Pandora.Shared.DTOs.CategoryDTOs;

namespace Pandora.Application.AutoMapper.Mappings;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CategoryAddDto, Category>();
        CreateMap<CategoryUpdateDto, Category>();
        
        // Category → CategoryDto (Navigation property'den field çekiyoruz)
        CreateMap<Category, CategoryDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.Username : string.Empty))
            .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User != null ? $"{src.User.FirstName} {src.User.LastName}".Trim() : string.Empty));
        
        // CategoryDto → Category (Sadece temel alanlar)
        CreateMap<CategoryDto, Category>()
            .ForMember(dest => dest.User, opt => opt.Ignore()); // Navigation property'yi ignore et
    }
}
