using AutoMapper;
using Pandora.Core.Domain.Entities;
using Pandora.Shared.DTOs.PasswordVaultDTOs;

namespace Pandora.Application.AutoMapper.Mappings;

public class PasswordVaultProfile : Profile
{
    public PasswordVaultProfile()
    {
        //CreateMap<PasswordVaultAddDto, PasswordVault>();

        CreateMap<PasswordVaultAddDto, PasswordVault>()
        .ForMember(dest => dest.SecureSiteName, opt => opt.MapFrom(src => src.SiteName))
        .ForMember(dest => dest.SecureUsernameOrEmail, opt => opt.MapFrom(src => src.UsernameOrEmail))
        .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))  // The password will be hashed separately
        .ForMember(dest => dest.SecureNotes, opt => opt.MapFrom(src => src.Notes));

        //CreateMap<PasswordVaultDto, PasswordVault>();

        // Mapping for UpdateDto to PasswordVault
        CreateMap<PasswordVaultUpdateDto, PasswordVault>()
            .ForMember(dest => dest.SecureSiteName, opt => opt.MapFrom(src => src.SiteName))
            .ForMember(dest => dest.SecureUsernameOrEmail, opt => opt.MapFrom(src => src.UsernameOrEmail))
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.SecureNotes, opt => opt.MapFrom(src => src.Notes))
            .ForMember(dest => dest.LastPasswordChangeDate, opt => opt.MapFrom(src => DateTime.UtcNow))  // Update LastPasswordChangeDate
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));

        // PasswordVault → PasswordVaultDto (Navigation property'den field çekiyoruz)
        CreateMap<PasswordVault, PasswordVaultDto>()
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User != null ? src.User.Username : string.Empty));
        
        // PasswordVaultDto → PasswordVault (Sadece temel alanlar)
        CreateMap<PasswordVaultDto, PasswordVault>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.Category, opt => opt.Ignore()) // Navigation property'yi ignore et
            .ForMember(dest => dest.User, opt => opt.Ignore()); // Navigation property'yi ignore et

    }
}