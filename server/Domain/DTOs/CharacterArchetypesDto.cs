using VitalityBuilder.Api.Models.Enums;

namespace VitalityBuilder.Api.Models.DTOs;


public class CharacterArchetypesDto
{
    public MovementArchetypeDto MovementArchetype { get; set; } = null!;
    public AttackTypeArchetypeDto AttackTypeArchetype { get; set; } = null!;
    public EffectTypeArchetypeDto EffectTypeArchetype { get; set; } = null!;
    public UniqueAbilityArchetypeDto UniqueAbilityArchetype { get; set; } = null!;
    public SpecialAttackArchetypeDto SpecialAttackArchetype { get; set; } = null!;
    public UtilityArchetypeDto UtilityArchetype { get; set; } = null!;
}

public class MovementArchetypeDto
{
    public string Name { get; set; } = string.Empty;
    public MovementArchetypeType Type { get; set; }
    public Dictionary<int, int> SpeedBonusByTier { get; set; } = new();
    public bool IgnoresOpportunityAttacks { get; set; }
    public bool IgnoresDifficultTerrain { get; set; }
    public bool IsImmuneToProne { get; set; }
    public float MovementMultiplier { get; set; } = 1.0f;
}

public class AttackTypeArchetypeDto
{
    public string Name { get; set; } = string.Empty;
    public AttackTypeArchetypeCategory Category { get; set; }
    public int AccuracyPenalty { get; set; }
    public int EffectPenalty { get; set; }
    public bool BypassesAccuracyChecks { get; set; }
    public bool HasFreeAOE { get; set; }
}

public class EffectTypeArchetypeDto
{
    public string Name { get; set; } = string.Empty;
    public EffectTypeCategory Category { get; set; }
    public bool HasAccessToAdvancedConditions { get; set; }
    public int DamagePenalty { get; set; }
    public int ConditionPenalty { get; set; }
    public bool RequiresHybridEffects { get; set; }
}

public class UniqueAbilityArchetypeDto
{
    public string Name { get; set; } = string.Empty;
    public UniqueAbilityCategory Category { get; set; }
    public int ExtraQuickActions { get; set; }
    public int ExtraPointPool { get; set; }
    public Dictionary<string, int> StatBonuses { get; set; } = new();
}

public class SpecialAttackArchetypeDto
{
    public string Name { get; set; } = string.Empty;
    public SpecialAttackCategory Category { get; set; }
    public int BasePoints { get; set; }
    public int MaxSpecialAttacks { get; set; }
    public float LimitPointMultiplier { get; set; }
    public bool CanTakeLimits { get; set; }
    public List<string> RequiredLimits { get; set; } = new();
}

public class UtilityArchetypeDto
{
    public string Name { get; set; } = string.Empty;
    public UtilityCategory Category { get; set; }
    public int BaseUtilityPool { get; set; }
    public bool CanPurchaseExpertise { get; set; }
    public float TierBonusMultiplier { get; set; }
    public List<string> Restrictions { get; set; } = new();
}