namespace VitalityBuilder.Api.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Tier { get; set; } = 1;
        
        // Combat Attributes
        public int Focus { get; set; }
        public int Power { get; set; }
        public int Mobility { get; set; }
        public int Endurance { get; set; }
        
        // Utility Attributes
        public int Awareness { get; set; }
        public int Communication { get; set; }
        public int Intelligence { get; set; }
    }
}