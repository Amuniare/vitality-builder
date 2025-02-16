using Microsoft.EntityFrameworkCore;
using VitalityBuilder.Api.Domain.Character;
using VitalityBuilder.Api.Interfaces.Repositories;

namespace VitalityBuilder.Api.Infrastructure.Data;

public class CharacterRepository : ICharacterRepository
{
    private readonly VitalityBuilderContext _context;
    private readonly ILogger<CharacterRepository> _logger;

    public CharacterRepository(
        VitalityBuilderContext context,
        ILogger<CharacterRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Character?> GetCharacterAsync(int id)
    {
        return await _context.Characters
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Character?> GetCharacterWithDetailsAsync(int id)
    {
        return await _context.GetCharactersWithRelated()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> CharacterExistsAsync(int id)
    {
        return await _context.CharacterExistsAsync(id);
    }

    public async Task<bool> NameExistsAsync(string name)
    {
        return await _context.Characters
            .AnyAsync(c => c.Name.ToLower() == name.ToLower());
    }

    public async Task<IEnumerable<Character>> GetCharactersAsync(
        int? minTier = null,
        int? maxTier = null,
        string? nameSearch = null,
        int? skip = null,
        int? take = null)
    {
        var query = _context.GetCharactersWithRelated();

        if (minTier.HasValue)
            query = query.Where(c => c.Tier >= minTier.Value);

        if (maxTier.HasValue)
            query = query.Where(c => c.Tier <= maxTier.Value);

        if (!string.IsNullOrWhiteSpace(nameSearch))
            query = query.Where(c => c.Name.Contains(nameSearch));

        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (take.HasValue)
            query = query.Take(take.Value);

        return await query.ToListAsync();
    }

    public async Task<int> GetCharacterCountAsync(
        int? minTier = null,
        int? maxTier = null,
        string? nameSearch = null)
    {
        var query = _context.Characters.AsQueryable();

        if (minTier.HasValue)
            query = query.Where(c => c.Tier >= minTier.Value);

        if (maxTier.HasValue)
            query = query.Where(c => c.Tier <= maxTier.Value);

        if (!string.IsNullOrWhiteSpace(nameSearch))
            query = query.Where(c => c.Name.Contains(nameSearch));

        return await query.CountAsync();
    }

    public async Task AddCharacterAsync(Character character)
    {
        await _context.Characters.AddAsync(character);
        _logger.LogInformation("Added new character: {Name}", character.Name);
    }

    public void UpdateCharacter(Character character)
    {
        _context.Entry(character).State = EntityState.Modified;
        _logger.LogInformation("Updated character: {Id}", character.Id);
    }

    public async Task DeleteCharacterAsync(int id)
    {
        var character = await _context.Characters.FindAsync(id);
        if (character != null)
        {
            character.IsDeleted = true;
            _logger.LogInformation("Marked character {Id} as deleted", id);
        }
    }

    public async Task<IEnumerable<SpecialAttack>> GetSpecialAttacksAsync(int characterId)
    {
        return await _context.SpecialAttacks
            .Where(sa => sa.CharacterId == characterId)
            .ToListAsync();
    }

    public async Task<IEnumerable<CharacterFeature>> GetFeaturesAsync(int characterId)
    {
        return await _context.Features
            .Where(f => f.CharacterId == characterId)
            .ToListAsync();
    }

    public async Task<IEnumerable<CharacterExpertise>> GetExpertiseAsync(int characterId)
    {
        return await _context.Expertise
            .Where(e => e.CharacterId == characterId)
            .ToListAsync();
    }

    public async Task<bool> HasFeatureAsync(int characterId, string featureName)
    {
        return await _context.Features
            .AnyAsync(f => f.CharacterId == characterId && 
                          f.Name.ToLower() == featureName.ToLower());
    }

    public async Task<IEnumerable<Character>> GetCharactersWithExceededPointsAsync()
    {
        return await _context.Characters
            .Include(c => c.CombatAttributes)
            .Include(c => c.UtilityAttributes)
            .Where(c => 
                c.CombatAttributes.TotalPoints > c.Tier * 2 ||
                c.UtilityAttributes.TotalPoints > c.Tier)
            .ToListAsync();
    }

    public async Task<IEnumerable<Character>> GetCharactersWithInvalidArchetypesAsync()
    {
        return await _context.Characters
            .Include(c => c.Archetypes)
            .Where(c => !c.ValidateArchetypes())
            .ToListAsync();
    }

    public async Task<IEnumerable<Character>> GetCharactersNeedingRecalculationAsync()
    {
        // This could be based on a LastCalculated timestamp or version number
        return await _context.Characters
            .Include(c => c.CombatAttributes)
            .Include(c => c.UtilityAttributes)
            .Include(c => c.Archetypes)
            .ToListAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}