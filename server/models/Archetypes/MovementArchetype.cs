using System.ComponentModel.DataAnnotations.Schema;
using VitalityBuilder.Api.Models.Enums;

namespace VitalityBuilder.Api.Models.Archetypes;

public class MovementArchetype
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public MovementArchetypeType Type { get; set; }
    
    [Column(TypeName = "nvarchar(max)")]
    public Dictionary<int, int> SpeedBonusByTier { get; set; } = new();
    
    public bool IgnoresOpportunityAttacks { get; set; }
    public bool IgnoresDifficultTerrain { get; set; }
    public bool IsImmuneToProne { get; set; }
    public float MovementMultiplier { get; set; } = 1.0f;
    
    public int CharacterArchetypesId { get; set; }
    public CharacterArchetypes CharacterArchetypes { get; set; } = null!;
}