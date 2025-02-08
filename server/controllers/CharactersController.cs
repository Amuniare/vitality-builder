using Microsoft.AspNetCore.Mvc;
using VitalityBuilder.Api.Data;
using VitalityBuilder.Api.Models;

namespace VitalityBuilder.Api.Controllers
{
    /// <summary>
    /// Manages RPG characters in the Vitality system
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CharactersController : ControllerBase
    {
        private readonly ICharacterRepository _repository;

        public CharactersController(ICharacterRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves all characters
        /// </summary>
        /// <response code="200">Returns list of characters</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCharacters()
        {
            var characters = await _repository.GetAllCharactersAsync();
            return Ok(characters);
        }

        /// <summary>
        /// Creates a new character
        /// </summary>
        /// <param name="character">Character data</param>
        /// <response code="201">Returns the newly created character</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateCharacter(Character character)
        {
            var createdCharacter = await _repository.CreateCharacterAsync(character);
            return CreatedAtAction(nameof(GetCharacterById), 
                new { id = createdCharacter.Id }, createdCharacter);
        }

        /// <summary>
        /// Gets a specific character by ID
        /// </summary>
        /// <param name="id">Character identifier</param>
        /// <response code="200">Returns the requested character</response>
        /// <response code="404">Character not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCharacterById(int id)
        {
            var character = await _repository.GetCharacterByIdAsync(id);
            return character != null ? Ok(character) : NotFound();
        }
    }
}