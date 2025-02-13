namespace VitalityBuilder.Api.Domain.Constants;

public static class GameRuleConstants
{
    // Character Limits
    public const int MinimumTier = 2;
    public const int MaximumTier = 10;
    public const int BaseHealthPool = 100;
    public const int MaxEffortPerSession = 2;
    
    // Attribute Rules
    public const int BaseMovementSpeed = 6;
    public const double EnduranceMultiplier = 1.5;
    public const int BaseResistanceValue = 10;
    
    // Point Calculations
    public const int UtilityPointsPerTier = 5;
    public const int CombatAttributePointsMultiplier = 2;
    
    // Combat Rules
    public const int CriticalHitThreshold = 20;
    public const int BaseDamageRoll = 3; // Number of d6
    public const int BaseAccuracyRoll = 1; // Number of d20
    public const int FallingDamageThreshold = 6; // Spaces
    public const int FallingDamageBase = 1; // Base d6
    
    // Pool Calculations
    public static int CalculateMainPool(int tier) => (tier - 2) * 15;
    public static int CalculateUtilityPoints(int tier) => 5 * (tier - 1);
    public static int CalculateCombatAttributePoints(int tier) => tier * 2;
    public static int CalculateUtilityAttributePoints(int tier) => tier;
    
    // Movement Rules
    public const int BaseJumpDistance = 1;
    public const int JumpDCMultiplier = 5;
    public const int SwimmingMovementCost = 2;
    public const int ClimbingMovementCost = 3;
    public const int FallingSpacesPerTurn = 18;
    
    // Healing Rules
    public const int RestHealingPerHour = 10;
    
    // Special Attack Calculations
    public static class SpecialAttackLimits
    {
        public static int CalculateFullValueLimit(int tier) => tier * 10;
        public static int CalculateHalfValueLimit(int tier) => tier * 20;
        public static double FullValueMultiplier = 1.0;
        public static double HalfValueMultiplier = 0.5;
        public static double QuarterValueMultiplier = 0.25;
    }
    
    // Resistance Values
    public static class ResistanceTypes
    {
        public const string Resolve = "Resolve";
        public const string Stability = "Stability";
        public const string Vitality = "Vitality";
    }
    
    // Validation Messages
    public static class ValidationMessages
    {
        public const string TierOutOfRange = "Character tier must be between {0} and {1}";
        public const string AttributeExceedsTier = "{0} cannot exceed character tier";
        public const string TotalAttributesExceedLimit = "Total {0} attributes cannot exceed {1}";
        public const string InsufficientPoints = "Insufficient points in {0} pool";
        public const string RequiredArchetype = "Must select a {0} archetype";
        public const string IncompatibleArchetypes = "Selected archetypes are not compatible";
    }
    
    // Combat Condition Rules
    public static class CombatConditions
    {
        public const int ExhaustedMovementPenalty = 2; // Halved movement
        public const int AdjacentAttackBonus = 5; // Against prone targets
        public const int RangedAttackPenalty = -5; // Against prone targets
        public const int ProneMovementCost = 3; // Spaces to stand up
    }
    
    // Skill Check Rules
    public static class SkillChecks
    {
        public const int CriticalSuccessThreshold = 20;
        public const int MaxExpertiseStack = 2;
        
        public static class DifficultyClass
        {
            public const int Simple = 10;
            public const int Easy = 15;
            public const int Moderate = 20;
            public const int Challenging = 25;
            public const int VeryDifficult = 30;
            public const int ExtremelyDifficult = 35;
            public const int NearlyImpossible = 40;
        }
    }
}