using System.ComponentModel.DataAnnotations;
using VitalityBuilder.Api.Domain.Attributes;
using VitalityBuilder.Api.Domain.ValueObjects;

namespace VitalityBuilder.Api.Domain.Character;

public class Character
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(2, 10)]
    public int Tier { get; set; }

    // Core attributes
    public CombatAttributes CombatAttributes { get; set; } = null!;
    public UtilityAttributes UtilityAttributes { get; set; } = null!;
    public CharacterArchetypes Archetypes { get; set; } = null!;

    // Base stats
    public int HealthPool { get; private set; } = 100;
    public int EffortUses { get; private set; } = 2;

    // Point pools
    public int MainPool => (Tier - 2) * 15;
    public int UtilityPoints => 5 * (Tier - 1);
    public int CombatAttributePoints => Tier * 2;
    public int UtilityAttributePoints => Tier;

    // Navigation properties
    public ICollection<SpecialAttack> SpecialAttacks { get; set; } = new List<SpecialAttack>();
    public ICollection<CharacterFeature> Features { get; set; } = new List<CharacterFeature>();
    public ICollection<CharacterExpertise> Expertise { get; set; } = new List<CharacterExpertise>();

    // Point tracking
    public int SpentMainPoints { get; private set; }
    public int SpentUtilityPoints { get; private set; }

    /// <summary>
    /// Calculates derived combat statistics based on current attributes and tier
    /// </summary>
    public CombatStats CalculateCombatStats()
    {
        return new CombatStats
        {
            Avoidance = CombatAttributes.CalculateAvoidance(Tier),
            Durability = CombatAttributes.CalculateDurability(Tier),
            ResolveResistance = CombatAttributes.CalculateResolveResistance(Tier),
            StabilityResistance = CombatAttributes.CalculateStabilityResistance(Tier),
            VitalityResistance = CombatAttributes.CalculateVitalityResistance(Tier),
            MovementSpeed = CalculateMovementSpeed(),
            Initiative = CalculateInitiative()
        };
    }

    /// <summary>
    /// Calculates total movement speed including archetype modifiers
    /// </summary>
    public int CalculateMovementSpeed()
    {
        var baseSpeed = CombatAttributes.CalculateMovementSpeed(Tier);
        return Archetypes.ApplyMovementModifiers(baseSpeed, Tier, CombatAttributes.Endurance);
    }

    /// <summary>
    /// Calculates initiative bonus including all modifiers
    /// </summary>
    public int CalculateInitiative()
    {
        return CombatAttributes.Focus + UtilityAttributes.Awareness;
    }

    /// <summary>
    /// Spends points from the main pool
    /// </summary>
    /// <returns>True if points were successfully spent</returns>
    public bool SpendMainPoints(int amount)
    {
        if (SpentMainPoints + amount > MainPool)
        {
            return false;
        }

        SpentMainPoints += amount;
        return true;
    }

    /// <summary>
    /// Spends points from the utility pool
    /// </summary>
    /// <returns>True if points were successfully spent</returns>
    public bool SpendUtilityPoints(int amount)
    {
        if (SpentUtilityPoints + amount > UtilityPoints)
        {
            return false;
        }

        SpentUtilityPoints += amount;
        return true;
    }

    /// <summary>
    /// Uses an effort and returns success status
    /// </summary>
    public bool UseEffort()
    {
        if (EffortUses <= 0)
        {
            return false;
        }

        EffortUses--;
        return true;
    }

    /// <summary>
    /// Resets effort uses after a rest
    /// </summary>
    public void ResetEffort()
    {
        EffortUses = 2;
    }

    /// <summary>
    /// Validates core combat attribute distribution
    /// </summary>
    public bool ValidateCombatAttributes()
    {
        if (CombatAttributes == null)
        {
            return false;
        }

        return CombatAttributes.ValidateAgainstTier(Tier);
    }

    /// <summary>
    /// Validates core utility attribute distribution
    /// </summary>
    public bool ValidateUtilityAttributes()
    {
        if (UtilityAttributes == null)
        {
            return false;
        }

        return UtilityAttributes.ValidateAgainstTier(Tier);
    }

    /// <summary>
    /// Validates that archetypes are properly selected and compatible
    /// </summary>
    public bool ValidateArchetypes()
    {
        if (Archetypes == null)
        {
            return false;
        }

        return Archetypes.ValidateArchetypes();
    }

    /// <summary>
    /// Performs a survival check when reaching 0 health
    /// </summary>
    /// <param name="excessDamage">Amount of damage that exceeded current health</param>
    /// <returns>True if the character survives</returns>
    public (bool Survived, bool Critical) PerformSurvivalCheck(int excessDamage)
    {
        var random = new Random();
        var roll = random.Next(1, 21); // d20 roll

        if (roll == 20) // Natural 20 always succeeds
        {
            return (true, true);
        }

        // Add Endurance to roll for characters with high vitality
        roll += CombatAttributes.Endurance;

        return (roll >= excessDamage, false);
    }

    /// <summary>
    /// Creates a deep copy of the character
    /// </summary>
    public Character Clone()
    {
        var clone = new Character
        {
            Name = Name,
            Tier = Tier,
            HealthPool = HealthPool,
            EffortUses = EffortUses,
            SpentMainPoints = SpentMainPoints,
            SpentUtilityPoints = SpentUtilityPoints,
            CombatAttributes = CombatAttributes.Clone(),
            UtilityAttributes = UtilityAttributes.Clone(),
            Archetypes = Archetypes.Clone()
        };

        foreach (var attack in SpecialAttacks)
        {
            clone.SpecialAttacks.Add(attack.Clone());
        }

        foreach (var feature in Features)
        {
            clone.Features.Add(feature.Clone());
        }

        foreach (var expertise in Expertise)
        {
            clone.Expertise.Add(expertise.Clone());
        }

        return clone;
    }
}