using Pandora.Application.DTOs.PandoraBoxDTOs;
using Pandora.Application.DTOs.PasswordVaultDTOs;

namespace Pandora.Application.DTOs.CategoryDTOs;

public class CategoryDto : BaseDto<Guid>
{
    public string Name { get; set; }  // Category name, e.g., Social Media, Work
    public string Description { get; set; }  // Optional category description

    // Associated vaults and Pandora's boxes
    public List<PasswordVaultDto> PasswordVaults { get; set; } = new List<PasswordVaultDto>();
    public List<PandoraBoxDto> PandoraBoxes { get; set; } = new List<PandoraBoxDto>();

    // Optional metadata for frontend display
    public int TotalVaults => PasswordVaults.Count;  // Count of vaults in this category
    public int TotalPandoraBoxes => PandoraBoxes.Count;  // Count of Pandora boxes in this category
}
