using VitalityBuilder.Domain.Character;
using VitalityBuilder.Domain.Dtos.Character;
using VitalityBuilder.Domain.Errors;
using VitalityBuilder.Infrastructure.Security;
using VitalityBuilder.Interfaces.Services;

namespace VitalityBuilder.Services.Character;

public class CharacterCreationService : ICharacterCreationService
{
    private readonly ICharacterRepository _repository;
    private readonly ICharacterStatCalculator _statCalculator;
    private readonly IPointPoolCalculator _pointCalculator;
    private readonly IValidationService _validationService;
    private readonly ILogger<CharacterCreationService> _logger;

    public CharacterCreationService(
        ICharacterRepository repository,
        ICharacterStatCalculator statCalculator,
        IPointPoolCalculator pointCalculator,
        IValidationService validationService,
        ILogger<CharacterCreationService> logger)
    {
        _repository = repository;
        _statCalculator = statCalculator;
        _pointCalculator = pointCalculator;
        _validationService = validationService;
        _logger = logger;
    }

    public async Task<CharacterResponseDto> CreateCharacterAsync(CreateCharacterDto request)
    {
        try
        {
            // Validate the request
            var validationResult = await _validationService.ValidateCharacterCreation(request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Character creation validation failed: {Errors}", 
                    string.Join(", ", validationResult.Errors));
                throw new ValidationException(validationResult.Errors);
            }

            // Sanitize input
            var sanitizedName = InputSanitizer.SanitizeName(request.Name);

            // Create character entity
            var character = new Domain.Character.Character
            {
                Name = sanitizedName,
                Tier = request.Tier,
                CombatAttributes = new()
                {
                    Focus = request.Focus,
                    Power = request.Power,
                    Mobility = request.Mobility,
                    Endurance = request.Endurance
                },
                UtilityAttributes = new()
                {
                    Awareness = request.Awareness,
                    Communication = request.Communication,
                    Intelligence = request.Intelligence
                },
                Archetypes = new()
                {
                    MovementType = Enum.Parse<MovementArchetype>(request.MovementArchetype),
                    AttackType = Enum.Parse<AttackArchetype>(request.AttackArchetype),
                    EffectType = Enum.Parse<EffectArchetype>(request.EffectArchetype),
                    UniqueAbility = Enum.Parse<UniqueAbilityArchetype>(request.UniqueAbilityArchetype),
                    SpecialAttack = Enum.Parse<SpecialAttackArchetype>(request.SpecialAttackArchetype),
                    UtilityType = Enum.Parse<UtilityArchetype>(request.UtilityArchetype)
                }
            };

            // Calculate initial stats
            var stats = _statCalculator.CalculateAllStats(character);
            var points = _pointCalculator.CalculateAllPools(character);

            // Validate point allocation
            var pointValidation = _pointCalculator.ValidatePointAllocation(character);
            if (!pointValidation.IsValid)
            {
                throw new ValidationException(pointValidation.Errors);
            }

            // Save character
            await _repository.AddCharacterAsync(character);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Created character {Id} with name {Name}", 
                character.Id, character.Name);

            // Create response DTO
            var response = new CharacterResponseDto
            {
                Id = character.Id,
                Name = character.Name,
                Tier = character.Tier,
                CombatAttributes = new()
                {
                    Focus = character.CombatAttributes.Focus,
                    Power = character.CombatAttributes.Power,
                    Mobility = character.CombatAttributes.Mobility,
                    Endurance = character.CombatAttributes.Endurance,
                    Values = new()
                    {
                        Avoidance = stats.Avoidance,
                        Durability = stats.Durability,
                        ResolveResistance = stats.ResolveResistance,
                        StabilityResistance = stats.StabilityResistance,
                        VitalityResistance = stats.VitalityResistance
                    }
                },
                UtilityAttributes = new()
                {
                    Awareness = character.UtilityAttributes.Awareness,
                    Communication = character.UtilityAttributes.Communication,
                    Intelligence = character.UtilityAttributes.Intelligence
                },
                Archetypes = new()
                {
                    MovementType = character.Archetypes.MovementType.ToString(),
                    AttackType = character.Archetypes.AttackType.ToString(),
                    EffectType = character.Archetypes.EffectType.ToString(),
                    UniqueAbility = character.Archetypes.UniqueAbility.ToString(),
                    SpecialAttack = character.Archetypes.SpecialAttack.ToString(),
                    UtilityType = character.Archetypes.UtilityType.ToString()
                },
                PointPools = new()
                {
                    MainPool = points.MainPool,
                    UtilityPoints = points.UtilityPoints,
                    CombatAttributePoints = points.CombatAttributePoints,
                    UtilityAttributePoints = points.UtilityAttributePoints,
                    SpecialAttackPoints = points.SpecialAttackPoints
                },
                Warnings = pointValidation.Warnings.ToList()
            };

            return response;
        }
        catch (Exception ex) when (ex is not ValidationException)
        {
            _logger.LogError(ex, "Error creating character");
            throw;
        }
    }
}