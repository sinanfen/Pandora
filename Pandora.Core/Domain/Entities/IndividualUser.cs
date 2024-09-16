using Pandora.Core.Domain.Constants.Enums;

namespace Pandora.Core.Domain.Entities;

// Core/Domain/Entities/IndividualUser.cs
public class IndividualUser : User
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public IndividualUser() : base()
    {
        UserType = UserType.Individual;
    }
}

