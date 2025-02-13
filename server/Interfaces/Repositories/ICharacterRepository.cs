using VitalityBuilder.Domain.Character;

namespace VitalityBuilder.Interfaces.Repositories;

/// <summary>
/// Repository interface for character data access operations
/// </summary>
public interface ICharacterRepository
{
    /// <summary>
    /// Gets a character by ID
    /// </summary>
    Task<Character?> GetCharacterAsync(int id);

    /// <summary>
    /// Gets a character with all related entities
    /// </summary>
    Task<Character?> GetCharacterWithDetailsAsync(int id);

    /// <summary>
    /// Checks if a character exists
    /// </summary>
    Task<bool> CharacterExistsAsync(int id);

    /// <summary>
    /// Checks if a character name is already taken
    /// </summary>
    Task<bool> NameExistsAsync(string name);

    /// <summary>
    /// Gets characters matching specified criteria
    /// </summary>
    Task<IEnumerable<Character>> GetCharactersAsync(
        int? minTier = null,
        int? maxTier = null,
        string? nameSearch = null,
        int? skip = null,
        int? take = null);

    /// <summary>
    /// Gets the total count of characters matching criteria
    /// </summary>
    Task<int> GetCharacterCountAsync(
        int? minTier = null,
        int? maxTier = null,
        string? nameSearch = null);

    /// <summary>
    /// Adds a new character
    /// </summary>
    Task AddCharacterAsync(Character character);

    /// <summary>
    /// Updates an existing character
    /// </summary>
    void UpdateCharacter(Character character);

    /// <summary>
    /// Marks a character as deleted
    /// </summary>
    Task DeleteCharacterAsync(int id);

    /// <summary>
    /// Gets a character's special attacks
    /// </summary>
    Task<IEnumerable<SpecialAttack>> GetSpecialAttacksAsync(int characterId);

    /// <summary>
    /// Gets a character's features
    /// </summary>
    Task<IEnumerable<CharacterFeature>> GetFeaturesAsync(int characterId);

    /// <summary>
    /// Gets a character's expertise
    /// </summary>
    Task<IEnumerable<CharacterExpertise>> GetExpertiseAsync(int characterId);

    /// <summary>
    /// Checks if a character has specific features
    /// </summary>
    Task<bool> HasFeatureAsync(int characterId, string featureName);

    /// <summary>
    /// Gets characters with point allocations exceeding their tier limits
    /// </summary>
    Task<IEnumerable<Character>> GetCharactersWithExceededPointsAsync();

    /// <summary>
    /// Gets characters with invalid archetype combinations
    /// </summary>
    Task<IEnumerable<Character>> GetCharactersWithInvalidArchetypesAsync();

    /// <summary>
    /// Gets characters that need recalculation due to rule changes
    /// </summary>
    Task<IEnumerable<Character>> GetCharactersNeedingRecalculationAsync();

    /// <summary>
    /// Saves all changes to the database
    /// </summary>
    Task<int> SaveChangesAsync();
}