using Microsoft.EntityFrameworkCore.Query;
using Pandora.Application.DTOs.UserDTOs;
using Pandora.Application.Utilities.Results.Interfaces;
using Pandora.Core.Domain.Entities;
using Pandora.Core.Persistence.Paging;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace Pandora.Application.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetAsync(
        Expression<Func<User, bool>> predicate,
        Func<IQueryable<User>, IIncludableQueryable<User, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default);
    Task<Paginate<UserDto>?> GetListAsync(
      Expression<Func<User, bool>>? predicate = null,
      Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
      Func<IQueryable<User>, IIncludableQueryable<User, object>>? include = null,
      int index = 0,
      int size = 10,
      bool withDeleted = false,
      bool enableTracking = true,
      CancellationToken cancellationToken = default);
    Task<UserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<UserDto?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
    Task<IDataResult<UserDto>> RegisterUserAsync(UserRegisterDto dto,CancellationToken cancellationToken);
    Task<IDataResult<UserDto>> UpdateUserAsync(UserUpdateDto dto, CancellationToken cancellationToken);
    Task<UserDto> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken, bool withDeleted = false);
    Task<string> GeneratePasswordHashAsync(string password);
    Task<bool> VerifyPasswordHashAsync(string hashedPassword, string password);



}
