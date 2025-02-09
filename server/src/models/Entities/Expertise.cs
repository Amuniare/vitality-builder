namespace VitalityBuilder.Api.Models;

public class Expertise
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Cost { get; set; }
    public string Description { get; set; } = string.Empty;

    public int CharacterId { get; set; }
    public Character Character { get; set; } = null!;
}