using AutoMapper;
using Pandora.Core.Domain.Entities;
using Pandora.Shared.DTOs.PersonalVaultDTOs;

namespace Pandora.Application.AutoMapper.Mappings;

public class PersonalVaultProfile : Profile
{
    public PersonalVaultProfile()
    {
        CreateMap<PersonalVaultAddDto, PersonalVault>();
        CreateMap<PersonalVaultDto, PersonalVault>();
        CreateMap<PersonalVault, PersonalVaultDto>().ReverseMap();
    }
}
