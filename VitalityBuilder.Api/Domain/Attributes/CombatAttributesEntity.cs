using System.ComponentModel.DataAnnotations;
using VitalityBuilder.Api.Domain.Character;

namespace VitalityBuilder.Api.Domain.Attributes;

public class CombatAttributes
{
    public int Id { get; set; }

    public int CharacterId { get; set; }
    public Domain.Character.Character character { get; set; } = null!;

    [Range(0, 10)]
    public int Focus { get; set; }

    [Range(0, 10)]
    public int Power { get; set; }

    [Range(0, 10)]
    public int Mobility { get; set; }

    [Range(0, 10)]
    public int Endurance { get; set; }

    /// <summary>
    /// Gets the total number of points spent on combat attributes
    /// </summary>
    public int TotalPoints => Focus + Power + Mobility + Endurance;

    /// <summary>
    /// Validates if the attributes are within the character's tier limits
    /// </summary>
    public bool ValidateAgainstTier(int characterTier)
    {
        // Individual attributes cannot exceed character tier
        if (Focus > characterTier || Power > characterTier || 
            Mobility > characterTier || Endurance > characterTier)
        {
            return false;
        }

        // Total points cannot exceed tier × 2
        if (TotalPoints > characterTier * 2)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Calculates the character's Avoidance score
    /// </summary>
    public int CalculateAvoidance(int characterTier)
    {
        return 10 + characterTier + Mobility;
    }

    /// <summary>
    /// Calculates the character's Durability score
    /// </summary>
    public int CalculateDurability(int characterTier)
    {
        return characterTier + (int)Math.Ceiling(Endurance * 1.5);
    }

    /// <summary>
    /// Calculates the character's Resolve Resistance
    /// </summary>
    public int CalculateResolveResistance(int characterTier)
    {
        return 10 + characterTier + Focus;
    }

    /// <summary>
    /// Calculates the character's Stability Resistance
    /// </summary>
    public int CalculateStabilityResistance(int characterTier)
    {
        return 10 + characterTier + Power;
    }

    /// <summary>
    /// Calculates the character's Vitality Resistance
    /// </summary>
    public int CalculateVitalityResistance(int characterTier)
    {
        return 10 + characterTier + Endurance;
    }

    /// <summary>
    /// Calculates base damage bonus for attacks
    /// </summary>
    public int CalculateDamageBonus(int characterTier)
    {
        return characterTier + (int)Math.Ceiling(Power * 1.5);
    }

    /// <summary>
    /// Calculates movement speed based on mobility
    /// </summary>
    public int CalculateMovementSpeed(int characterTier)
    {
        return Math.Max(6 + Mobility, Mobility + characterTier);
    }

    /// <summary>
    /// Creates a deep copy of the combat attributes
    /// </summary>
    public CombatAttributes Clone()
    {
        return new CombatAttributes
        {
            Focus = Focus,
            Power = Power,
            Mobility = Mobility,
            Endurance = Endurance,
            CharacterId = CharacterId
        };
    }
}