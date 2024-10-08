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
        CreateMap<Category, CategoryDto>().ReverseMap();
    }
}
