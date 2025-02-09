using VitalityBuilder.Api.Models.Archetypes;

namespace VitalityBuilder.Api.Services;

public partial class CharacterArchetypesService
{
    private static int CalculateNormalArchetypePoints(int tier) => tier * 10;

    private static int CalculateSpecialistPoints(int tier, List<string> requiredLimits)
    {
        float limitMultiplier = tier / 3.0f;
        int limitPoints = requiredLimits.Sum(GetLimitBaseValue);
        return (int)(limitPoints * limitMultiplier);
    }

    private static int GetLimitBaseValue(string limitName) => limitName switch
    {
        "Reload" => 20,
        "Stockpile" => 40,
        "Cooldown2" => 20,
        "Cooldown3" => 30,
        "Reserves3" => 10,
        "Reserves2" => 20,
        "Reserves1" => 40,
        "Finite5" => 10,
        "Finite3" => 20,
        "Finite2" => 30,
        "Finite1" => 50,
        "Charger" => 10,
        "Slowed" => 10,
        "Unreliable20" => 200,
        "Unreliable15" => 80,
        "Unreliable10" => 40,
        "Unreliable5" => 20,
        _ => 0
    };

    private static int AdjustSpecialMovementSpeed(MovementArchetypeType type, int baseSpeed) => type switch
    {
        MovementArchetypeType.Flight => baseSpeed,
        MovementArchetypeType.Teleportation => baseSpeed - 2, // Teleport has reduced range
        MovementArchetypeType.Portal => baseSpeed,
        _ => baseSpeed
    };
}
