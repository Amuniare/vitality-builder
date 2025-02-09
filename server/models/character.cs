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

        // Relationships (EF Core navigation properties)
        public CombatAttributes CombatAttributes { get; set; } = new();
        public UtilityAttributes UtilityAttributes { get; set; } = new();
        public List<Expertise> Expertise { get; set; } = new();
        public List<SpecialAttack> SpecialAttacks { get; set; } = new();
        public List<UniquePower> UniquePowers { get; set; } = new();
    }
}