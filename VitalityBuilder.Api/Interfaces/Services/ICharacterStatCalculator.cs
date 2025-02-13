using VitalityBuilder.Api.Domain.Character;
using VitalityBuilder.Api.Domain.ValueObjects;

namespace VitalityBuilder.Api.Interfaces.Services;

/// <summary>
/// Interface for calculating character statistics and derived values
/// </summary>
public interface ICharacterStatCalculator
{
    /// <summary>
    /// Calculates all combat statistics for a character
    /// </summary>
    CombatStats CalculateAllStats(Character character);

    /// <summary>
    /// Calculates base attack values without archetype modifiers
    /// </summary>
    Task<AttackValues> CalculateBaseAttackValuesAsync(Character character);

    /// <summary>
    /// Calculates modified attack values including archetype effects
    /// </summary>
    Task<AttackValues> CalculateModifiedAttackValuesAsync(
        Character character,
        bool includeTemporaryEffects = false);

    /// <summary>
    /// Calculates defensive values
    /// </summary>
    Task<DefenseValues> CalculateDefenseValuesAsync(Character character);

    /// <summary>
    /// Calculates resistance values against conditions
    /// </summary>
    Task<ResistanceValues> CalculateResistanceValuesAsync(Character character);

    /// <summary>
    /// Calculates movement capabilities
    /// </summary>
    Task<MovementValues> CalculateMovementValuesAsync(Character character);

    /// <summary>
    /// Calculates all initiative-related values
    /// </summary>
    Task<InitiativeValues> CalculateInitiativeValuesAsync(Character character);

    /// <summary>
    /// Calculates derived utility values
    /// </summary>
    Task<UtilityValues> CalculateUtilityValuesAsync(Character character);

    /// <summary>
    /// Validates that all calculated values are within legal limits
    /// </summary>
    Task<ValidationResult> ValidateCalculatedValuesAsync(Character character);
}

public class AttackValues
{
    public int BaseAccuracyBonus { get; set; }
    public int BaseDamageBonus { get; set; }
    public int BaseConditionBonus { get; set; }
    public double CriticalHitChance { get; set; }
    public int SpecialAttackModifier { get; set; }
    public Dictionary<string, int> TypeSpecificBonuses { get; set; } = new();
}

public class DefenseValues
{
    public int BaseAvoidance { get; set; }
    public int ModifiedAvoidance { get; set; }
    public int BaseDurability { get; set; }
    public int ModifiedDurability { get; set; }
    public int DamageReduction { get; set; }
    public Dictionary<string, int> TypeSpecificResistances { get; set; } = new();
}

public class ResistanceValues
{
    public int ResolveResistance { get; set; }
    public int StabilityResistance { get; set; }
    public int VitalityResistance { get; set; }
    public Dictionary<string, int> ConditionImmunities { get; set; } = new();
    public Dictionary<string, double> ResistanceMultipliers { get; set; } = new();
}

public class MovementValues
{
    public int BaseMovementSpeed { get; set; }
    public int ModifiedMovementSpeed { get; set; }
    public bool IgnoresDifficultTerrain { get; set; }
    public bool IgnoresOpportunityAttacks { get; set; }
    public bool CanFly { get; set; }
    public bool CanTeleport { get; set; }
    public int JumpDistance { get; set; }
    public Dictionary<string, double> TerrainMultipliers { get; set; } = new();
}

public class InitiativeValues
{
    public int BaseInitiative { get; set; }
    public int ModifiedInitiative { get; set; }
    public bool AdvantageOnInitiative { get; set; }
    public int BonusToAllies { get; set; }
    public Dictionary<string, int> SituationalBonuses { get; set; } = new();
}

public class UtilityValues
{
    public int DynamicEntryAllies { get; set; }
    public int DynamicEntrySpaces { get; set; }
    public Dictionary<string, int> SkillBonuses { get; set; } = new();
    public Dictionary<string, int> ExpertiseBonuses { get; set; } = new();
    public List<string> SpecialSenses { get; set; } = new();
}