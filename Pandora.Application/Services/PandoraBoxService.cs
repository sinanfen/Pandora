using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Pandora.Application.DTOs.PandoraBoxDTOs;
using Pandora.Application.Interfaces;
using Pandora.Application.Interfaces.Repositories;
using Pandora.Application.Security.Interfaces;
using Pandora.Application.Utilities.Results.Interfaces;
using Pandora.Core.Domain.Entities;
using Pandora.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Pandora.Application.Services;

public class PandoraBoxService : IPandoraBoxService
{
    private readonly IPandoraBoxRepository _pandoraBoxRepository;
    private readonly IMapper _mapper;
    private readonly IHasher _hasher;
    private readonly IEncryption _encryption;

    public PandoraBoxService(IPandoraBoxRepository pandoraBoxRepository, IMapper mapper, IHasher hasher, IEncryption encryption)
    {
        _pandoraBoxRepository = pandoraBoxRepository;
        _mapper = mapper;
        _hasher = hasher;
        _encryption = encryption;
    }

    public Task<IDataResult<PandoraBoxDto>> AddAsync(PandoraBoxUpdateDto dto, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IResult> DeleteAsync(Guid pandoraBoxId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<List<PandoraBoxDto>> GetAllAsync(CancellationToken cancellationToken, bool withDeleted = false)
    {
        throw new NotImplementedException();
    }

    public Task<PandoraBoxDto?> GetAsync(Expression<Func<PandoraBox, bool>> predicate, Func<IQueryable<PandoraBox>, IIncludableQueryable<PandoraBox, object>>? include = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<PandoraBoxDto> GetByIdAsync(Guid pandoraBoxId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Paginate<PandoraBoxDto>?> GetListAsync(Expression<Func<PandoraBox, bool>>? predicate = null, Func<IQueryable<PandoraBox>, IOrderedQueryable<PandoraBox>>? orderBy = null, Func<IQueryable<PandoraBox>, IIncludableQueryable<PandoraBox, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IDataResult<PandoraBoxDto>> UpdateAsync(PandoraBoxUpdateDto dto, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
