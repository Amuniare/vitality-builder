using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VitalityBuilder.Api.Models.Entities;

public class Character
{
    private int _mainPointPool;

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Tier { get; set; } = 1;
    
    // Stored calculated value
    public int MainPointPool
    {
        get => _mainPointPool;
        private set => _mainPointPool = Math.Max(0, (Tier - 2) * 15);
    }

    // Database-persisted values
    public int SpecialAttacksPointPool { get; set; }
    public int UtilityPointPool { get; set; }

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

    // Non-persisted calculated properties
    [NotMapped]
    public int AvailableMainPoints => MainPointPool - Expertise.Sum(e => e.Cost);

    [NotMapped]
    public int AvailableSpecialAttacksPoints => SpecialAttacksPointPool - SpecialAttacks.Sum(sa => sa.Cost);

    [NotMapped]
    public int AvailableUtilityPoints => UtilityPointPool - UniquePowers.Sum(up => up.Cost);

    // Constructor to ensure initial calculation
    public Character() => CalculateMainPointPool();

    // Call this whenever Tier changes
    public void CalculateMainPointPool() => _mainPointPool = Math.Max(0, (Tier - 2) * 15);
}