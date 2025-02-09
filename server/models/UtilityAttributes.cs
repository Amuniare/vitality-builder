namespace VitalityBuilder.Api.Models;
public class UtilityAttributes
{
    public int Id { get; set; }
    public int Awareness { get; set; }
    public int Communication { get; set; }
    public int Intelligence { get; set; }

    public int CharacterId { get; set; }
    public Character Character { get; set; } = null!;
}