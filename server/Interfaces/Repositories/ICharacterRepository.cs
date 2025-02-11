using VitalityBuilder.Api.Models.Entities;
public interface ICharacterRepository
{
    Task<Character> CreateCharacterAsync(Character character);
    Task<List<Character>> GetAllCharactersAsync();
    Task<Character?> GetCharacterByIdAsync(int id);
}