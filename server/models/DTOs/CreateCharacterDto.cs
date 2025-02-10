namespace VitalityBuilder.Api.Models.DTOs;

public class CreateCharacterDto
{
    public string Name { get; set; } = string.Empty;
    public int Tier { get; set; } = 1;
    public CombatAttributesDto CombatAttributes { get; set; } = new();
    public UtilityAttributesDto UtilityAttributes { get; set; } = new();
}