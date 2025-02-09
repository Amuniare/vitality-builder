using VitalityBuilder.Api.Models.Archetypes;
using VitalityBuilder.Api.Models.DTOs;

namespace VitalityBuilder.Api.Services;

public partial class CharacterArchetypesService 
{
    private static MovementArchetype MapMovementArchetype(MovementArchetypeDto dto) => 
        new()
        {
            Name = dto.Name,
            Type = (MovementArchetypeType)dto.Type,
            SpeedBonusByTier = dto.SpeedBonusByTier,
            IgnoresOpportunityAttacks = dto.IgnoresOpportunityAttacks,
            IgnoresDifficultTerrain = dto.IgnoresDifficultTerrain,
            IsImmuneToProne = dto.IsImmuneToProne,
            MovementMultiplier = dto.MovementMultiplier
        };

    private static AttackTypeArchetype MapAttackTypeArchetype(AttackTypeArchetypeDto dto) => 
        new()
        {
            Name = dto.Name,
            Category = (AttackTypeArchetypeCategory)dto.Category,
            AccuracyPenalty = dto.AccuracyPenalty,
            EffectPenalty = dto.EffectPenalty,
            BypassesAccuracyChecks = dto.BypassesAccuracyChecks,
            HasFreeAOE = dto.HasFreeAOE
        };

    private static EffectTypeArchetype MapEffectTypeArchetype(EffectTypeArchetypeDto dto) =>
        new()
        {
            Name = dto.Name,
            Category = (EffectTypeCategory)dto.Category,
            HasAccessToAdvancedConditions = dto.HasAccessToAdvancedConditions,
            DamagePenalty = dto.DamagePenalty,
            ConditionPenalty = dto.ConditionPenalty,
            RequiresHybridEffects = dto.RequiresHybridEffects
        };

    private static UniqueAbilityArchetype MapUniqueAbilityArchetype(UniqueAbilityArchetypeDto dto) =>
        new()
        {
            Name = dto.Name,
            Category = (UniqueAbilityCategory)dto.Category,
            ExtraQuickActions = dto.ExtraQuickActions,
            ExtraPointPool = dto.ExtraPointPool,
            StatBonuses = dto.StatBonuses
        };

    private static SpecialAttackArchetype MapSpecialAttackArchetype(SpecialAttackArchetypeDto dto) =>
        new()
        {
            Name = dto.Name,
            Category = (SpecialAttackCategory)dto.Category,
            BasePoints = dto.BasePoints,
            MaxSpecialAttacks = dto.MaxSpecialAttacks,
            LimitPointMultiplier = dto.LimitPointMultiplier,
            CanTakeLimits = dto.CanTakeLimits,
            RequiredLimits = dto.RequiredLimits
        };

    private static UtilityArchetype MapUtilityArchetype(UtilityArchetypeDto dto) =>
        new()
        {
            Name = dto.Name,
            Category = (UtilityCategory)dto.Category,
            BaseUtilityPool = dto.BaseUtilityPool,
            CanPurchaseExpertise = dto.CanPurchaseExpertise,
            TierBonusMultiplier = dto.TierBonusMultiplier,
            Restrictions = dto.Restrictions
        };
}
