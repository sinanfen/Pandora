using AutoMapper;
using Pandora.Core.Domain.Constants.Enums;
using Pandora.Core.Domain.Entities;
using Pandora.Shared.DTOs.UserDTOs;

namespace Pandora.Application.AutoMapper.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // Mapping for individual user update
        CreateMap<IndividualUserUpdateDto, IndividualUser>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ReverseMap(); // For two-way mapping if needed

        // Mapping for corporate user update
        CreateMap<CorporateUserUpdateDto, CorporateUser>()
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
            .ForMember(dest => dest.TaxNumber, opt => opt.MapFrom(src => src.TaxNumber))
            .ReverseMap();

        CreateMap<IndividualUser, UserDto>()
            .IncludeBase<User, UserDto>();

        CreateMap<CorporateUser, UserDto>()
            .IncludeBase<User, UserDto>();

        // Mapping for IndividualUser from IndividualUserRegisterDto
        CreateMap<IndividualUserRegisterDto, IndividualUser>()
            .IncludeBase<UserRegisterDto, IndividualUser>();

        // Mapping for CorporateUser from CorporateUserRegisterDto
        CreateMap<CorporateUserRegisterDto, CorporateUser>()
            .IncludeBase<UserRegisterDto, CorporateUser>();

        // General User mappings for Individual and Corporate users
        CreateMap<UserRegisterDto, IndividualUser>();
        CreateMap<UserRegisterDto, CorporateUser>();

        // General User mappings
        CreateMap<User, UserDto>().ReverseMap();
    }
}
