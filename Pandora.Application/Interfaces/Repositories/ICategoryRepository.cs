using Pandora.Core.Domain.Entities;
using Pandora.Core.Domain.Interfaces;

namespace Pandora.Application.Interfaces.Repositories;

public interface ICategoryRepository: IAsyncRepository<Category,Guid>, IRepository<Category, Guid>
{

}
