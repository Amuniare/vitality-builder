using System.ComponentModel.DataAnnotations;

namespace VitalityBuilder.Domain.Dtos.Archetypes;

/// <summary>
/// Data transfer object for character archetype selections
/// </summary>
public class CharacterArchetypesDto
{
    [Required(ErrorMessage = "Movement archetype is required")]
    public string MovementType { get; set; } = string.Empty;

    [Required(ErrorMessage = "Attack archetype is required")]
    public string AttackType { get; set; } = string.Empty;

    [Required(ErrorMessage = "Effect archetype is required")]
    public string EffectType { get; set; } = string.Empty;

    [Required(ErrorMessage = "Unique ability archetype is required")]
    public string UniqueAbility { get; set; } = string.Empty;

    [Required(ErrorMessage = "Special attack archetype is required")]
    public string SpecialAttack { get; set; } = string.Empty;

    [Required(ErrorMessage = "Utility archetype is required")]
    public string UtilityType { get; set; } = string.Empty;

    // Calculated modifiers based on archetype combinations
    public ArchetypeModifiersDto Modifiers { get; set; } = new();

    // Point calculations for special attacks
    public ArchetypePointsDto Points { get; set; } = new();

    // Validation state
    public ArchetypeValidationDto Validation { get; set; } = new();
}

/// <summary>
/// Contains all archetype-based modifiers for character calculations
/// </summary>
public class ArchetypeModifiersDto
{
    // Movement Modifiers
    public int MovementSpeedModifier { get; set; }
    public bool IgnoreOpportunityAttacks { get; set; }
    public bool IgnoreTerrainPenalties { get; set; }
    public bool ImmuneToProne { get; set; }
    public int AttackReachBonus { get; set; }

    // Attack Modifiers
    public int AccuracyModifier { get; set; }
    public bool BypassAccuracyChecks { get; set; }
    public bool HasFreeAOE { get; set; }

    // Effect Modifiers
    public int DamageModifier { get; set; }
    public int ConditionModifier { get; set; }
    public bool HasFreeConditions { get; set; }

    // Action Modifiers
    public int ExtraQuickActions { get; set; }
    public int QuickActionConversionCost { get; set; }

    // Point Modifiers
    public double MainPoolMultiplier { get; set; } = 1.0;
    public double UtilityPoolMultiplier { get; set; } = 1.0;
}

/// <summary>
/// Contains point calculations specific to archetype selections
/// </summary>
public class ArchetypePointsDto
{
    public int SpecialAttackBasePoints { get; set; }
    public int SpecialAttackLimit { get; set; }
    public double LimitPointMultiplier { get; set; }
    public bool CanTakeLimits { get; set; }
    public int SharedUses { get; set; }
}

/// <summary>
/// Contains validation information for archetype selections
/// </summary>
public class ArchetypeValidationDto
{
    public bool IsValid { get; set; }
    public ICollection<string> Errors { get; set; } = new List<string>();
    public ICollection<string> Warnings { get; set; } = new List<string>();
    public ICollection<string> Incompatibilities { get; set; } = new List<string>();
}