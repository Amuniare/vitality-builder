namespace VitalityBuilder.Api.Models
{
    public class CreateCharacterDto
    {
        public string Name { get; set; } = string.Empty;
        public int Tier { get; set; } = 1;
        public int MainPointPool { get; set; }
        public int SpecialAttacksPointPool { get; set; }
        public int UtilityPointPool { get; set; }
        public CombatAttributesDto CombatAttributes { get; set; } = new();
        public UtilityAttributesDto UtilityAttributes { get; set; } = new();
        public List<ExpertiseDto> Expertise { get; set; } = new();
        public List<SpecialAttackDto> SpecialAttacks { get; set; } = new();
        public List<UniquePowerDto> UniquePowers { get; set; } = new();
    }

    public class CombatAttributesDto
    {
        public int Focus { get; set; }
        public int Power { get; set; }
        public int Mobility { get; set; }
        public int Endurance { get; set; }
    }

    public class UtilityAttributesDto
    {
        public int Awareness { get; set; }
        public int Communication { get; set; }
        public int Intelligence { get; set; }
    }

    public class ExpertiseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Cost { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class SpecialAttackDto
    {
        public string Name { get; set; } = string.Empty;
        public string AttackType { get; set; } = string.Empty;
        public string EffectType { get; set; } = string.Empty;
        public List<string> Limits { get; set; } = new();
        public List<string> Upgrades { get; set; } = new();
    }

    public class UniquePowerDto
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Cost { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}