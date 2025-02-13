using System.ComponentModel.DataAnnotations;
using VitalityBuilder.Api.Domain.Character;

namespace VitalityBuilder.Api.Domain.Attributes;

public class UtilityAttributes
{
    public int Id { get; set; }

    public int CharacterId { get; set; }
    public Domain.Character.Character character { get; set; } = null!;

    [Range(0, 10)]
    public int Awareness { get; set; }

    [Range(0, 10)]
    public int Communication { get; set; }

    [Range(0, 10)]
    public int Intelligence { get; set; }

    /// <summary>
    /// Gets the total number of points spent on utility attributes
    /// </summary>
    public int TotalPoints => Awareness + Communication + Intelligence;

    /// <summary>
    /// Validates if the attributes are within the character's tier limits
    /// </summary>
    public bool ValidateAgainstTier(int characterTier)
    {
        // Individual attributes cannot exceed character tier
        if (Awareness > characterTier || Communication > characterTier || 
            Intelligence > characterTier)
        {
            return false;
        }

        // Total points cannot exceed tier
        if (TotalPoints > characterTier)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Calculates Initiative bonus based on Awareness
    /// </summary>
    public int CalculateInitiativeBonus()
    {
        return Awareness;
    }

    /// <summary>
    /// Calculates the number of Dynamic Entry allies that can be affected
    /// Based on Intelligence score, minimum of 1
    /// </summary>
    public int CalculateDynamicEntryAllies()
    {
        return Math.Max(1, Intelligence);
    }

    /// <summary>
    /// Calculates the number of Dynamic Entry spaces
    /// Based on Communication score, minimum of 1
    /// </summary>
    public int CalculateDynamicEntrySpaces()
    {
        return Math.Max(1, Communication);
    }

    /// <summary>
    /// Performs a standard skill check for the given attribute
    /// </summary>
    /// <param name="attributeName">The name of the attribute to check</param>
    /// <param name="expertiseBonus">Any additional expertise bonuses</param>
    /// <returns>The total skill check result</returns>
    public (int Total, int[] DiceRolls) PerformSkillCheck(
        string attributeName, 
        int expertiseBonus = 0)
    {
        // Roll 3d6 for skill checks
        var random = new Random();
        var diceRolls = new int[3];
        var total = 0;

        // Roll dice and handle exploding 6s
        for (int i = 0; i < 3; i++)
        {
            var roll = random.Next(1, 7);
            diceRolls[i] = roll;
            total += roll;

            // Handle exploding 6s
            while (roll == 6)
            {
                roll = random.Next(1, 7);
                total += roll;
            }
        }

        // Add attribute bonus
        var attributeBonus = attributeName.ToLower() switch
        {
            "awareness" => Awareness,
            "communication" => Communication,
            "intelligence" => Intelligence,
            _ => throw new ArgumentException($"Unknown attribute: {attributeName}")
        };

        total += attributeBonus + expertiseBonus;

        return (total, diceRolls);
    }

    /// <summary>
    /// Creates a deep copy of the utility attributes
    /// </summary>
    public UtilityAttributes Clone()
    {
        return new UtilityAttributes
        {
            Awareness = Awareness,
            Communication = Communication,
            Intelligence = Intelligence,
            CharacterId = CharacterId
        };
    }
}