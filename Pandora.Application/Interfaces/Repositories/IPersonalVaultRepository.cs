using Pandora.Core.Domain.Entities;
using Pandora.Core.Domain.Interfaces;

namespace Pandora.Application.Interfaces.Repositories;

public interface IPersonalVaultRepository: IAsyncRepository<PersonalVault, Guid>, IRepository<PersonalVault, Guid>
{

}
