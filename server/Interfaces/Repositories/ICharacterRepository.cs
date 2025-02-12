using VitalityBuilder.Api.Models.Entities;
using VitalityBuilder.Api.Infrastructure;

namespace Server.Repositories;
public class CharacterRepository : ICharacterRepository
{
    private readonly VitalityBuilderContext _context;
    public CharacterRepository(VitalityBuilderContext context) => _context = context;

    public async Task<CharacterEntity> CreateCharacterAsync(CharacterEntity character)
    {
        _context.Characters.Add(character);
        await _context.SaveChangesAsync();
        return character;
    }

    public async Task<List<CharacterEntity>> GetAllCharactersAsync() 
        => await _context.Characters.ToListAsync();

    public async Task<CharacterEntity?> GetCharacterByIdAsync(int id) 
        => await _context.Characters.FindAsync(id);
}