namespace VitalityBuilder.Api.Models.DTOs
    {

        public class CharacterResponseDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public int Tier { get; set; }
        
            // Defense values
            public int Avoidance { get; set; }
            public double Durability { get; set; }
            public int ResolveResistance { get; set; }
            public int StabilityResistance { get; set; }
            public int VitalityResistance { get; set; }
        
            // Point pools
            public int RemainingCombatAP { get; set; }
            public int RemainingUtilityAP { get; set; }
        
            // Attributes
            public CombatAttributesDto CombatAttributes { get; set; } = new();
            public UtilityAttributesDto UtilityAttributes { get; set; } = new();
        }
    }
