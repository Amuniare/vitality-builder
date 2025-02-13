using System.ComponentModel.DataAnnotations;
using VitalityBuilder.Api.Domain.Constants;

namespace VitalityBuilder.Api.Domain.Dtos.Attributes;

/// <summary>
/// Data transfer object for combat-related attributes
/// </summary>
public class CombatAttributesDto
{
    [Range(0, GameRuleConstants.MaximumTier)]
    public int Focus { get; set; }

    [Range(0, GameRuleConstants.MaximumTier)]
    public int Power { get; set; }

    [Range(0, GameRuleConstants.MaximumTier)]
    public int Mobility { get; set; }

    [Range(0, GameRuleConstants.MaximumTier)]
    public int Endurance { get; set; }

    // Calculated Values
    public int TotalPoints => Focus + Power + Mobility + Endurance;

    public int AvailablePoints { get; set; }
    public int RemainingPoints => AvailablePoints - TotalPoints;

    // Combat Values (Calculated with Tier)
    public CombatValuesDto Values { get; set; } = new();

    public bool ValidateAgainstTier(int tier)
    {
        // Individual attributes cannot exceed tier
        if (Focus > tier || Power > tier || Mobility > tier || Endurance > tier)
        {
            return false;
        }

        // Total points cannot exceed tier × 2
        if (TotalPoints > tier * 2)
        {
            return false;
        }

        return true;
    }
}

/// <summary>
/// Calculated combat values based on attributes and tier
/// </summary>
public class CombatValuesDto
{
    // Core Defense Values
    public int Avoidance { get; set; }
    public int Durability { get; set; }

    // Resistance Values
    public int ResolveResistance { get; set; }
    public int StabilityResistance { get; set; }
    public int VitalityResistance { get; set; }

    // Combat Bonuses
    public int DamageBonus { get; set; }
    public int MovementSpeed { get; set; }

    // Point Status
    public bool HasSufficientPoints { get; set; }
    public ICollection<string> ValidationMessages { get; set; } = new List<string>();
}