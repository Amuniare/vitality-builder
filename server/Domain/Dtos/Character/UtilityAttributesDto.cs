using System.ComponentModel.DataAnnotations;
using VitalityBuilder.Domain.Constants;

namespace VitalityBuilder.Domain.Dtos.Attributes;

/// <summary>
/// Data transfer object for utility-related attributes
/// </summary>
public class UtilityAttributesDto
{
    [Range(0, GameRuleConstants.MaximumTier)]
    public int Awareness { get; set; }

    [Range(0, GameRuleConstants.MaximumTier)]
    public int Communication { get; set; }

    [Range(0, GameRuleConstants.MaximumTier)]
    public int Intelligence { get; set; }

    // Calculated Values
    public int TotalPoints => Awareness + Communication + Intelligence;

    public int AvailablePoints { get; set; }
    public int RemainingPoints => AvailablePoints - TotalPoints;

    // Utility Values (Calculated with Tier)
    public UtilityValuesDto Values { get; set; } = new();

    public bool ValidateAgainstTier(int tier)
    {
        // Individual attributes cannot exceed tier
        if (Awareness > tier || Communication > tier || Intelligence > tier)
        {
            return false;
        }

        // Total points cannot exceed tier
        if (TotalPoints > tier)
        {
            return false;
        }

        return true;
    }
}

/// <summary>
/// Calculated utility values based on attributes and tier
/// </summary>
public class UtilityValuesDto
{
    // Core Ability Values
    public int InitiativeBonus { get; set; }
    public int DynamicEntryAllies { get; set; }
    public int DynamicEntrySpaces { get; set; }

    // Skill Check Information
    public ICollection<SkillBonusDto> SkillBonuses { get; set; } = new List<SkillBonusDto>();

    // Point Status
    public bool HasSufficientPoints { get; set; }
    public ICollection<string> ValidationMessages { get; set; } = new List<string>();
}

/// <summary>
/// Represents skill check bonuses for specific skills
/// </summary>
public class SkillBonusDto
{
    public string SkillName { get; set; } = string.Empty;
    public string AttributeName { get; set; } = string.Empty;
    public int AttributeBonus { get; set; }
    public int ExpertiseBonus { get; set; }
    public int TotalBonus => AttributeBonus + ExpertiseBonus;
}