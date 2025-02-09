namespace VitalityBuilder.Api.Models.Archetypes;

public class UtilityArchetype
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public UtilityCategory Category { get; set; }
    public int BaseUtilityPool { get; set; }
    public bool CanPurchaseExpertise { get; set; }
    public float TierBonusMultiplier { get; set; }
    public List<string> Restrictions { get; set; } = new();
    
    public int CharacterArchetypesId { get; set; }
    public CharacterArchetypes CharacterArchetypes { get; set; } = null!;
}