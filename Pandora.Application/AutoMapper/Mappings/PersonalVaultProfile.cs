using AutoMapper;
using Pandora.Application.DTOs.PersonalVaultDTOs;
using Pandora.Core.Domain.Entities;

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
