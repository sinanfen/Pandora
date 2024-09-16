using Pandora.Core.Domain.Entities;
using Pandora.Core.Domain.Interfaces;

namespace Pandora.Application.Interfaces.Repositories;

public interface IPandoraBoxRepository: IAsyncRepository<PandoraBox, Guid>, IRepository<PandoraBox, Guid>
{

}
