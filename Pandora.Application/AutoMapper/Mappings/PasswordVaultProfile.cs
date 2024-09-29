using AutoMapper;
using Pandora.Application.DTOs.PasswordVaultDTOs;
using Pandora.Core.Domain.Entities;

namespace Pandora.Application.AutoMapper.Mappings;

public class PasswordVaultProfile : Profile
{
    public PasswordVaultProfile()
    {
        CreateMap<PasswordVaultAddDto, PasswordVault>();
        CreateMap<PasswordVaultDto, PasswordVault>();
        CreateMap<PasswordVault, PasswordVaultDto>().ReverseMap();
    }
}