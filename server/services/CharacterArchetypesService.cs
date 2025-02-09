using Microsoft.Extensions.Logging;
using VitalityBuilder.Api.Infrastructure;
using VitalityBuilder.Api.Models;
using VitalityBuilder.Api.Models.Archetypes;
using VitalityBuilder.Api.Models.DTOs;

namespace VitalityBuilder.Api.Services;

/// <summary>
/// Service implementing character archetype management and calculations
/// </summary>
public partial class CharacterArchetypesService : ICharacterArchetypesService
{
    private readonly ArchetypeDbContext _context;
    private readonly ILogger<CharacterArchetypesService> _logger;

    public CharacterArchetypesService(ArchetypeDbContext context, ILogger<CharacterArchetypesService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Models.Archetypes.CharacterArchetypes> CreateArchetypesAsync(CharacterArchetypesDto dto, int characterId)
    {
        _logger.LogInformation("Creating archetypes for character {CharacterId}", characterId);

        try
        {
            var archetypes = new Models.Archetypes.CharacterArchetypes
            {
                CharacterId = characterId,
                MovementArchetype = MapMovementArchetype(dto.MovementArchetype),
                AttackTypeArchetype = MapAttackTypeArchetype(dto.AttackTypeArchetype),
                EffectTypeArchetype = MapEffectTypeArchetype(dto.EffectTypeArchetype),
                UniqueAbilityArchetype = MapUniqueAbilityArchetype(dto.UniqueAbilityArchetype),
                SpecialAttackArchetype = MapSpecialAttackArchetype(dto.SpecialAttackArchetype),
                UtilityArchetype = MapUtilityArchetype(dto.UtilityArchetype)
            };

            _context.Set<Models.Archetypes.CharacterArchetypes>().Add(archetypes);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully created archetypes for character {CharacterId}", characterId);
            return archetypes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating archetypes for character {CharacterId}", characterId);
            throw;
        }
    }

}
