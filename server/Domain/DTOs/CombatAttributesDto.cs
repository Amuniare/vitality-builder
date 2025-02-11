namespace VitalityBuilder.Api.Models.DTOs;

public class CombatAttributesDto
{
    public int Focus { get; set; }
    public int Power { get; set; }
    public int Mobility { get; set; }
    public int Endurance { get; set; }
    public int Total => Focus + Power + Mobility + Endurance;
}