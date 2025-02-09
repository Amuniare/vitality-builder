using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VitalityBuilder.Api.Models;
using VitalityBuilder.Api.Infrastructure; 

namespace VitalityBuilder.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharactersController : ControllerBase
    {
        private readonly VitalityBuilderContext _context;
        private readonly ILogger<CharactersController> _logger;

        public CharactersController(VitalityBuilderContext context, ILogger<CharactersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<Character>> CreateCharacter(CreateCharacterDto dto)
        {
            _logger.LogInformation("Received character creation request for {CharacterName}", dto.Name);

            if (!ValidateCharacterDto(dto))
            {
                _logger.LogWarning("Character validation failed for {CharacterName}", dto.Name);
                return BadRequest("Invalid character data");
            }

            try 
            {
                var character = new Character
                {
                    Name = dto.Name,
                    Tier = dto.Tier,
                    MainPointPool = dto.MainPointPool,
                    SpecialAttacksPointPool = dto.SpecialAttacksPointPool,
                    UtilityPointPool = dto.UtilityPointPool,
                    CombatAttributes = new CombatAttributes
                    {
                        Focus = dto.CombatAttributes.Focus,
                        Power = dto.CombatAttributes.Power,
                        Mobility = dto.CombatAttributes.Mobility,
                        Endurance = dto.CombatAttributes.Endurance
                    },
                    UtilityAttributes = new UtilityAttributes
                    {
                        Awareness = dto.UtilityAttributes.Awareness,
                        Communication = dto.UtilityAttributes.Communication,
                        Intelligence = dto.UtilityAttributes.Intelligence
                    }
                };

                _context.Characters.Add(character);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully created character {CharacterName} with ID {CharacterId}", 
                    character.Name, character.Id);

                return CreatedAtAction(
                    nameof(GetCharacter),
                    new { id = character.Id },
                    character);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating character {CharacterName}", dto.Name);
                return StatusCode(500, "An error occurred while creating the character");
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Character>> GetCharacter(int id)
        {
            var character = await _context.Characters
                .Include(c => c.CombatAttributes)
                .Include(c => c.UtilityAttributes)
                .Include(c => c.Expertise)
                .Include(c => c.SpecialAttacks)
                .Include(c => c.UniquePowers)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (character == null)
            {
                return NotFound();
            }

            return character;
        }


        private static bool ValidateCharacterDto(CreateCharacterDto dto)
        {
            // Validate tier range
            if (dto.Tier < 1 || dto.Tier > 10)
                return false;

            // Combat attributes total must not exceed tier × 2
            int combatTotal = dto.CombatAttributes.Focus + dto.CombatAttributes.Power + 
                            dto.CombatAttributes.Mobility + dto.CombatAttributes.Endurance;
            if (combatTotal > dto.Tier * 2 ||
                dto.CombatAttributes.Focus > dto.Tier ||
                dto.CombatAttributes.Power > dto.Tier ||
                dto.CombatAttributes.Mobility > dto.Tier ||
                dto.CombatAttributes.Endurance > dto.Tier)
                return false;

            // Utility attributes total must not exceed tier
            int utilityTotal = dto.UtilityAttributes.Awareness + dto.UtilityAttributes.Communication + 
                             dto.UtilityAttributes.Intelligence;
            if (utilityTotal > dto.Tier ||
                dto.UtilityAttributes.Awareness > dto.Tier ||
                dto.UtilityAttributes.Communication > dto.Tier ||
                dto.UtilityAttributes.Intelligence > dto.Tier)
                return false;

            // Main pool calculation: (Tier - 2) × 15
            int expectedMainPool = Math.Max(0, (dto.Tier - 2) * 15);
            if (dto.MainPointPool > expectedMainPool)
                return false;

            // Special attacks count validation
            if (dto.SpecialAttacks.Count > dto.Tier)
                return false;

            // Expertise points validation: 5 × (Tier - 1)
            int maxExpertisePoints = 5 * (dto.Tier - 1);
            int usedExpertisePoints = dto.Expertise.Sum(e => e.Cost);
            if (usedExpertisePoints > maxExpertisePoints)
                return false;

            return true;
        }
    }
}