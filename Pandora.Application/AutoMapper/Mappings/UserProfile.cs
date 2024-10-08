using AutoMapper;
using Pandora.Core.Domain.Constants.Enums;
using Pandora.Core.Domain.Entities;
using Pandora.Shared.DTOs.UserDTOs;

namespace Pandora.Application.AutoMapper.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserRegisterDto, User>()
            .Include<UserRegisterDto, IndividualUser>()
            .Include<UserRegisterDto, CorporateUser>();

        CreateMap<UserRegisterDto, IndividualUser>()
            .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => UserType.Individual));

        CreateMap<UserRegisterDto, CorporateUser>()
            .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => UserType.Corporate));

        // Diğer mapping işlemleri
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<UserUpdateDto, User>().ReverseMap();

    }
}