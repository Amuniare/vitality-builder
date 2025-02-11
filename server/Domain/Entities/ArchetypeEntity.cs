using System.ComponentModel.DataAnnotations;
using VitalityBuilder.Api.Models.Archetypes;
using VitalityBuilder.Api.Models.Enums;

namespace VitalityBuilder.Api.Models.Entities;

public class CharacterArchetypesEntity
{
    public int Id { get; set; }
    public MovementArchetype MovementArchetype { get; set; } = null!;
    public AttackTypeArchetype AttackTypeArchetype { get; set; } = null!;
    public EffectTypeArchetype EffectTypeArchetype { get; set; } = null!;
    public UniqueAbilityArchetype UniqueAbilityArchetype { get; set; } = null!;
    public SpecialAttackArchetype SpecialAttackArchetype { get; set; } = null!;
    public UtilityArchetype UtilityArchetype { get; set; } = null!;
    
    public int CharacterId { get; set; }
    public CharacterArchetypesEntity CharacterEntity { get; set; } = null!;
}

public class MovementArchetypeEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public MovementArchetypeType Type { get; set; }
    public Dictionary<int, int> SpeedBonusByTier { get; set; } = new();
    public bool IgnoresOpportunityAttacks { get; set; }
    public bool IgnoresDifficultTerrain { get; set; }
    public bool IsImmuneToProne { get; set; }
    public float MovementMultiplier { get; set; } = 1.0f;
    
    public int CharacterArchetypesId { get; set; }
    public CharacterArchetypesEntity CharacterArchetypesEntity { get; set; } = null!;
}


public class AttackTypeArchetypeEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public AttackTypeArchetypeCategory Category { get; set; }
    public int AccuracyPenalty { get; set; }
    public int EffectPenalty { get; set; }
    public bool BypassesAccuracyChecks { get; set; }
    public bool HasFreeAOE { get; set; }
    
    public int CharacterArchetypesId { get; set; }
    public CharacterArchetypesEntity CharacterArchetypesEntity { get; set; } = null!;
}



public class EffectTypeArchetypeEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public EffectTypeCategory Category { get; set; }
    public bool HasAccessToAdvancedConditions { get; set; }
    public int DamagePenalty { get; set; }
    public int ConditionPenalty { get; set; }
    public bool RequiresHybridEffects { get; set; }
    
    public int CharacterArchetypesId { get; set; }
    public CharacterArchetypesEntity CharacterArchetypesEntity { get; set; } = null!;
}



public class UniqueAbilityArchetypeEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public UniqueAbilityCategory Category { get; set; }
    public int ExtraQuickActions { get; set; }
    public int ExtraPointPool { get; set; }
    public Dictionary<string, int> StatBonuses { get; set; } = new();
    
    public int CharacterArchetypesId { get; set; }
    public CharacterArchetypesEntity CharacterArchetypesEntity { get; set; } = null!;
}



public class SpecialAttackArchetypeEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public SpecialAttackCategory Category { get; set; }
    public int BasePoints { get; set; }
    public int MaxSpecialAttacks { get; set; }
    public float LimitPointMultiplier { get; set; }
    public bool CanTakeLimits { get; set; }
    public List<string> RequiredLimits { get; set; } = new();
    
    public int CharacterArchetypesId { get; set; }
    public CharacterArchetypesEntity CharacterArchetypesEntity { get; set; } = null!;
}



public class UtilityArchetypeEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public UtilityCategory Category { get; set; }
    public int BaseUtilityPool { get; set; }
    public bool CanPurchaseExpertise { get; set; }
    public float TierBonusMultiplier { get; set; }
    public List<string> Restrictions { get; set; } = new();
    
    public int CharacterArchetypesId { get; set; }
    public CharacterArchetypesEntity CharacterArchetypesEntity { get; set; } = null!;
}
