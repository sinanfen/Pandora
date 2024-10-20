using AutoMapper;
using Pandora.Core.Domain.Entities;
using Pandora.Shared.DTOs.UserDTOs;

namespace Pandora.Application.AutoMapper.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // UserUpdateDto ve UserDto arasında çift yönlü dönüşüm sağlıyoruz
        CreateMap<UserUpdateDto, UserDto>().ReverseMap();

        // UserUpdateDto ve User arasında çift yönlü dönüşüm sağlıyoruz
        CreateMap<UserUpdateDto, User>().ReverseMap();

        // User ve UserDto arasında çift yönlü dönüşüm (Rolleri dahil edebiliriz)
        CreateMap<User, UserDto>().ReverseMap();

        // UserRegisterDto ve User arasında dönüşüm sağlıyoruz
        CreateMap<UserRegisterDto, User>();
        CreateMap<User, UserDto>().ReverseMap();
    }
}
