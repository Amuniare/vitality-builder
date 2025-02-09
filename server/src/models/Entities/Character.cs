using System.Text.Json.Serialization;

namespace VitalityBuilder.Api.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Tier { get; set; } = 1;
        public int MainPointPool { get; set; }
        public int SpecialAttacksPointPool { get; set; }
        public int UtilityPointPool { get; set; }

        // Make navigation properties virtual for lazy loading
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
    }
}