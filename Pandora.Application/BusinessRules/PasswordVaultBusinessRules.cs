using Pandora.Application.Interfaces.Repositories;
namespace Pandora.Application.BusinessRules;

public class PasswordVaultBusinessRules
{
    private readonly IPasswordVaultRepository _passwordVaultRepository;

    public PasswordVaultBusinessRules(IPasswordVaultRepository passwordVaultRepository)
    {
        _passwordVaultRepository = passwordVaultRepository;
    }   
}

