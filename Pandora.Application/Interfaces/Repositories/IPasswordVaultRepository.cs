using Pandora.Core.Domain.Entities;
using Pandora.Core.Domain.Interfaces;

namespace Pandora.Application.Interfaces.Repositories;

public interface IPasswordVaultRepository: IAsyncRepository<PasswordVault, Guid>, IRepository<PasswordVault, Guid>
{

}