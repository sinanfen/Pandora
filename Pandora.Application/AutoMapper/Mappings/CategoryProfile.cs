using AutoMapper;
using Pandora.Application.DTOs.CategoryDTOs;
using Pandora.Core.Domain.Entities;

namespace Pandora.Application.AutoMapper.Mappings;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CategoryAddDto, Category>();
        CreateMap<CategoryUpdateDto, Category>();
        CreateMap<Category, CategoryDto>().ReverseMap();
    }
}
