using VitalityBuilder.Api.Models.Enums;

namespace VitalityBuilder.Api.Models.Archetypes;

public class AttackTypeArchetype
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public AttackTypeArchetypeCategory Category { get; set; }
    public int AccuracyPenalty { get; set; }
    public int EffectPenalty { get; set; }
    public bool BypassesAccuracyChecks { get; set; }
    public bool HasFreeAOE { get; set; }
    
    public int CharacterArchetypesId { get; set; }
    public CharacterArchetypes CharacterArchetypes { get; set; } = null!;
}