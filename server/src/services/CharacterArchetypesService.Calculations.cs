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

    public int CalculateSpecialAttackPoints(Models.Archetypes.SpecialAttackArchetype archetype, int tier)
    {
        return archetype.Category switch
        {
            // Normal gets points based on limits
            Models.Archetypes.SpecialAttackCategory.Normal => CalculateNormalArchetypePoints(tier),
            
            // Paragon gets 10 × Tier points but can't take limits
            Models.Archetypes.SpecialAttackCategory.Paragon => 10 * tier,
            
            // One Trick gets 20 × Tier points for a single powerful attack
            Models.Archetypes.SpecialAttackCategory.OneTrick => 20 * tier,
            
            // Dual-Natured gets 15 × Tier points for two balanced attacks
            Models.Archetypes.SpecialAttackCategory.DualNatured => 15 * tier,
            
            // Basic gets 10 × Tier points for enhancing base attacks
            Models.Archetypes.SpecialAttackCategory.Basic => 10 * tier,
            
            // Specialist gains enhanced points from required limits
            Models.Archetypes.SpecialAttackCategory.Specialist => CalculateSpecialistPoints(tier, archetype.RequiredLimits),
            
            _ => 0
        };
    }

    public int CalculateUtilityPoints(Models.Archetypes.UtilityArchetype archetype, int tier)
    {
        return archetype.Category switch
        {
            // Standard points: 5 × (Tier - 1)
            Models.Archetypes.UtilityCategory.Practical => 5 * (tier - 1),
            
            // Specialized gets double tier bonus but restricted options
            Models.Archetypes.UtilityCategory.Specialized => 5 * (tier - 2),
            
            // Jack of All Trades gets fewer points but applies tier to all checks
            Models.Archetypes.UtilityCategory.JackOfAllTrades => 5 * (tier - 2),
            
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