using System.Text.Json.Serialization;

namespace VitalityBuilder.Api.Models.Entities;  // Make sure this is the correct namespace

public class Character
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Tier { get; set; } = 1;
    
    // Point Pool Calculations
    public int CombatAP => Tier * 2;
    public int UtilityAP => Tier;
    public int MainPointPool => Math.Max(0, (Tier - 2) * 15);
    public int SpecialAttacksPointPool { get; set; }
    public int UtilityPointPool { get; set; }
    
    // Defense Calculations
    public int Avoidance => 10 + Tier + (CombatAttributes?.Mobility ?? 0);
    public double Durability => Math.Ceiling(Tier + (CombatAttributes?.Endurance * 1.5) ?? 0);
    public int HealthPool => 100;

    // Navigation Properties
    [JsonIgnore]
    public virtual CombatAttributes? CombatAttributes { get; set; }
    [JsonIgnore]
    public virtual UtilityAttributes? UtilityAttributes { get; set; }
    [JsonIgnore]
    public virtual List<Expertise> Expertise { get; set; } = new();
    [JsonIgnore]
    public virtual List<SpecialAttack> SpecialAttacks { get; set; } = new();
    [JsonIgnore]
    public virtual List<UniquePower> UniquePowers { get; set; } = new();
    [JsonIgnore]
    public virtual Models.Archetypes.CharacterArchetypes? CharacterArchetypes { get; set; }

    [JsonIgnore]
    public int AvailableMainPoints => MainPointPool - Expertise.Sum(e => e.Cost);
}