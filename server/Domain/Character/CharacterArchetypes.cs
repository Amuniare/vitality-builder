using VitalityBuilder.Domain.Enums;

namespace VitalityBuilder.Domain.Character;

public class CharacterArchetypes
{
    public int Id { get; set; }

    public int CharacterId { get; set; }
    public Character Character { get; set; } = null!;

    // Core archetypes
    public MovementArchetype MovementType { get; set; }
    public AttackArchetype AttackType { get; set; }
    public EffectArchetype EffectType { get; set; }
    public UniqueAbilityArchetype UniqueAbility { get; set; }
    public SpecialAttackArchetype SpecialAttack { get; set; }
    public UtilityArchetype UtilityType { get; set; }

    /// <summary>
    /// Validates that all required archetypes are selected and compatible
    /// </summary>
    public bool ValidateArchetypes()
    {
        // Ensure all archetypes are selected
        if (!Enum.IsDefined(typeof(MovementArchetype), MovementType) ||
            !Enum.IsDefined(typeof(AttackArchetype), AttackType) ||
            !Enum.IsDefined(typeof(EffectArchetype), EffectType) ||
            !Enum.IsDefined(typeof(UniqueAbilityArchetype), UniqueAbility) ||
            !Enum.IsDefined(typeof(SpecialAttackArchetype), SpecialAttack) ||
            !Enum.IsDefined(typeof(UtilityArchetype), UtilityType))
        {
            return false;
        }

        // Add any specific incompatibility checks here
        return ValidateArchetypeCompatibility();
    }

    /// <summary>
    /// Checks for any archetype combinations that are not allowed
    /// </summary>
    private bool ValidateArchetypeCompatibility()
    {
        // Example compatibility check
        if (EffectType == EffectArchetype.DamageSpecialist &&
            AttackType == AttackArchetype.DirectSpecialist)
        {
            return false; // Direct attacks cannot be damage-only
        }

        return true;
    }

    /// <summary>
    /// Calculates movement speed modifications based on movement archetype
    /// </summary>
    public int ApplyMovementModifiers(int baseSpeed, int tier, int endurance)
    {
        return MovementType switch
        {
            MovementArchetype.Swift => baseSpeed + (int)Math.Ceiling(tier / 2.0),
            MovementArchetype.Vanguard => baseSpeed + endurance,
            MovementArchetype.Flight => baseSpeed,
            MovementArchetype.Portal => baseSpeed - 2,
            MovementArchetype.SuperJump => baseSpeed + (tier >= 7 ? 2 : 1),
            MovementArchetype.Swinging => baseSpeed + (tier >= 7 ? 2 : 1),
            _ => baseSpeed
        };
    }

    /// <summary>
    /// Applies attack archetype modifiers to accuracy rolls
    /// </summary>
    public int ApplyAttackModifiers(int baseAccuracy, int tier)
    {
        return AttackType switch
        {
            AttackArchetype.AOESpecialist => baseAccuracy - tier,
            _ => baseAccuracy
        };
    }

    /// <summary>
    /// Applies effect archetype modifiers to damage and condition rolls
    /// </summary>
    public (int Damage, int Condition) ApplyEffectModifiers(int baseDamage, int baseCondition, int tier)
    {
        return EffectType switch
        {
            EffectArchetype.HybridSpecialist => (baseDamage - tier, baseCondition - tier),
            EffectArchetype.CrowdControl => (baseDamage - tier, baseCondition),
            _ => (baseDamage, baseCondition)
        };
    }

    /// <summary>
    /// Gets the number of special attacks allowed based on archetype
    /// </summary>
    public int GetSpecialAttackLimit(int tier)
    {
        return SpecialAttack switch
        {
            SpecialAttackArchetype.OneSpell => 1,
            SpecialAttackArchetype.DualNatured => 2,
            _ => tier
        };
    }

    /// <summary>
    /// Calculates special attack points based on archetype and limits
    /// </summary>
    public int CalculateSpecialAttackPoints(int tier, int limitPoints)
    {
        return SpecialAttack switch
        {
            SpecialAttackArchetype.Paragon => 10 * tier,
            SpecialAttackArchetype.OneTrick => 20 * tier,
            SpecialAttackArchetype.DualNatured => 15 * tier,
            SpecialAttackArchetype.Basic => 10 * tier,
            SpecialAttackArchetype.SharedUses => 10 * tier - limitPoints,
            _ => limitPoints
        };
    }

    /// <summary>
    /// Gets utility point multiplier based on archetype
    /// </summary>
    public double GetUtilityPointMultiplier()
    {
        return UtilityType switch
        {
            UtilityArchetype.Specialized => 0.8, // (5 × [Tier-2])
            UtilityArchetype.JackOfAllTrades => 0.8, // (5 × [Tier-2])
            _ => 1.0 // Practical: (5 × [Tier-1])
        };
    }

    /// <summary>
    /// Creates a deep copy of the archetypes
    /// </summary>
    public CharacterArchetypes Clone()
    {
        return new CharacterArchetypes
        {
            MovementType = MovementType,
            AttackType = AttackType,
            EffectType = EffectType,
            UniqueAbility = UniqueAbility,
            SpecialAttack = SpecialAttack,
            UtilityType = UtilityType,
            CharacterId = CharacterId
        };
    }
}