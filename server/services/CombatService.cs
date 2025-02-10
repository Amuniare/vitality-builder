using System;
using Microsoft.Extensions.Logging;

namespace VitalityBuilder.Api.Services;

/// <summary>
/// Handles combat calculations and attack resolution according to Vitality System rules
/// </summary>
public class CombatService
{
    private readonly ILogger<CombatService> _logger;
    private readonly Random _random;

    public CombatService(ILogger<CombatService> logger)
    {
        _logger = logger;
        _random = new Random();
    }

    /// <summary>
    /// Resolves an attack attempt and calculates resulting damage or condition effects
    /// </summary>
    /// <param name="attack">Attack parameters</param>
    /// <param name="defense">Target's defensive stats</param>
    /// <returns>Attack resolution results including success and effect values</returns>
    public AttackResolution ResolveAttack(AttackParameters attack, DefenseParameters defense)
    {
        var resolution = new AttackResolution();

        // Accuracy check phase
        var accuracyRoll = RollD20();
        var accuracyTotal = CalculateAccuracyTotal(accuracyRoll, attack);
        var targetAvoidance = CalculateAvoidance(defense);

        _logger.LogDebug("Attack roll: {Roll} + {Mods} = {Total} vs Avoidance {Avoid}", 
            accuracyRoll, attack.AccuracyBonus, accuracyTotal, targetAvoidance);

        // Check for automatic success on natural 20
        if (accuracyRoll == 20)
        {
            resolution.Success = true;
            resolution.IsCritical = true;
        }
        // Otherwise compare total against target's Avoidance
        else if (accuracyTotal >= targetAvoidance)
        {
            resolution.Success = true;
        }
        else
        {
            resolution.Success = false;
            return resolution;
        }

        // Effect roll phase (damage or condition)
        if (attack.IsDamageAttack)
        {
            resolution.Effect = ResolveDamageEffect(attack, defense, resolution.IsCritical);
        }
        else
        {
            resolution.Effect = ResolveConditionEffect(attack, defense, resolution.IsCritical);
        }

        return resolution;
    }

    /// <summary>
    /// Calculates damage for a successful attack
    /// </summary>
    private int ResolveDamageEffect(AttackParameters attack, DefenseParameters defense, bool isCritical)
    {
        // Base damage roll is 3d6
        var damageRoll = Roll3D6();
        
        // Calculate total damage including attacker's Power
        var damageTotal = damageRoll + attack.Tier + 
            (int)Math.Ceiling(attack.Power * 1.5);

        // Add critical bonus if applicable
        if (isCritical)
        {
            damageTotal += attack.Tier;
        }

        // Calculate target's Durability
        var durability = CalculateDurability(defense);

        // Apply damage reduction and ensure minimum 0 damage
        var finalDamage = Math.Max(0, damageTotal - durability);

        _logger.LogDebug("Damage roll: {Roll} + {Mods} - {DR} = {Final}",
            damageRoll, (attack.Tier + attack.Power), durability, finalDamage);

        return finalDamage;
    }

    /// <summary>
    /// Calculates and applies condition effects
    /// </summary>
    private int ResolveConditionEffect(AttackParameters attack, DefenseParameters defense, bool isCritical)
    {
        // Condition check is d20 + (Tier × 2)
        var conditionRoll = RollD20();
        var conditionTotal = conditionRoll + (attack.Tier * 2);

        if (isCritical)
        {
            conditionTotal += attack.Tier;
        }

        // Get appropriate resistance based on condition type
        var resistance = GetResistanceValue(attack.ConditionType, defense);

        _logger.LogDebug("Condition roll: {Roll} + {Mods} vs Resistance {Res}",
            conditionRoll, (attack.Tier * 2), resistance);

        // Return margin of success/failure
        return conditionTotal - resistance;
    }

    #region Helper Methods

    private int RollD20() => _random.Next(1, 21);

    private int Roll3D6()
    {
        int total = 0;
        int diceToRoll = 3;

        // Roll 3d6 with explosion on 6s
        while (diceToRoll > 0)
        {
            int result = _random.Next(1, 7);
            total += result;
            diceToRoll--;

            // On a 6, add another die
            if (result == 6)
            {
                diceToRoll++;
            }
        }

        return total;
    }

    private int CalculateAccuracyTotal(int roll, AttackParameters attack)
    {
        return roll + attack.Tier + attack.Focus + attack.AccuracyBonus;
    }

    private int CalculateAvoidance(DefenseParameters defense)
    {
        return 10 + defense.Tier + defense.Mobility;
    }

    private int CalculateDurability(DefenseParameters defense)
    {
        return defense.Tier + (int)Math.Ceiling(defense.Endurance * 1.5);
    }

    private int GetResistanceValue(ConditionType type, DefenseParameters defense)
    {
        return type switch
        {
            ConditionType.Resolve => 10 + defense.Tier + defense.Focus,
            ConditionType.Stability => 10 + defense.Tier + defense.Power,
            ConditionType.Vitality => 10 + defense.Tier + defense.Endurance,
            _ => throw new ArgumentException("Invalid condition type", nameof(type))
        };
    }

    #endregion
}

/// <summary>
/// Parameters for the attacking character
/// </summary>
public class AttackParameters
{
    public int Tier { get; set; }
    public int Focus { get; set; }
    public int Power { get; set; }
    public int AccuracyBonus { get; set; }
    public bool IsDamageAttack { get; set; }
    public ConditionType ConditionType { get; set; }
}

/// <summary>
/// Parameters for the defending character
/// </summary>
public class DefenseParameters
{
    public int Tier { get; set; }
    public int Focus { get; set; }
    public int Power { get; set; }
    public int Mobility { get; set; }
    public int Endurance { get; set; }
}

/// <summary>
/// Results of an attack resolution
/// </summary>
public class AttackResolution
{
    public bool Success { get; set; }
    public bool IsCritical { get; set; }
    public int Effect { get; set; }
}

/// <summary>
/// Types of conditions that can be applied
/// </summary>
public enum ConditionType
{
    Resolve,
    Stability,
    Vitality
}