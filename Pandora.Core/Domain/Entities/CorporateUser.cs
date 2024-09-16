using Pandora.Core.Domain.Constants.Enums;

namespace Pandora.Core.Domain.Entities;

// Core/Domain/Entities/CorporateUser.cs
public class CorporateUser : User
{
    public string CompanyName { get; set; }
    public string TaxNumber { get; set; }

    public CorporateUser() : base()
    {
        UserType = UserType.Corporate;
    }
}

