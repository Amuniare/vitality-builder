using System.ComponentModel.DataAnnotations;
using VitalityBuilder.Domain.Constants;

namespace VitalityBuilder.Domain.Dtos.Character;

/// <summary>
/// Basic data transfer object for character creation and updates
/// </summary>
public class CharacterBasicDto
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [Range(GameRuleConstants.MinimumTier, GameRuleConstants.MaximumTier)]
    public int Tier { get; set; }

    // Combat Attributes
    [Range(0, GameRuleConstants.MaximumTier)]
    public int Focus { get; set; }

    [Range(0, GameRuleConstants.MaximumTier)]
    public int Power { get; set; }

    [Range(0, GameRuleConstants.MaximumTier)]
    public int Mobility { get; set; }

    [Range(0, GameRuleConstants.MaximumTier)]
    public int Endurance { get; set; }

    // Utility Attributes
    [Range(0, GameRuleConstants.MaximumTier)]
    public int Awareness { get; set; }

    [Range(0, GameRuleConstants.MaximumTier)]
    public int Communication { get; set; }

    [Range(0, GameRuleConstants.MaximumTier)]
    public int Intelligence { get; set; }

    // Archetype Selections
    [Required]
    public string MovementArchetype { get; set; } = string.Empty;

    [Required]
    public string AttackArchetype { get; set; } = string.Empty;

    [Required]
    public string EffectArchetype { get; set; } = string.Empty;

    [Required]
    public string UniqueAbilityArchetype { get; set; } = string.Empty;

    [Required]
    public string SpecialAttackArchetype { get; set; } = string.Empty;

    [Required]
    public string UtilityArchetype { get; set; } = string.Empty;
}