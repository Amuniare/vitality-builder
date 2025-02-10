using VitalityBuilder.Api.Models.Enums;

namespace VitalityBuilder.Api.Models.Archetypes;

public class UniqueAbilityArchetype
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public UniqueAbilityCategory Category { get; set; }
    public int ExtraQuickActions { get; set; }
    public int ExtraPointPool { get; set; }
    public Dictionary<string, int> StatBonuses { get; set; } = new();
    
    public int CharacterArchetypesId { get; set; }
    public CharacterArchetypes CharacterArchetypes { get; set; } = null!;
}