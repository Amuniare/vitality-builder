namespace VitalityBuilder.Api.Models.DTOs;

public class UtilityAttributesDto
{
    public int Awareness { get; set; }
    public int Communication { get; set; }
    public int Intelligence { get; set; }
    public int Total => Awareness + Communication + Intelligence;
}