    
public class ValidateAttributePointsService
{
    public bool IsValid(CreateCharacterDto dto)
    {
        // Combat attributes cannot exceed Tier × 2 total
        if (dto.CombatAttributes.Total > dto.Tier * 2)
            return false;

        // Individual combat attributes cannot exceed Tier
        if (dto.CombatAttributes.Focus > dto.Tier ||
            dto.CombatAttributes.Power > dto.Tier ||
            dto.CombatAttributes.Mobility > dto.Tier ||
            dto.CombatAttributes.Endurance > dto.Tier)
            return false;

        // Utility attributes cannot exceed Tier total
        if (dto.UtilityAttributes.Total > dto.Tier)
            return false;

        // Individual utility attributes cannot exceed Tier
        if (dto.UtilityAttributes.Awareness > dto.Tier ||
            dto.UtilityAttributes.Communication > dto.Tier ||
            dto.UtilityAttributes.Intelligence > dto.Tier)
            return false;

        return true;
    }