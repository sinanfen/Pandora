using AutoMapper;
using Pandora.Core.Domain.Entities;
using Pandora.Shared.DTOs.PersonalVaultDTOs;

namespace Pandora.Application.AutoMapper.Mappings;

public class PersonalVaultProfile : Profile
{
    public PersonalVaultProfile()
    {
        // PersonalVaultAddDto → PersonalVault
        CreateMap<PersonalVaultAddDto, PersonalVault>()
            .ForMember(dest => dest.ShareToken, opt => opt.Ignore()) // Share token service tarafından set edilir
            .ForMember(dest => dest.SharedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ShareViewCount, opt => opt.Ignore());

        CreateMap<PersonalVaultUpdateDto, PersonalVault>()
            .ForMember(dest => dest.ShareToken, opt => opt.Ignore()) // Update sırasında değişmez
            .ForMember(dest => dest.SharedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ShareViewCount, opt => opt.Ignore());
        
        // PersonalVault → PersonalVaultDto (Navigation property'den field çekiyoruz)
        CreateMap<PersonalVault, PersonalVaultDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User != null ? src.User.Username : string.Empty));
        
        // PersonalVaultDto → PersonalVault (Sadece temel alanlar)
        CreateMap<PersonalVaultDto, PersonalVault>()
            .ForMember(dest => dest.Category, opt => opt.Ignore()) // Navigation property'yi ignore et
            .ForMember(dest => dest.User, opt => opt.Ignore()) // Navigation property'yi ignore et
            .ForMember(dest => dest.ShareToken, opt => opt.Ignore()); // Share token'ı ignore et
    }
}
