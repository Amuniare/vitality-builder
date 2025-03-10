using VitalityBuilder.Api.Domain.Character;
using VitalityBuilder.Api.Domain.Constants;
using VitalityBuilder.Api.Domain.ValueObjects;
using VitalityBuilder.Api.Interfaces.Services;

namespace VitalityBuilder.Api.Services.Calculations;

public class CharacterStatCalculator : ICharacterStatCalculator
{
    
    
    public CombatStats CalculateAllStats(Domain.Character.Character character)
    {
        var stats = new CombatStats
        {
            // Base Defense Values
            Avoidance = CalculateAvoidance(character),
            Durability = CalculateDurability(character),
            MovementSpeed = CalculateMovementSpeed(character),

            // Core Combat Values
            Initiative = CalculateInitiative(character),
            BaseAccuracyBonus = CalculateBaseAccuracy(character),
            BaseDamageBonus = CalculateBaseDamage(character),
            BaseConditionBonus = CalculateBaseCondition(character),

            // Resistances
            ResolveResistance = CalculateResolveResistance(character),
            StabilityResistance = CalculateStabilityResistance(character),
            VitalityResistance = CalculateVitalityResistance(character)
        };

        // Apply archetype modifiers
        ApplyArchetypeModifiers(stats, character);

        return stats;
    }

    private int CalculateAvoidance(Domain.Character.Character character)
    {
        return GameRuleConstants.BaseResistanceValue +
               character.Tier +
               character.CombatAttributes.Mobility;
    }

    private int CalculateDurability(Domain.Character.Character character)
    {
        return character.Tier +
               (int)Math.Ceiling(character.CombatAttributes.Endurance * 
                                GameRuleConstants.EnduranceMultiplier);
    }

    private int CalculateMovementSpeed(Domain.Character.Character character)
    {
        // Calculate base movement
        var baseSpeed = Math.Max(
            GameRuleConstants.BaseMovementSpeed + character.CombatAttributes.Mobility,
            character.CombatAttributes.Mobility + character.Tier
        );

        // Apply archetype modifiers
        return character.Archetypes.ApplyMovementModifiers(
            baseSpeed, 
            character.Tier, 
            character.CombatAttributes.Endurance
        );
    }

    private int CalculateInitiative(Domain.Character.Character character)
    {
        return character.CombatAttributes.Focus +
               character.UtilityAttributes.Awareness;
    }

    private int CalculateBaseAccuracy(Domain.Character.Character character)
    {
        var baseAccuracy = character.Tier + character.CombatAttributes.Focus;
        return character.Archetypes.ApplyAttackModifiers(baseAccuracy, character.Tier);
    }

    private int CalculateBaseDamage(Domain.Character.Character character)
    {
        var baseDamage = character.Tier + 
                        (int)Math.Ceiling(character.CombatAttributes.Power * 
                                        GameRuleConstants.EnduranceMultiplier);
        
        var (modifiedDamage, _) = character.Archetypes.ApplyEffectModifiers(
            baseDamage, 
            0, 
            character.Tier
        );
        
        return modifiedDamage;
    }

    private int CalculateBaseCondition(Domain.Character.Character character)
    {
        var baseCondition = character.Tier * 2;
        var (_, modifiedCondition) = character.Archetypes.ApplyEffectModifiers(
            0, 
            baseCondition, 
            character.Tier
        );
        
        return modifiedCondition;
    }

    private int CalculateResolveResistance(Domain.Character.Character character)
    {
        return GameRuleConstants.BaseResistanceValue +
               character.Tier +
               character.CombatAttributes.Focus;
    }

    private int CalculateStabilityResistance(Domain.Character.Character character)
    {
        return GameRuleConstants.BaseResistanceValue +
               character.Tier +
               character.CombatAttributes.Power;
    }

    private int CalculateVitalityResistance(Domain.Character.Character character)
    {
        return GameRuleConstants.BaseResistanceValue +
               character.Tier +
               character.CombatAttributes.Endurance;
    }

    private void ApplyArchetypeModifiers(CombatStats stats, Domain.Character.Character character)
    {
        // Apply attack archetype effects
        if (character.Archetypes.AttackType == AttackArchetype.AOESpecialist)
        {
            stats.BaseAccuracyBonus -= character.Tier;
        }
        else if (character.Archetypes.AttackType == AttackArchetype.DirectSpecialist)
        {
            stats.BaseConditionBonus -= character.Tier;
        }

        // Apply effect archetype modifiers
        if (character.Archetypes.EffectType == EffectArchetype.HybridSpecialist)
        {
            stats.BaseDamageBonus -= character.Tier;
            stats.BaseConditionBonus -= character.Tier;
        }
        else if (character.Archetypes.EffectType == EffectArchetype.CrowdControl)
        {
            stats.BaseDamageBonus -= character.Tier;
        }

        // Apply movement archetype effects
        switch (character.Archetypes.MovementType)
        {
            case MovementArchetype.Swift:
                stats.MovementSpeed += (int)Math.Ceiling(character.Tier / 2.0);
                break;
            case MovementArchetype.Vanguard:
                stats.MovementSpeed += character.CombatAttributes.Endurance;
                break;
            case MovementArchetype.Portal:
                stats.MovementSpeed -= 2;
                break;
        }

        // Apply unique ability effects
        if (character.Archetypes.UniqueAbility == UniqueAbilityArchetype.CutAbove)
        {
            var bonus = character.Tier switch
            {
                <= 4 => 1,
                <= 7 => 2,
                _ => 3
            };
            
            stats.BaseAccuracyBonus += bonus;
            stats.BaseDamageBonus += bonus;
            stats.BaseConditionBonus += bonus;
            stats.Avoidance += bonus;
            stats.Durability += bonus;
            stats.MovementSpeed += bonus;
            stats.ResolveResistance += bonus;
            stats.StabilityResistance += bonus;
            stats.VitalityResistance += bonus;
        }
    }

    public Task CalculateAllStats(Domain.Character.Character character)
    {
        throw new NotImplementedException();
    }

    public Task CalculateBaseAttackValuesAsync(Domain.Character.Character character)
    {
        throw new NotImplementedException();
    }

    public Task CalculateModifiedAttackValuesAsync(Domain.Character.Character character, bool isBoosted)
    {
        throw new NotImplementedException();
    }

    
}