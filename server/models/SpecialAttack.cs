namespace VitalityBuilder.Api.Models;

public class SpecialAttack
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AttackType { get; set; } = string.Empty;
    public string EffectType { get; set; } = string.Empty;
    public List<string> Limits { get; set; } = new();
    public List<string> Upgrades { get; set; } = new();

    public int CharacterId { get; set; }
    public Character Character { get; set; } = null!;
}