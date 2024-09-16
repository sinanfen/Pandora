using AutoMapper;
using Pandora.Application.Interfaces;
using Pandora.Application.Interfaces.Repositories;
using Pandora.Application.Security.Interfaces;

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
}
