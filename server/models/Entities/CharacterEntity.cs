using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VitalityBuilder.Api.Models.Entities;

public class CharacterEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Tier { get; set; } = 1;
    
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public int MainPointPool { get; private set; }

    public int SpecialAttacksPointPool { get; set; }
    public int UtilityPointPool { get; set; }

    // Navigation Properties
    [JsonIgnore]
    public virtual CombatAttributes? CombatAttributes { get; set; }
    
    [JsonIgnore]
    public virtual UtilityAttributes? UtilityAttributes { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<Expertise> Expertise { get; set; } = new List<Expertise>();
    
    [JsonIgnore]
    public virtual ICollection<SpecialAttack> SpecialAttacks { get; set; } = new List<SpecialAttack>();
    
    [JsonIgnore]
    public virtual ICollection<UniquePower> UniquePowers { get; set; } = new List<UniquePower>();

    // Add method for tier updates to encapsulate any future tier-related logic
    public void UpdateTier(int newTier)
    {
        if (newTier < 1) throw new ArgumentException("Tier cannot be less than 1", nameof(newTier));
        Tier = newTier;
        // MainPointPool will be automatically recalculated by the database
    }
    
    [JsonIgnore]
    public virtual Models.Archetypes.CharacterArchetypes? CharacterArchetypes { get; set; }

    // Calculated properties
    [NotMapped]
    public int AvailableMainPoints => MainPointPool - (Expertise?.Sum(e => e.Cost) ?? 0);

    [NotMapped]
    public int AvailableSpecialAttacksPoints => SpecialAttacksPointPool - (SpecialAttacks?.Sum(sa => sa.Cost) ?? 0);

    [NotMapped]
    public int AvailableUtilityPoints => UtilityPointPool - (UniquePowers?.Sum(up => up.Cost) ?? 0);
}