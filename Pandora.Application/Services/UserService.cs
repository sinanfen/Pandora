using AutoMapper;
using Pandora.Application.DTOs.UserDTOs;
using Pandora.Application.Interfaces;
using Pandora.Application.Interfaces.Repositories;
using Pandora.Application.Security.Interfaces;
using Pandora.Core.Domain.Entities;
using System.Security.Authentication;

namespace Pandora.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IHasher _hasher;
    private readonly IEncryption _encryption;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper, IHasher hasher, IEncryption encryption)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _hasher = hasher;
        _encryption = encryption;
    }

    public async Task<UserDto?> GetByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return user == null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> GetByUsernameAsync(string username)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        return user == null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> RegisterUserAsync(UserRegisterDto dto)
    {
        var user = _mapper.Map<User>(dto); // Map registration DTO to user entity
        user.PasswordHash = _hasher.HashPassword(dto.Password, HashAlgorithmType.Sha512); // Use hasher for password hashing
        user.SecurityStamp = Guid.NewGuid().ToString(); // Assign security stamp

        await _userRepository.AddAsync(user); // Add user to the repository
        return _mapper.Map<UserDto>(user); // Return the newly created UserDto
    }

    public async Task<UserDto> UpdateUserAsync(UserUpdateDto dto)
    {
        var user = await _userRepository.GetAsync(u => u.Id == dto.Id);
        if (user == null) throw new Exception("User not found");

        // Map updated fields from DTO to the entity
        _mapper.Map(dto, user);
        await _userRepository.UpdateAsync(user);

        return _mapper.Map<UserDto>(user); // Return updated UserDto
    }

    public Task<string> GeneratePasswordHashAsync(string password)
    {
        return Task.FromResult(_hasher.HashPassword(password, HashAlgorithmType.Sha512)); // Use SHA512 for hashing
    }

    public Task<bool> VerifyPasswordHashAsync(string hashedPassword, string password)
    {
        var result = _hasher.VerifyHashedPassword(hashedPassword, password, HashAlgorithmType.Sha512);
        return Task.FromResult(result);
    }
}
