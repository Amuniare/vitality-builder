
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
│       └── DerivedStats.cs
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
│       └── Validators/
│           ├── CharacterValidator.cs
│           ├── CombatAttributesValidator.cs
│           └── UtilityAttributesValidator.cs
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


## Controllers Folder
Purpose: Handles incoming HTTP requests and manages API endpoints.

Files:
- CharacterController.cs
  - Manages character-related HTTP endpoints (GET, POST, PUT, DELETE)
  - Implements error handling for API responses
  - Validates incoming requests
  - Routes requests to appropriate services

## Domain Folder
Purpose: Contains core business logic and data structures.

### Attributes Folder
Contains core attribute definitions:
- CombatAttributes.cs
  - Defines Focus, Power, Mobility, Endurance
  - Includes validation rules for combat attributes
  - Manages combat attribute calculations

- UtilityAttributes.cs
  - Defines Awareness, Communication, Intelligence
  - Handles utility attribute validation
  - Contains utility calculation methods

### Character Folder
Core character-related entities:
- Character.cs
  - Main character entity definition
  - Manages relationships between character components
  - Contains basic character properties

- CharacterArchetypes.cs
  - Defines character archetype selections
  - Manages archetype-specific rules
  - Handles archetype relationships

### Constants Folder
Game rule constants and configurations:
- GameRuleConstants.cs
  - Defines base values (like movement speed)
  - Contains calculation constants
  - Stores game rule limitations

### DTOs Folder
Data transfer objects for API communication:
- Character/
  - CharacterBasicDto.cs: Basic character creation/update data
  - CharacterResponseDto.cs: Complete character response data

- Attributes/
  - CombatAttributesDto.cs: Combat attribute transfer
  - UtilityAttributesDto.cs: Utility attribute transfer

- Archetypes/
  - CharacterArchetypesDto.cs: Archetype selection data



## Infrastructure Folder
This folder manages our application's technical infrastructure - think of it as the foundation and plumbing of our system.

### Data Folder
This folder handles all database-related operations and configurations.

- Configurations/
  - CharacterConfiguration.cs
    - Defines how character entities map to database tables
    - Sets up relationships between character and its components
    - Configures database constraints and indexes
    - Example: Maps the Tier property to a database column with appropriate constraints

  - CombatAttributesConfiguration.cs
    - Configures combat attribute database mapping
    - Sets up validation rules at the database level
    - Manages relationships with the Character entity
    - Example: Ensures Focus cannot exceed Tier value

  - UtilityAttributesConfiguration.cs
    - Similar to CombatAttributes but for utility attributes
    - Manages database constraints for utility values
    - Sets up proper relationships and validations

- VitalityBuilderContext.cs
  - Central database context class
  - Manages database connections and transactions
  - Configures entity relationships
  - Sets up database-wide conventions
  - Example: Configuring automatic property value calculations

### Security Folder
Handles basic security measures to protect our application.

- InputSanitizer.cs
  - Cleans user input to prevent injection attacks
  - Validates input formats
  - Ensures data safety before processing
  - Example: Removing dangerous characters from text input

### Validation Folder
Manages all validation rules across the application.

- Validators/
  - CharacterValidator.cs
    - Validates complete character objects
    - Ensures game rules are followed
    - Checks relationships between components
    - Example: Validating total attribute points don't exceed limits

  - CombatAttributesValidator.cs
    - Specific validation for combat attributes
    - Ensures attribute values meet game rules
    - Validates calculations and relationships
    - Example: Ensuring Focus + Power + Mobility + Endurance ≤ Tier × 2

  - UtilityAttributesValidator.cs
    - Validates utility attribute rules
    - Ensures proper attribute relationships
    - Checks against game limitations
    - Example: Validating total utility points ≤ Tier

## Services Folder
This folder contains the business logic and calculations for our application.

### Calculations Folder
Handles all game-related calculations.

- CharacterStatCalculator.cs
  - Calculates derived statistics (Accuracy, Damage, etc.)
  - Handles archetype modifications
  - Manages stat interactions
  - Example: Calculating Avoidance = 10 + Tier + Mobility

- PointPoolCalculator.cs
  - Manages point pool calculations
  - Tracks point expenditure
  - Handles archetype modifications to pools
  - Example: Calculating main point pool = (Tier - 2) × 15

### Character Folder
Manages character operations.

- CharacterCreationService.cs
  - Handles character creation process
  - Validates initial character setup
  - Sets default values
  - Example: Setting up initial attribute pools

- CharacterUpdateService.cs
  - Manages character modifications
  - Handles attribute updates
  - Processes archetype changes
  - Example: Updating derived stats when attributes change

### Validation Folder
Centralizes validation services.

- ValidationService.cs
  - Coordinates validation across the application
  - Manages validation flow
  - Combines multiple validation rules
  - Example: Validating a character update across all components

## Interfaces Folder
Defines contracts for our services and repositories.

### Repositories Folder
- ICharacterRepository.cs
  - Defines data access methods
  - Specifies CRUD operations
  - Sets up query interfaces
  - Example: Defining methods for fetching and updating characters

### Services Folder
- ICharacterStatCalculator.cs
  - Defines calculation method contracts
  - Specifies required calculation interfaces
  - Sets up stat computation contracts
  - Example: Defining methods for calculating derived stats

- IPointPoolCalculator.cs
  - Defines point calculation contracts
  - Specifies pool management interfaces
  - Sets up point tracking methods
  - Example: Defining methods for calculating available points






# Part 1: Component Interactions

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

The system remains simple enough for a small user base while maintaining proper software development practices.