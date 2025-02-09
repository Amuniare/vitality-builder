using VitalityBuilder.Api.Models;
using VitalityBuilder.Api.Models.Archetypes;
using VitalityBuilder.Api.Models.DTOs;

namespace VitalityBuilder.Api.Services;

/// <summary>
/// Interface defining core character archetype management operations
/// </summary>
public interface ICharacterArchetypesService
{
    Task<Models.Archetypes.CharacterArchetypes> CreateArchetypesAsync(CharacterArchetypesDto dto, int characterId);
    int CalculateMovementSpeed(Models.Archetypes.MovementArchetype archetype, int tier, int mobility);
    int CalculateSpecialAttackPoints(Models.Archetypes.SpecialAttackArchetype archetype, int tier);
    int CalculateUtilityPoints(Models.Archetypes.UtilityArchetype archetype, int tier);
    Dictionary<string, int> CalculateArchetypeBonuses(Models.Archetypes.CharacterArchetypes archetypes, int tier);
}
