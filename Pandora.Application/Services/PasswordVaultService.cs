using AutoMapper;
using Pandora.Application.Interfaces;
using Pandora.Application.Interfaces.Repositories;
using Pandora.Application.Security.Interfaces;

namespace Pandora.Application.Services;

public class PasswordVaultService : IPasswordVaultService
{
    private readonly IPasswordVaultRepository _passwordVaultRepository;
    private readonly IMapper _mapper;
    private readonly IHasher _hasher;
    private readonly IEncryption _encryption;

    public PasswordVaultService(IPasswordVaultRepository passwordVaultRepository, IMapper mapper, IHasher hasher, IEncryption encryption)
    {
        _passwordVaultRepository = passwordVaultRepository;
        _mapper = mapper;
        _hasher = hasher;
        _encryption = encryption;
    }
}