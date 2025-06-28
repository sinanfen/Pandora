namespace Pandora.Shared.DTOs.PasswordVaultDTOs;

public class PasswordHealthReportDto
{
    public int SecurityScore { get; set; } // 0-100
    public string SecurityLevel { get; set; } = string.Empty; // Excellent, Good, Fair, Poor
    public int TotalPasswords { get; set; }
    public int WeakPasswords { get; set; }
    public int DuplicatePasswords { get; set; }
    public int OldPasswords { get; set; }
    public int ShortPasswords { get; set; }
    public int StrongPasswords { get; set; }
    public List<SecurityIssueDto> Issues { get; set; } = new();
    public List<SecurityRecommendationDto> Recommendations { get; set; } = new();
}

public class SecurityIssueDto
{
    public string Type { get; set; } = string.Empty; // Weak, Duplicate, Old, Short
    public string Message { get; set; } = string.Empty;
    public int Count { get; set; }
    public string Severity { get; set; } = string.Empty; // High, Medium, Low
    public string Icon { get; set; } = string.Empty; // Emoji or icon
}

public class SecurityRecommendationDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty; // High, Medium, Low
}

public class DuplicatePasswordDto
{
    public string SiteName { get; set; } = string.Empty;
    public List<string> DuplicateSites { get; set; } = new();
    public int UsageCount { get; set; }
    public DateTime LastUsed { get; set; }
}

public class WeakPasswordDto
{
    public Guid VaultId { get; set; }
    public string SiteName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string WeaknessReason { get; set; } = string.Empty;
    public int PasswordLength { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SecurityScoreDto
{
    public int OverallScore { get; set; } // 0-100
    public string Level { get; set; } = string.Empty; // Excellent (90-100), Good (70-89), Fair (50-69), Poor (0-49)
    public string LevelColor { get; set; } = string.Empty; // green, yellow, orange, red
    public string LevelIcon { get; set; } = string.Empty; // 🛡️, ⚠️, 🔶, 🚨
    public Dictionary<string, int> CategoryScores { get; set; } = new(); // Strength, Uniqueness, Age, etc.
    public int ImprovementPotential { get; set; } // Kaç puan artırılabilir
    public DateTime LastAnalyzed { get; set; }
} 