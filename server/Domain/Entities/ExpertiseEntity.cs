namespace VitalityBuilder.Api.Models.Entities;
public class Expertise
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Cost { get; set; }
    public string Description { get; set; } = string.Empty;

    public int CharacterId { get; set; }
    public CharacterEntity Character { get; set; } = null!;
}