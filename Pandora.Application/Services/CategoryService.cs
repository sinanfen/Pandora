using AutoMapper;
using Pandora.Application.Interfaces;
using Pandora.Application.Interfaces.Repositories;

namespace Pandora.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }
}
