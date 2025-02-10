using VitalityBuilder.Api.Models;
using VitalityBuilder.Api.Models.Archetypes;

namespace VitalityBuilder.Api.Services;

public partial class CharacterArchetypesService
{
    public int CalculateMovementSpeed(Models.Archetypes.MovementArchetype archetype, int tier, int mobility)
    {
        // Base movement is the higher of (6) or (mobility + tier)
        var baseSpeed = Math.Max(6, mobility + tier);
        
        // Apply archetype-specific modifiers
        switch (archetype.Type)
        {
            case Models.Archetypes.MovementArchetypeType.Swift:
                // Swift characters get bonus speed based on tier level
                baseSpeed += (tier + 1) / 2;
                break;
            case Models.Archetypes.MovementArchetypeType.Flight:
            case Models.Archetypes.MovementArchetypeType.Teleportation:
            case Models.Archetypes.MovementArchetypeType.Portal:
                baseSpeed = AdjustSpecialMovementSpeedForType(archetype.Type, baseSpeed);
                break;
        }

        // Apply any tier-specific bonuses configured for this archetype
        if (archetype.SpeedBonusByTier.TryGetValue(tier, out int bonus))
        {
            baseSpeed += bonus;
        }

        // Apply movement multiplier (e.g., for effects like swimming)
        return (int)(baseSpeed * archetype.MovementMultiplier);
    }

    private int AdjustSpecialMovementSpeedForType(Models.Archetypes.MovementArchetypeType type, int baseSpeed)
    {
        // Implement the logic for adjusting special movement speed here
        // For now, let's just return the baseSpeed as a placeholder
        return baseSpeed;
    }

    public int CalculateSpecialAttackPoints(SpecialAttackArchetype archetype, int tier)
    {
        return archetype.Category switch
        {
            SpecialAttackCategory.Paragon => 10 * tier,
            SpecialAttackCategory.OneTrick => 20 * tier,
            SpecialAttackCategory.DualNatured => 15 * tier,
            SpecialAttackCategory.Basic => 10 * tier,
            SpecialAttackCategory.Specialist => CalculateSpecialistPoints(tier, archetype.RequiredLimits),
            _ => 0
        };
    }

    public int CalculateUtilityPoints(UtilityArchetype archetype, int tier)
    {
        return archetype.Category switch
        {
            UtilityCategory.Specialized => 5 * (tier - 2),
            UtilityCategory.Practical => 5 * (tier - 1),
            UtilityCategory.JackOfAllTrades => 5 * (tier - 2),
            _ => 0
        };
    }


    public Dictionary<string, int> CalculateArchetypeBonuses(Models.Archetypes.CharacterArchetypes archetypes, int tier)
    {
        var bonuses = new Dictionary<string, int>();

        // Add movement archetype bonuses
        if (archetypes.MovementArchetype.Type == Models.Archetypes.MovementArchetypeType.Swift)
        {
            bonuses["MovementSpeed"] = (tier + 1) / 2;
        }

        // Add attack type bonuses/penalties
        if (archetypes.AttackTypeArchetype.Category == Models.Archetypes.AttackTypeArchetypeCategory.AOESpecialist)
        {
            bonuses["AOEAccuracy"] = -tier;
        }
        else if (archetypes.AttackTypeArchetype.Category == Models.Archetypes.AttackTypeArchetypeCategory.DirectSpecialist)
        {
            bonuses["DirectEffect"] = -tier;
        }

        // Add effect type bonuses/penalties
        if (archetypes.EffectTypeArchetype.Category == Models.Archetypes.EffectTypeCategory.HybridSpecialist)
        {
            bonuses["DamageRoll"] = -tier;
            bonuses["ConditionRoll"] = -tier;
        }
        else if (archetypes.EffectTypeArchetype.Category == Models.Archetypes.EffectTypeCategory.CrowdControl)
        {
            bonuses["DamageRoll"] = -tier;
        }

        // Add Cut Above archetype bonuses
        if (archetypes.UniqueAbilityArchetype.Category == Models.Archetypes.UniqueAbilityCategory.CutAbove)
        {
            var bonus = tier switch
            {
                <= 4 => 1,
                <= 7 => 2,
                _ => 3
            };
            
            bonuses["AllStats"] = bonus;
        }

        return bonuses;
    }
}