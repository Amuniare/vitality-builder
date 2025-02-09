namespace VitalityBuilder.Api.Models.Archetypes;

public class SpecialAttackArchetype
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public SpecialAttackCategory Category { get; set; }
    public int BasePoints { get; set; }
    public int MaxSpecialAttacks { get; set; }
    public float LimitPointMultiplier { get; set; }
    public bool CanTakeLimits { get; set; }
    public List<string> RequiredLimits { get; set; } = new();
    
    public int CharacterArchetypesId { get; set; }
    public CharacterArchetypes CharacterArchetypes { get; set; } = null!;
}
