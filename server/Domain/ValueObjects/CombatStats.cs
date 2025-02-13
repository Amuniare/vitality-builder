using VitalityBuilder.Domain.Constants;

namespace VitalityBuilder.Domain.ValueObjects;

/// <summary>
/// Represents derived combat statistics for a character
/// </summary>
public class CombatStats
{
    // Core Defense Values
    public int Avoidance { get; set; }
    public int Durability { get; set; }
    public int MovementSpeed { get; set; }

    // Combat Values
    public int Initiative { get; set; }
    public int BaseAccuracyBonus { get; set; }
    public int BaseDamageBonus { get; set; }
    public int BaseConditionBonus { get; set; }

    // Resistances
    public int ResolveResistance { get; set; }
    public int StabilityResistance { get; set; }
    public int VitalityResistance { get; set; }

    // Status Tracking
    public bool IsExhausted { get; set; }
    public bool IsStealthed { get; set; }
    public bool IsProne { get; set; }

    // Movement Calculations
    public int GetEffectiveMovementSpeed()
    {
        var speed = MovementSpeed;

        if (IsExhausted)
        {
            speed /= 2;
        }

        return Math.Max(1, speed); // Minimum 1 space of movement
    }

    // Attack Modifications
    public int GetAccuracyForAttack(bool isMelee, bool targetIsProne)
    {
        var accuracy = BaseAccuracyBonus;

        if (targetIsProne)
        {
            if (isMelee)
            {
                accuracy += GameRuleConstants.CombatConditions.AdjacentAttackBonus;
            }
            else
            {
                accuracy += GameRuleConstants.CombatConditions.RangedAttackPenalty;
            }
        }

        return accuracy;
    }

    // Defense Calculations
    public int GetEffectiveAvoidance(bool isBeingFlanked)
    {
        var avoidance = Avoidance;

        if (IsProne)
        {
            avoidance -= 2;
        }

        if (isBeingFlanked)
        {
            avoidance -= 2;
        }

        return Math.Max(5, avoidance); // Minimum avoidance of 5
    }

    // Damage Reduction
    public int ReduceDamage(int incomingDamage)
    {
        return Math.Max(0, incomingDamage - Durability);
    }

    // Condition Resistance
    public int GetResistanceForType(string resistanceType)
    {
        return resistanceType.ToLower() switch
        {
            "resolve" => ResolveResistance,
            "stability" => StabilityResistance,
            "vitality" => VitalityResistance,
            _ => throw new ArgumentException($"Unknown resistance type: {resistanceType}")
        };
    }

    // Clone Method
    public CombatStats Clone()
    {
        return new CombatStats
        {
            Avoidance = Avoidance,
            Durability = Durability,
            MovementSpeed = MovementSpeed,
            Initiative = Initiative,
            BaseAccuracyBonus = BaseAccuracyBonus,
            BaseDamageBonus = BaseDamageBonus,
            BaseConditionBonus = BaseConditionBonus,
            ResolveResistance = ResolveResistance,
            StabilityResistance = StabilityResistance,
            VitalityResistance = VitalityResistance,
            IsExhausted = IsExhausted,
            IsStealthed = IsStealthed,
            IsProne = IsProne
        };
    }
}