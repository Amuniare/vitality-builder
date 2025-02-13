using VitalityBuilder.Domain.Character;
using VitalityBuilder.Domain.Constants;
using VitalityBuilder.Interfaces.Services;

namespace VitalityBuilder.Services.Calculations;

public class PointPoolCalculator : IPointPoolCalculator
{
    public PointPools CalculateAllPools(Character character)
    {
        return new PointPools
        {
            MainPool = CalculateMainPool(character),
            UtilityPoints = CalculateUtilityPoints(character),
            CombatAttributePoints = CalculateCombatAttributePoints(character),
            UtilityAttributePoints = CalculateUtilityAttributePoints(character),
            SpecialAttackPoints = CalculateSpecialAttackPoints(character)
        };
    }

    private int CalculateMainPool(Character character)
    {
        var basePool = GameRuleConstants.CalculateMainPool(character.Tier);

        // Apply archetype modifiers
        if (character.Archetypes.UniqueAbility == UniqueAbilityArchetype.Extraordinary)
        {
            basePool += GameRuleConstants.CalculateMainPool(character.Tier);
        }

        return basePool;
    }

    private int CalculateUtilityPoints(Character character)
    {
        var basePoints = GameRuleConstants.CalculateUtilityPoints(character.Tier);

        // Apply archetype modifier
        var multiplier = character.Archetypes.GetUtilityPointMultiplier();
        return (int)(basePoints * multiplier);
    }

    private int CalculateCombatAttributePoints(Character character)
    {
        return GameRuleConstants.CombatAttributePointsMultiplier * character.Tier;
    }

    private int CalculateUtilityAttributePoints(Character character)
    {
        return character.Tier;
    }

    private int CalculateSpecialAttackPoints(Character character)
    {
        // Calculate base points from limits
        var limitPoints = CalculateLimitPoints(character);

        // Apply archetype modifications
        return character.Archetypes.CalculateSpecialAttackPoints(
            character.Tier, 
            limitPoints
        );
    }

    private int CalculateLimitPoints(Character character)
    {
        var totalLimitValue = 0;
        foreach (var attack in character.SpecialAttacks)
        {
            var limitValues = attack.Limits.Sum(l => GetLimitValue(l));
            totalLimitValue += CalculateLimitPointValue(limitValues, character.Tier);
        }
        return totalLimitValue;
    }

    private int CalculateLimitPointValue(int limitValue, int tier)
    {
        var fullValueLimit = GameRuleConstants.SpecialAttackLimits.CalculateFullValueLimit(tier);
        var halfValueLimit = GameRuleConstants.SpecialAttackLimits.CalculateHalfValueLimit(tier);

        var fullValuePoints = Math.Min(limitValue, fullValueLimit);
        var halfValuePoints = Math.Min(
            Math.Max(0, limitValue - fullValueLimit),
            halfValueLimit
        );
        var quarterValuePoints = Math.Max(0, limitValue - fullValueLimit - halfValueLimit);

        return fullValuePoints +
               (int)(halfValuePoints * GameRuleConstants.SpecialAttackLimits.HalfValueMultiplier) +
               (int)(quarterValuePoints * GameRuleConstants.SpecialAttackLimits.QuarterValueMultiplier);
    }

    private int GetLimitValue(string limit)
    {
        // Implementation would match limit names to their point values
        // This would likely use a dictionary or switch statement
        return limit.ToLower() switch
        {
            "reload" => 20,
            "stockpile" => 40,
            "cooldown2" => 20,
            "cooldown3" => 30,
            "reserves3" => 10,
            "reserves2" => 20,
            "reserves1" => 40,
            "finite5" => 10,
            "finite3" => 20,
            "finite2" => 30,
            "finite1" => 50,
            "charger" => 10,
            "slowed" => 10,
            "focused" => 30,
            "unhealthy" => 30,
            "healthy" => 20,
            "timid" => 50,
            "avenger" => 50,
            "purist" => 10,
            _ => 0
        };
    }

    public ValidationResult ValidatePointAllocation(Character character)
    {
        var pools = CalculateAllPools(character);
        var result = new ValidationResult();

        // Validate main pool
        if (character.SpentMainPoints > pools.MainPool)
        {
            result.AddError($"Main pool overspent by {character.SpentMainPoints - pools.MainPool} points");
        }

        // Validate utility points
        if (character.SpentUtilityPoints > pools.UtilityPoints)
        {
            result.AddError($"Utility points overspent by {character.SpentUtilityPoints - pools.UtilityPoints} points");
        }

        // Validate combat attributes
        var spentCombatPoints = character.CombatAttributes.TotalPoints;
        if (spentCombatPoints > pools.CombatAttributePoints)
        {
            result.AddError($"Combat attribute points overspent by {spentCombatPoints - pools.CombatAttributePoints} points");
        }

        // Validate utility attributes
        var spentUtilityPoints = character.UtilityAttributes.TotalPoints;
        if (spentUtilityPoints > pools.UtilityAttributePoints)
        {
            result.AddError($"Utility attribute points overspent by {spentUtilityPoints - pools.UtilityAttributePoints} points");
        }

        // Add warnings for unspent points
        if (pools.MainPool - character.SpentMainPoints > 0)
        {
            result.AddWarning($"Unspent main pool points: {pools.MainPool - character.SpentMainPoints}");
        }

        if (pools.UtilityPoints - character.SpentUtilityPoints > 0)
        {
            result.AddWarning($"Unspent utility points: {pools.UtilityPoints - character.SpentUtilityPoints}");
        }

        return result;
    }
}

public class PointPools
{
    public int MainPool { get; set; }
    public int UtilityPoints { get; set; }
    public int CombatAttributePoints { get; set; }
    public int UtilityAttributePoints { get; set; }
    public int SpecialAttackPoints { get; set; }
}

public class ValidationResult
{
    public bool IsValid => !Errors.Any();
    public List<string> Errors { get; } = new();
    public List<string> Warnings { get; } = new();

    public void AddError(string error) => Errors.Add(error);
    public void AddWarning(string warning) => Warnings.Add(warning);
}