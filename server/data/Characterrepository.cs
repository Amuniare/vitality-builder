public class CharacterRepository : ICharacterRepository
{
    private readonly AppDbContext _context;

    public CharacterRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Character> CreateCharacterAsync(Character character)
    {
        _context.Characters.Add(character);
        await _context.SaveChangesAsync();
        return character;
    }

    public async Task<List<Character>> GetAllCharactersAsync()
    {
        return await _context.Characters.ToListAsync();
    }

    public async Task<Character?> GetCharacterByIdAsync(int id)
    {
        return await _context.Characters.FindAsync(id);
    }
}