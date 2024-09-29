using Pandora.Application.Interfaces.Repositories;

namespace Pandora.Application.BusinessRules;

public class PersonalVaultBusinessRules
{
    private readonly IPersonalVaultRepository _personalVaultRepository;

    public PersonalVaultBusinessRules(IPersonalVaultRepository personalVaultRepository)
    {
        _personalVaultRepository = personalVaultRepository;
    }
}

