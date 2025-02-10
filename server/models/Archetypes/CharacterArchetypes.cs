using VitalityBuilder.Api.Models.Entities;
using VitalityBuilder.Api.Models.Enums;

namespace VitalityBuilder.Api.Models.Archetypes;

public class CharacterArchetypes
{
    public int Id { get; set; }
    public MovementArchetype MovementArchetype { get; set; } = null!;
    public AttackTypeArchetype AttackTypeArchetype { get; set; } = null!;
    public EffectTypeArchetype EffectTypeArchetype { get; set; } = null!;
    public UniqueAbilityArchetype UniqueAbilityArchetype { get; set; } = null!;
    public SpecialAttackArchetype SpecialAttackArchetype { get; set; } = null!;
    public UtilityArchetype UtilityArchetype { get; set; } = null!;
    
    public int CharacterId { get; set; }
    public CharacterEntity Character { get; set; } = null!;
}