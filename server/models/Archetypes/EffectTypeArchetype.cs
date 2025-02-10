using VitalityBuilder.Api.Models.Enums;

namespace VitalityBuilder.Api.Models.Archetypes;

public class EffectTypeArchetype
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public EffectTypeCategory Category { get; set; }
    public bool HasAccessToAdvancedConditions { get; set; }
    public int DamagePenalty { get; set; }
    public int ConditionPenalty { get; set; }
    public bool RequiresHybridEffects { get; set; }
    
    public int CharacterArchetypesId { get; set; }
    public CharacterArchetypes CharacterArchetypes { get; set; } = null!;
}