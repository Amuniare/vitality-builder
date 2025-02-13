using VitalityBuilder.Api.Domain.Character;
using VitalityBuilder.Api.Domain.Errors;

namespace VitalityBuilder.Api.Interfaces.Services;

/// <summary>
/// Interface for calculating and validating character point pools
/// </summary>
public interface IPointPoolCalculator
{
    /// <summary>
    /// Calculates all point pools for a character
    /// </summary>
    Task<PointPools> CalculateAllPoolsAsync(Character character);

    /// <summary>
    /// Calculates special attack points including limit modifiers
    /// </summary>
    Task<SpecialAttackPoints> CalculateSpecialAttackPointsAsync(
        Character character,
        IEnumerable<string> limits);

    /// <summary>
    /// Validates point allocation across all pools
    /// </summary>
    Task<ValidationResult> ValidatePointAllocationAsync(Character character);

    /// <summary>
    /// Checks if a character has sufficient points for a purchase
    /// </summary>
    Task<bool> HasSufficientPointsAsync(
        Character character,
        string poolType,
        int cost);

    /// <summary>
    /// Calculates remaining points in each pool
    /// </summary>
    Task<PointPoolRemaining> CalculateRemainingPointsAsync(Character character);

    /// <summary>
    /// Calculates point costs for a specific purchase
    /// </summary>
    Task<PointCost> CalculatePointCostAsync(
        Character character,
        string purchaseType,
        IDictionary<string, object> parameters);

    /// <summary>
    /// Validates limit point calculations
    /// </summary>
    Task<ValidationResult> ValidateLimitPointsAsync(
        Character character,
        IEnumerable<string> limits);
}

public class PointPools
{
    /// <summary>
    /// Base point pools
    /// </summary>
    public int MainPool { get; set; }
    public int UtilityPoints { get; set; }
    public int CombatAttributePoints { get; set; }
    public int UtilityAttributePoints { get; set; }

    /// <summary>
    /// Special attack points
    /// </summary>
    public int BaseSpecialAttackPoints { get; set; }
    public int BonusSpecialAttackPoints { get; set; }
    public int TotalSpecialAttackPoints => BaseSpecialAttackPoints + BonusSpecialAttackPoints;

    /// <summary>
    /// Point modifiers from archetypes
    /// </summary>
    public double MainPoolMultiplier { get; set; } = 1.0;
    public double UtilityPoolMultiplier { get; set; } = 1.0;
    public double SpecialAttackMultiplier { get; set; } = 1.0;

    /// <summary>
    /// Point tracking
    /// </summary>
    public Dictionary<string, int> SpentPoints { get; set; } = new();
    public Dictionary<string, int> BonusPoints { get; set; } = new();
}

public class SpecialAttackPoints
{
    /// <summary>
    /// Base points from tier
    /// </summary>
    public int BasePoints { get; set; }

    /// <summary>
    /// Points from limits
    /// </summary>
    public int LimitPoints { get; set; }

    /// <summary>
    /// Archetype bonuses
    /// </summary>
    public int ArchetypeBonus { get; set; }

    /// <summary>
    /// Total available points
    /// </summary>
    public int TotalPoints => BasePoints + LimitPoints + ArchetypeBonus;

    /// <summary>
    /// Detail about limit calculations
    /// </summary>
    public Dictionary<string, int> LimitValues { get; set; } = new();
    public Dictionary<string, double> LimitMultipliers { get; set; } = new();
}

public class PointPoolRemaining
{
    /// <summary>
    /// Remaining points in each pool
    /// </summary>
    public int MainPoolRemaining { get; set; }
    public int UtilityPointsRemaining { get; set; }
    public int CombatAttributePointsRemaining { get; set; }
    public int UtilityAttributePointsRemaining { get; set; }
    public int SpecialAttackPointsRemaining { get; set; }

    /// <summary>
    /// Tracking for specific categories
    /// </summary>
    public Dictionary<string, int> CategoryRemaining { get; set; } = new();
    public Dictionary<string, int> UnspentBonusPoints { get; set; } = new();
}

public class PointCost
{
    /// <summary>
    /// Base cost before modifiers
    /// </summary>
    public int BaseCost { get; set; }

    /// <summary>
    /// Modifiers affecting cost
    /// </summary>
    public List<PointCostModifier> Modifiers { get; set; } = new();

    /// <summary>
    /// Final cost after all modifiers
    /// </summary>
    public int FinalCost { get; set; }

    /// <summary>
    /// Pool that points should be spent from
    /// </summary>
    public string PoolType { get; set; } = string.Empty;

    /// <summary>
    /// Whether prerequisites are met
    /// </summary>
    public bool PrerequisitesMet { get; set; }

    /// <summary>
    /// Any warnings about the purchase
    /// </summary>
    public List<string> Warnings { get; set; } = new();
}

public class PointCostModifier
{
    /// <summary>
    /// Source of the modifier
    /// </summary>
    public string Source { get; set; } = string.Empty;

    /// <summary>
    /// Type of modification
    /// </summary>
    public PointModificationType Type { get; set; }

    /// <summary>
    /// Value of the modifier
    /// </summary>
    public double Value { get; set; }

    /// <summary>
    /// Order in which modifier should be applied
    /// </summary>
    public int Priority { get; set; }
}

public enum PointModificationType
{
    Multiplier,
    FlatBonus,
    PercentageDiscount,
    MinimumCost,
    MaximumCost
}