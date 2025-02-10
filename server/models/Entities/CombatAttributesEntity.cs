namespace VitalityBuilder.Api.Models.Entities;

public class CombatAttributes
{
    public int Id { get; set; }
    public int Focus { get; set; }
    public int Power { get; set; }
    public int Mobility { get; set; }
    public int Endurance { get; set; }

    public int CharacterId { get; set; }
    public Character Character { get; set; } = null!;
    public int Total { get; internal set; }
}

