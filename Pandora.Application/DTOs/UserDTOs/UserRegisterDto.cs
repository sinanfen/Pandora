using Pandora.Core.Domain.Constants.Enums;

namespace Pandora.Application.DTOs.UserDTOs;

public class UserRegisterDto
{
    // Common fields for all users
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }  // Optional but recommended for confirmation
    public string PhoneNumber { get; set; }
    public UserType UserType { get; set; } // Individual or Corporate

    // Corporate-specific fields
    public string CompanyName { get; set; }  // Required for corporate users
    public string TaxNumber { get; set; }    // Required for corporate users

    // Individual-specific fields
    public string FirstName { get; set; }    // Required for individual users
    public string LastName { get; set; }     // Required for individual users
}