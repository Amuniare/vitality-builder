
# System Architecture Overview
The backend uses a three-tier architecture organized around clear responsibility separation:

## Presentation Layer (DTOs)
This layer handles data transfer between the client and server. Think of DTOs as specialized envelopes that package data in exactly the format needed for transmission. They don't contain business logic - they just carry data.

## Data Layer (Entities)
This layer manages data persistence and storage. Entities represent our core database tables and relationships. They store the fundamental character data that everything else builds upon.

## Service Layer (Calculators and Business Logic)
This layer contains all our game rules and calculations. When a user makes changes in the interface, the service layer determines how those changes affect other aspects of the character.

## Directory Tree

```
Server/
├── Controllers/
│   └── CharacterController.cs
│
├── Domain/
│   ├── Attributes/
│   │   ├── CombatAttributes.cs
│   │   └── UtilityAttributes.cs
│   │
│   ├── Character/
│   │   ├── Character.cs
│   │   └── CharacterArchetypes.cs
│   │
│   ├── Constants/
│   │   └── GameRuleConstants.cs
│   │
│   ├── Dtos/
│   │   ├── Character/
│   │   │   ├── CharacterBasicDto.cs
│   │   │   └── CharacterResponseDto.cs
│   │   ├── Attributes/
│   │   │   ├── CombatAttributesDto.cs
│   │   │   └── UtilityAttributesDto.cs
│   │   └── Archetypes/
│   │       └── CharacterArchetypesDto.cs
│   │
│   ├── Enums/
│   │   └── ArchetypeEnums.cs
│   │
│   ├── Errors/
│   │   ├── ErrorResponse.cs
│   │   └── ValidationResult.cs
│   │
│   └── ValueObjects/
│       └── CombatStats.cs
│
├── Infrastructure/
│   ├── Data/
│   │   ├── Configurations/
│   │   │   ├── CharacterConfiguration.cs
│   │   │   ├── CombatAttributesConfiguration.cs
│   │   │   └── UtilityAttributesConfiguration.cs
│   │   │
│   │   ├── Migrations/
│   │   └── VitalityBuilderContext.cs
│   │
│   ├── Security/
│   │   └── InputSanitizer.cs
│   │
│   └── Validation/
│       ├── CharacterValidator.cs
│       ├── CombatAttributesValidator.cs
│       └── UtilityAttributesValidator.cs
│
├── Services/
│   ├── Calculations/
│   │   ├── CharacterStatCalculator.cs
│   │   └── PointPoolCalculator.cs
│   │
│   ├── Character/
│   │   ├── CharacterCreationService.cs
│   │   └── CharacterUpdateService.cs
│   │
│   └── Validation/
│       └── ValidationService.cs
│
└── Interfaces/
    ├── Repositories/
    │   └── ICharacterRepository.cs
    └── Services/
        ├── ICharacterStatCalculator.cs
        └── IPointPoolCalculator.cs
```





# Folder Architecture and Data Flow Analysis

## Controllers Folder
The Controllers folder acts as the gateway for all external communication with our application. Think of it as the front desk at a hotel - it's where all requests first arrive and get directed to the right place.

### Data Sources
- HTTP requests from client applications
- Query parameters from URLs
- Request bodies containing character data
- Authentication tokens and headers
- API version information

### Data Destinations
- Routes requests to appropriate services
- Returns HTTP responses to clients
- Sends validation errors back to users
- Forwards logging information
- Triggers event notifications

### Example Flow
When a request comes in to update a character's attributes:
```csharp
[HttpPut("characters/{id}/attributes")]
public async Task<IActionResult> UpdateAttributes(
    int id, 
    UpdateAttributesRequest request)
{
    // First, validate the incoming request
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    // Forward to service layer for processing
    var result = await _characterService
        .UpdateAttributesAsync(id, request);

    // Return appropriate response
    return Ok(result);
}
```

## Domain Folder
The Domain folder contains the core business logic and data structures of our application. It's like the blueprint room of a construction company - it defines what everything should look like and how it should work.

### Data Sources
- Entity definitions from code
- Game rule constants
- Business logic requirements
- Data validation rules
- Relationship definitions

### Data Destinations
- Database structure through Entity Framework
- Service layer implementations
- Validation rule enforcement
- Response data formatting
- Calculation systems

### Key Components
- Entities: Define data structure
- DTOs: Handle data transfer
- Enums: Define fixed options
- Constants: Store game rules
- Value Objects: Handle complex values

### Example Structure
```csharp
public class Character
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Tier { get; set; }
    public CombatAttributes CombatAttributes { get; set; }
    public UtilityAttributes UtilityAttributes { get; set; }
    public CharacterArchetypes Archetypes { get; set; }
    
    public virtual ICollection<SpecialAttack> SpecialAttacks { get; set; }
}
```

## Infrastructure Folder
The Infrastructure folder handles all the technical underpinnings of our application. It's like the building's foundation and utility systems - you don't see it directly, but everything depends on it.

### Data Sources
- Database configuration settings
- Entity Framework mappings
- Security requirements
- Validation rules
- Logging configuration

### Data Destinations
- Database operations
- Security enforcement
- Input validation
- Error logging
- Performance monitoring

### Key Components
- Data: Database context and configurations
- Security: Input sanitization and protection
- Validation: Rule enforcement
- Logging: System monitoring

### Example Configuration
```csharp
public class CharacterConfiguration 
    : IEntityTypeConfiguration<Character>
{
    public void Configure(
        EntityTypeBuilder<Character> builder)
    {
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.HasOne(c => c.CombatAttributes)
            .WithOne()
            .HasForeignKey<CombatAttributes>("CharacterId");
    }
}
```

## Services Folder
The Services folder contains all the business logic implementation. It's like the staff of our hotel - they take requests from the front desk and actually do the work.

### Data Sources
- Requests from controllers
- Data from repositories
- Game rule constants
- Configuration settings
- User input data

### Data Destinations
- Database updates through repositories
- Response data to controllers
- Validation results
- Event notifications
- Logging system

### Key Components
- Character: Character management
- Combat: Combat resolution
- Calculations: Game rule math
- Validation: Business rule checking

### Example Service
```csharp
public class CharacterService : ICharacterService
{
    private readonly ICharacterRepository _repository;
    private readonly ICalculationService _calculator;
    private readonly IValidationService _validator;

    public async Task<Character> UpdateAttributesAsync(
        int id, 
        UpdateAttributesRequest request)
    {
        var character = await _repository
            .GetCharacterAsync(id);
            
        // Update attributes
        _mapper.Map(request, character.CombatAttributes);
        
        // Recalculate derived stats
        await _calculator
            .RecalculateStatsAsync(character);
            
        // Save changes
        await _repository.SaveChangesAsync();
        
        return character;
    }
}
```

## Interfaces Folder
The Interfaces folder defines the contracts that our services must follow. It's like having a standardized set of job descriptions - it ensures everyone knows exactly what they're supposed to do.

### Data Sources
- Service contract definitions
- Method signatures
- Parameter specifications
- Return type definitions
- Dependency declarations

### Data Destinations
- Service implementations
- Dependency injection system
- Unit test mocks
- Documentation generation
- API contracts

### Key Components
- Repositories: Data access contracts
- Services: Business logic contracts
- Calculators: Math operation contracts

### Example Interface
```csharp
public interface ICharacterService
{
    Task<Character> GetCharacterAsync(int id);
    Task<Character> CreateCharacterAsync(
        CreateCharacterRequest request);
    Task<Character> UpdateAttributesAsync(
        int id, 
        UpdateAttributesRequest request);
    Task DeleteCharacterAsync(int id);
}
```

## Data Flow Example
Let's follow a complete request through the system:

1. Client sends PUT request to update character attributes
2. Controllers folder receives request and validates basic format
3. Request passes to Services folder for processing
4. Services use Interfaces to access repositories
5. Domain models define data structure
6. Infrastructure handles database operations
7. Results flow back through services to controller
8. Controller returns response to client

```csharp
// Flow example with logging
public async Task<IActionResult> UpdateCharacter(
    int id, 
    UpdateCharacterRequest request)
{
    try 
    {
        _logger.LogInformation(
            "Updating character {Id}", id);
            
        // Controller validates request
        if (!ModelState.IsValid)
        {
            _logger.LogWarning(
                "Invalid request for character {Id}", id);
            return BadRequest(ModelState);
        }
        
        // Service processes update
        var result = await _characterService
            .UpdateCharacterAsync(id, request);
            
        // Infrastructure logs success
        _logger.LogInformation(
            "Successfully updated character {Id}", id);
            
        // Controller returns response
        return Ok(result);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, 
            "Error updating character {Id}", id);
        return StatusCode(500, "Internal server error");
    }
}
```

This architecture ensures:
- Clear separation of concerns
- Consistent data flow patterns
- Proper error handling
- Comprehensive logging
- Maintainable codebase
- Testable components
- Scalable structure

Each folder has a distinct responsibility, but they work together seamlessly to handle requests, process data, and maintain system integrity.




# Component Interactions

## Simple Interaction

Each of these components works together to create a cohesive system. For example, when updating a character's attributes:
1. The Controller receives the request
2. Validation Services check the input
3. The Character Service processes the update
4. Calculators update derived stats
5. The Repository saves the changes
6. The Controller returns the updated character via DTOs



## More Complex Interactions

Scenario: A user increases their character's Tier from 3 to 4. This change triggers multiple updates throughout the system.


1. Initial Request Handling:
```csharp
// CharacterController.cs
public async Task<IActionResult> UpdateTier(int characterId, int newTier)
{
    try 
    {
        // First, validate the basic request
        if (newTier < 1 || newTier > 10)
        {
            return BadRequest("Tier must be between 1 and 10");
        }

        // Begin the update process
        var result = await _characterService.UpdateTierAsync(characterId, newTier);
        
        // Return the updated character with all recalculated values
        return Ok(_mapper.Map<CharacterResponseDto>(result));
    }
    catch (ValidationException ex)
    {
        // Handle validation failures
        return BadRequest(ex.Message);
    }
}
```

2. Character Service Processing:
```csharp
// CharacterUpdateService.cs
public async Task<Character> UpdateTierAsync(int characterId, int newTier)
{
    var character = await _repository.GetCharacterAsync(characterId);
    var oldTier = character.Tier;
    
    // Update tier and trigger recalculations
    character.Tier = newTier;
    
    // Recalculate point pools
    await _pointPoolCalculator.RecalculatePoolsAsync(character);
    
    // Validate attributes against new tier
    var attributeValidation = await ValidateAttributesForNewTier(character, oldTier);
    if (!attributeValidation.IsValid)
    {
        // Mark attributes that now exceed limits but don't prevent the change
        character.AddValidationWarnings(attributeValidation.Warnings);
    }
    
    // Recalculate all derived statistics
    await _statCalculator.RecalculateAllStatsAsync(character);
    
    return character;
}
```

3. Point Pool Recalculation:
```csharp
// PointPoolCalculator.cs
public async Task RecalculatePoolsAsync(Character character)
{
    // Calculate new point pools
    character.MainPointPool = (character.Tier - 2) * 15;
    character.UtilityPointPool = 5 * (character.Tier - 1);
    
    // Calculate special attacks pool based on archetype
    character.SpecialAttacksPointPool = CalculateSpecialAttackPoints(
        character.Tier,
        character.Archetypes.SpecialAttackArchetype
    );
    
    // Validate existing purchases against new pools
    var poolValidation = ValidateExistingPurchases(character);
    if (!poolValidation.IsValid)
    {
        character.AddValidationWarnings(poolValidation.Warnings);
    }
}
```

# Game Mechanics Implementation

Let's examine how our architecture handles complex game mechanics like Special Attacks and their interactions with archetypes.

## Special Attack System Implementation

1. Special Attack Creation:
```csharp
// SpecialAttackService.cs
public async Task<SpecialAttack> CreateSpecialAttackAsync(
    Character character, 
    CreateSpecialAttackDto request)
{
    // Calculate base cost based on attack type
    var baseCost = CalculateBaseCost(request.AttackType);
    
    // Apply archetype modifications
    if (character.Archetypes.AttackType.MatchesAttackType(request.AttackType))
    {
        baseCost = 0; // Attack type is free due to archetype
    }
    
    // Calculate limit points
    var limitPoints = CalculateLimitPoints(
        request.Limits,
        character.Tier,
        character.Archetypes.SpecialAttackArchetype
    );
    
    // Create the special attack
    var specialAttack = new SpecialAttack
    {
        Name = request.Name,
        AttackType = request.AttackType,
        BaseCost = baseCost,
        LimitPoints = limitPoints,
        // ... additional properties
    };
    
    return specialAttack;
}
```

2. Combat Resolution:
```csharp
// CharacterStatCalculator.cs
public CombatStats CalculateCombatStats(Character character)
{
    var stats = new CombatStats();
    
    // Base accuracy calculation
    stats.Accuracy = character.Tier + character.CombatAttributes.Focus;
    
    // Apply archetype modifications
    if (character.Archetypes.AttackType == AttackTypeCategory.AOESpecialist)
    {
        stats.Accuracy -= character.Tier; // AOE penalty
    }
    
    // Calculate damage
    stats.Damage = character.Tier + 
        Math.Ceiling(character.CombatAttributes.Power * 1.5);
    
    if (character.Archetypes.EffectType == EffectTypeCategory.HybridSpecialist)
    {
        stats.Damage -= character.Tier; // Hybrid penalty
    }
    
    return stats;
}
```

3. Movement and Positioning:
```csharp
// MovementCalculator.cs
public MovementStats CalculateMovement(Character character)
{
    var stats = new MovementStats();
    
    // Calculate base movement
    int baseMovement = Math.Max(
        6 + character.CombatAttributes.Mobility,
        character.CombatAttributes.Mobility + character.Tier
    );
    
    // Apply archetype modifications
    switch (character.Archetypes.MovementType)
    {
        case MovementArchetypeType.Swift:
            stats.MovementSpeed = baseMovement + 
                Math.Ceiling(character.Tier / 2.0);
            break;
            
        case MovementArchetypeType.Vanguard:
            stats.MovementSpeed = baseMovement + 
                character.CombatAttributes.Endurance;
            break;
            
        default:
            stats.MovementSpeed = baseMovement;
            break;
    }
    
    return stats;
}
```




# Character Data Architecture

## Basic Character Information

### Character Name
Storage Location: Character entity in database
Data Type: String field
Validations: 
- Located in CreateCharacterDtoValidator
- Ensures name is not empty and under 100 characters

### Character Tier
Storage Location: Character entity in database
Data Type: Integer field
Validations:
- Located in CreateCharacterDtoValidator
- Ensures value between 1-10
Dependencies Created:
- Affects all point pool calculations in PointPoolCalculator
- Affects all derived stat calculations in CharacterStatCalculator
- Limits maximum values for all attributes in CombatAttributesDtoValidator and UtilityAttributesDtoValidator

## Core Attribute System

### Combat Attributes
Storage Location: CombatAttributes entity in database
Calculation Management: CharacterStatCalculator service
Validation Location: CombatAttributesDtoValidator

Individual attributes (Focus, Power, Mobility, Endurance):
- Data Type: Integer fields
- Validation Rules:
  - Cannot exceed character's Tier
  - Sum cannot exceed Tier × 2
- Dependencies Created:
  - Focus affects Accuracy and Resolve Resistance
  - Power affects Damage and Stability Resistance
  - Mobility affects Movement Speed and Avoidance
  - Endurance affects Durability and Vitality Resistance

### Utility Attributes
Storage Location: UtilityAttributes entity in database
Calculation Management: CharacterStatCalculator service
Validation Location: UtilityAttributesDtoValidator

Individual attributes (Awareness, Communication, Intelligence):
- Data Type: Integer fields
- Validation Rules:
  - Cannot exceed character's Tier
  - Sum cannot exceed Tier
- Dependencies Created:
  - Awareness affects Initiative
  - Communication affects social interactions
  - Intelligence affects knowledge checks

## Archetype System

### Archetype Selections
Storage Location: CharacterArchetypes entity in database
Available Options: Defined in ArchetypeEnums
Validation Location: ArchetypesValidator

Each archetype type (Attack, Effect, Movement, etc.):
- Data Type: Enum value
- Options Source: ArchetypeEnums.cs
- Validation Rules: Must select one from each category
- Dependencies Created:
  - Attack Type modifies accuracy calculations
  - Effect Type modifies damage and condition calculations
  - Movement Type modifies movement calculations

## Derived Statistics System

### Combat Statistics
Calculation Location: CharacterStatCalculator service
Storage Location: Calculated on demand, not stored

Individual statistics:
```csharp
// Example calculation from CharacterStatCalculator
public int CalculateAccuracy(Character character)
{
    int baseAccuracy = character.Tier + character.CombatAttributes.Focus;
    
    // Apply archetype modifications
    if (character.Archetypes.AttackType == AttackTypeCategory.AOESpecialist)
    {
        baseAccuracy -= character.Tier;
    }
    
    return baseAccuracy;
}
```

### Defensive Values
Calculation Location: CharacterStatCalculator service
Storage Location: Calculated on demand, not stored

Example calculation flow:
```csharp
public double CalculateDurability(Character character)
{
    return character.Tier + (Math.Ceiling(character.CombatAttributes.Endurance * 1.5));
}
```

## Point Pool System

### Pool Calculations
Calculation Location: PointPoolCalculator service
Storage Location: Point totals stored in Character entity

Example calculations:
```csharp
public int CalculateCorePointPool(Character character)
{
    int basePool = (character.Tier - 2) * 15;
    
    // Add archetype modifications
    if (character.Archetypes.UniqueAbility == UniqueAbilityCategory.Extraordinary)
    {
        basePool += (character.Tier - 2) * 15;
    }
    
    return basePool;
}

public int CalculateSpentPoints(Character character)
{
    return character.Purchases.Sum(p => p.Cost);
}
```




# Additional Architecture Components

## Error Handling Strategy

Our error handling strategy focuses on providing clear feedback while maintaining system stability. We implement this across three layers:

### Controller Layer Error Handling
The controller layer acts as our first line of defense, handling API request validation and providing consistent error responses. Each controller method follows this pattern:

```csharp
public async Task<IActionResult> UpdateCharacter(UpdateCharacterDto request)
{
    try 
    {
        // Data validation
        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorResponse {
                Type = "ValidationError",
                Message = "Invalid character data",
                Details = ModelState.GetErrors()
            });
        }

        // Business rule validation
        var validationResult = await _characterValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ErrorResponse {
                Type = "BusinessRuleError",
                Message = "Character update violates game rules",
                Details = validationResult.Errors
            });
        }

        var result = await _characterService.UpdateCharacterAsync(request);
        return Ok(result);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error updating character");
        return StatusCode(500, new ErrorResponse {
            Type = "SystemError",
            Message = "An unexpected error occurred"
        });
    }
}
```

### Service Layer Validation
The service layer implements our business rule validation, ensuring game rules are consistently enforced:

```csharp
public class CharacterValidator : AbstractValidator<Character>
{
    public CharacterValidator()
    {
        RuleFor(c => c.CombatAttributes)
            .Must((character, attributes) => 
                attributes.Total <= character.Tier * 2)
            .WithMessage("Combat attributes cannot exceed Tier × 2");

        RuleFor(c => c.CombatAttributes.Focus)
            .Must((character, focus) => 
                focus <= character.Tier)
            .WithMessage("Focus cannot exceed character tier");
        
        // Additional rules...
    }
}
```

## Data Change Management

Our system handles complex data changes through a coordinated update process. When modifying character attributes, we:

1. Accept the change attempt
2. Validate the new state
3. Show immediate visual feedback
4. Apply or reject changes based on validation

For example, when a user changes their character's tier:

```csharp
public async Task<CharacterUpdateResult> UpdateTier(int characterId, int newTier)
{
    var character = await _repository.GetCharacterAsync(characterId);
    var currentState = new CharacterState(character);
    
    // Update tier and recalculate dependent values
    character.Tier = newTier;
    character.RecalculatePointPools();
    character.ValidateAttributeLimits();
    
    // Collect all validation messages
    var validationResults = new ValidationResults {
        AttributeWarnings = character.GetAttributeWarnings(),
        PointPoolWarnings = character.GetPointPoolWarnings()
    };
    
    return new CharacterUpdateResult {
        IsValid = validationResults.HasNoErrors,
        ValidationResults = validationResults,
        UpdatedCharacter = character
    };
}
```

## Security Implementation

We implement essential security measures while keeping the system simple:

### Input Sanitization
```csharp
public static class InputSanitizer
{
    public static string SanitizeInput(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        
        // Remove potentially dangerous characters
        return Regex.Replace(input, @"[<>()&]", "");
    }
}
```

### Request Validation
All requests are validated server-side:

```csharp
public class CreateCharacterDtoValidator : AbstractValidator<CreateCharacterDto>
{
    public CreateCharacterDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .Matches(@"^[a-zA-Z0-9\s-]*$")
            .WithMessage("Character name can only contain letters, numbers, spaces, and hyphens");

        RuleFor(x => x.Tier)
            .InclusiveBetween(1, 10);
            
        // Additional validation rules...
    }
}
```

### API Documentation

Our API documentation is implemented using Swagger/OpenAPI:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Vitality Character Builder API",
            Version = "v1",
            Description = "API for managing Vitality System characters"
        });
        
        // Add XML comments to Swagger
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    });
}
```

Example controller documentation:

```csharp
/// <summary>
/// Creates a new character with the specified attributes
/// </summary>
/// <param name="request">Character creation details</param>
/// <returns>The created character or validation errors</returns>
/// <response code="201">Character created successfully</response>
/// <response code="400">Invalid character data</response>
[HttpPost]
[ProducesResponseType(typeof(CharacterResponseDto), StatusCodes.Status201Created)]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
public async Task<IActionResult> CreateCharacter(CreateCharacterDto request)
{
    // Implementation...
}
```

These additions complete our architecture by providing:
- Consistent error handling across all layers
- Clear validation feedback for users
- Basic security protections
- Comprehensive API documentation

