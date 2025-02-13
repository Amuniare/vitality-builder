namespace VitalityBuilder.Api.Domain.Dtos.Character;

/// <summary>
/// Comprehensive character data transfer object for API responses
/// </summary>
public class CharacterResponseDto
{
    // Basic Information
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Tier { get; set; }
    public int HealthPool { get; set; }
    public int EffortRemaining { get; set; }

    // Point Pools
    public PointPoolsDto PointPools { get; set; } = new();

    // Attributes
    public CombatAttributesDto CombatAttributes { get; set; } = new();
    public UtilityAttributesDto UtilityAttributes { get; set; } = new();

    // Archetypes
    public CharacterArchetypesDto Archetypes { get; set; } = new();

    // Derived Combat Stats
    public CombatStatsDto CombatStats { get; set; } = new();

    // Collections
    public ICollection<SpecialAttackDto> SpecialAttacks { get; set; } = new List<SpecialAttackDto>();
    public ICollection<CharacterFeatureDto> Features { get; set; } = new List<CharacterFeatureDto>();
    public ICollection<CharacterExpertiseDto> Expertise { get; set; } = new List<CharacterExpertiseDto>();

    // Validation Information
    public ICollection<string> Warnings { get; set; } = new List<string>();
}

public class PointPoolsDto
{
    public int MainPool { get; set; }
    public int MainPoolSpent { get; set; }
    public int MainPoolRemaining => MainPool - MainPoolSpent;

    public int UtilityPoints { get; set; }
    public int UtilityPointsSpent { get; set; }
    public int UtilityPointsRemaining => UtilityPoints - UtilityPointsSpent;

    public int CombatAttributePoints { get; set; }
    public int CombatAttributePointsSpent { get; set; }
    public int CombatAttributePointsRemaining => CombatAttributePoints - CombatAttributePointsSpent;

    public int UtilityAttributePoints { get; set; }
    public int UtilityAttributePointsSpent { get; set; }
    public int UtilityAttributePointsRemaining => UtilityAttributePoints - UtilityAttributePointsSpent;
}

public class CombatStatsDto
{
    // Core Combat Values
    public int Avoidance { get; set; }
    public int Durability { get; set; }
    public int MovementSpeed { get; set; }
    public int Initiative { get; set; }

    // Resistances
    public int ResolveResistance { get; set; }
    public int StabilityResistance { get; set; }
    public int VitalityResistance { get; set; }

    // Combat Bonuses
    public int BaseAccuracyBonus { get; set; }
    public int BaseDamageBonus { get; set; }
    public int BaseConditionBonus { get; set; }

    // Archetype Modifications
    public int ArchetypeAccuracyModifier { get; set; }
    public int ArchetypeDamageModifier { get; set; }
    public int ArchetypeConditionModifier { get; set; }

    // Final Values (Including All Modifiers)
    public int FinalAccuracy => BaseAccuracyBonus + ArchetypeAccuracyModifier;
    public int FinalDamage => BaseDamageBonus + ArchetypeDamageModifier;
    public int FinalCondition => BaseConditionBonus + ArchetypeConditionModifier;
}

public class SpecialAttackDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string AttackType { get; set; } = string.Empty;
    public string EffectType { get; set; } = string.Empty;
    public int PointCost { get; set; }
    public ICollection<string> Limits { get; set; } = new List<string>();
    public ICollection<string> Upgrades { get; set; } = new List<string>();
}

public class CharacterFeatureDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int PointCost { get; set; }
    public string Category { get; set; } = string.Empty;
}

public class CharacterExpertiseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int PointCost { get; set; }
    public int BonusValue { get; set; }
}