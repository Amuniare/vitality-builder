

**Server/**
**Program.cs**
**Purpose:**
- Functions as the application's entry point and central configuration hub. Think of this file as the conductor of an orchestra - it ensures all parts of our application start up in the right order and work together harmoniously. When you start the application, this is the first file that runs, and it sets up everything else needed for the system to function.

**Data Origin:**
- Reads crucial configuration data from the Server/appsettings.json file, which contains our database connection strings, logging preferences, and other application-wide settings
- Collects environment variables from the hosting system that help determine how the application should behave in different scenarios (development, testing, production)
- Takes in command-line arguments that might modify startup behavior, such as specifying different configuration files or setting specific flags
- Processes service registrations and middleware configurations defined in our codebase

**Where Data is Going:**
- Sets up our dependency injection container, which helps different parts of our application find and use each other efficiently
- Establishes the database connection by configuring our VitalityBuilderContext with the right settings and connection strings
- Creates the HTTP request pipeline that determines how our application will handle web requests
- Makes all configured services available throughout the application's lifetime, ensuring components can access what they need when they need it



# Additional File Descriptions

## Server/Controllers/CharacterController.cs
**Purpose:**
Serves as the primary entry point for all character-related HTTP requests. This controller manages the REST API endpoints that clients use to interact with character data. Think of it as the front desk of our application - it receives requests, ensures they're properly formatted, routes them to the appropriate services, and returns well-structured responses.

**Data Origin:**
- HTTP requests from client applications
- Query parameters for filtering and pagination
- Request bodies containing character data
- Authentication and authorization headers
- API version information

**Where Data is Going:**
- Character services for business logic processing
- Validation services for request verification
- Response formatting for client consumption
- Error handling for failed requests
- Logging system for request tracking

## Server/Services/Combat/CombatService.cs
**Purpose:**
Manages all combat-related calculations and resolutions in the system. This service handles the complex interactions between characters during combat scenarios, including attack resolution, damage calculation, and condition application. It acts as the referee during combat, ensuring all rules are properly applied.

**Data Origin:**
- Character combat statistics
- Attack and defense calculations
- Combat condition states
- Special ability triggers
- Archetype combat modifications
- Environmental factors

**Where Data is Going:**
- Combat resolution results
- Character state updates
- Event logging system
- Combat history tracking
- Client feedback messages

## Server/Services/Combat/DiceRollingService.cs
**Purpose:**
Provides a centralized system for handling all dice roll calculations in the game. This service ensures consistent and fair random number generation for various game mechanics, from combat rolls to skill checks. It maintains the integrity of the game's probability-based systems.

**Data Origin:**
- Roll type specifications (d20, 3d6, etc.)
- Advantage/disadvantage states
- Critical hit thresholds
- Tier-based modifiers
- Special ability effects on rolls

**Where Data is Going:**
- Roll results to combat service
- Critical hit determinations
- Roll history for logging
- Roll verification system
- Statistical tracking

## Server/Interfaces/Services/ICharacterService.cs
**Purpose:**
Defines the contract for character management operations. This interface ensures consistent character manipulation across the application by establishing standard methods for character creation, updating, and retrieval. It acts as a blueprint for implementing character-related services.

**Data Origin:**
- Character creation requests
- Update operation specifications
- Query parameters
- Character state information
- Validation requirements

**Where Data is Going:**
- Service implementations
- Character state management
- Database operations
- Event tracking system
- Response generation

## Server/Interfaces/Services/ICombatService.cs
**Purpose:**
Establishes the contract for combat-related operations. This interface defines how combat interactions should be handled, ensuring consistent combat resolution across different implementations. It provides a standardized way to handle attacks, defense, and combat conditions.

**Data Origin:**
- Combat action requests
- Character combat states
- Attack specifications
- Defense calculations
- Combat condition triggers

**Where Data is Going:**
- Combat resolution implementations
- State update systems
- Combat logging
- Event notification system
- Client response formatting

## Server/Interfaces/Services/ICalculationService.cs
**Purpose:**
Defines the contract for game rule calculations. This interface ensures that all game mechanics calculations follow consistent patterns and rules. It acts as a mathematical foundation for the game system, handling everything from basic attribute calculations to complex derived statistics.

**Data Origin:**
- Character attributes
- Archetype modifications
- Equipment bonuses
- Condition effects
- Environmental modifiers

**Where Data is Going:**
- Calculation implementations
- Character stat updates
- Combat resolution systems
- Validation services
- Response generation

Each of these components plays a crucial role in maintaining the integrity and functionality of the character building system. They work together to ensure that all game rules are properly enforced while providing a smooth and intuitive experience for users.

Example Interaction:
```csharp
// Example of how these components interact during a combat action
public async Task<CombatResult> ResolveCombatAction(CombatActionRequest request)
{
    // DiceRollingService handles the attack roll
    var attackRoll = await _diceService.RollD20(
        request.Attacker.GetAccuracyBonus(),
        request.HasAdvantage
    );

    // CombatService determines if the attack hits
    var hitResult = await _combatService.ResolveHit(
        attackRoll,
        request.Defender.GetAvoidance()
    );

    if (hitResult.IsHit)
    {
        // On hit, calculate and apply damage
        var damage = await _calculationService.CalculateDamage(
            request.Attacker,
            request.Attack,
            hitResult.IsCritical
        );

        await _characterService.ApplyDamage(
            request.Defender.Id,
            damage
        );
    }

    return new CombatResult
    {
        AttackRoll = attackRoll,
        HitResult = hitResult,
        DamageDealt = damage,
        // Additional result data...
    };
}
```


**Server/Infrastructure/Data/**
**VitalityBuilderContext.cs**
**Purpose:**
- Serves as our central database management system, much like a librarian who knows exactly where every book is and how to find it. This class handles all interactions between our application and the database, ensuring data is stored, retrieved, and managed correctly. It's particularly important because it maintains the relationships between different parts of our character system, like how a character's combat attributes relate to their core profile.

**Data Origin:**
- Receives detailed entity configurations from Server/Infrastructure/Data/Configurations/ folder, which tell it how to map our C# classes to database tables
- Takes essential database connection information from the configuration we set up in Program.cs
- Accepts changes to character data when services like our CharacterCreationService or CharacterUpdateService make modifications
- Gets entity relationship definitions from our domain models, helping it understand how different pieces of data connect to each other

**Where Data is Going:**
- Writes data changes to our SQL Server database, ensuring all character information is properly saved
- Provides tracking information about what's changed in our entities, helping services understand what needs to be updated
- Returns query results when other parts of the application need to retrieve character data
- Sends detailed logging information about database operations to our logging system, helping us monitor performance and troubleshoot issues



**Server/Infrastructure/Data/Configurations/**
**CharacterConfiguration.cs**
**Purpose:**
- Acts as the blueprint that tells our database exactly how to store character information. Think of it like an architect's detailed plans for building a house - it specifies where every piece of data belongs and how it connects to other pieces. This configuration ensures that when we save a character, every attribute, ability, and relationship is stored in a way that maintains data integrity and allows for efficient retrieval.

**Data Origin:**
- Takes in the Character entity structure from our domain model, including all its properties and relationships
- Receives database mapping rules specified in the code, such as how to handle unique identifiers and foreign keys
- References conventions and standards from Entity Framework Core for best practices in data storage
- Incorporates any custom configurations needed for our specific game rules, like computed columns for point pools

**Where Data is Going:**
- Provides mapping instructions to the VitalityBuilderContext for database table creation and updates
- Guides Entity Framework Core in generating proper database migrations
- Influences how data is loaded and saved when working with character records
- Establishes relationship configurations that affect how related data (like attributes and archetypes) is handled

**Server/Infrastructure/Data/Configurations/**
**CombatAttributesConfiguration.cs**
**Purpose:**
- Defines the database structure for storing a character's combat-related statistics. Similar to a specialized filing system, this configuration ensures that all combat attributes are stored efficiently and maintains their crucial relationships to the main character record. It's particularly important because combat attributes are fundamental to gameplay and need to be quickly accessible.

**Data Origin:**
- References the CombatAttributes entity definition from our domain model
- Takes database-specific configuration requirements for performance optimization
- Incorporates validation rules that must be enforced at the database level
- Accepts relationship definitions showing how combat attributes connect to characters

**Where Data is Going:**
- Drives the creation and maintenance of the combat attributes database table
- Provides validation rules that the database will enforce automatically
- Establishes indexed lookups for efficient combat calculations
- Sets up navigation properties that allow easy access to combat data during character operations

**Server/Infrastructure/Data/Configurations/**
**UtilityAttributesConfiguration.cs**
**Purpose:**
- Manages how we store and organize non-combat character abilities in the database. This configuration is like setting up a specialized section in our character library that handles all the social, intellectual, and utility-based aspects of characters. It ensures these attributes are just as accessible and well-organized as combat statistics.

**Data Origin:**
- Uses the UtilityAttributes entity structure from our domain model
- Takes validation rules specific to utility attributes
- Incorporates relationship mappings to the main character record
- Receives configuration for any computed properties based on utility attributes

**Where Data is Going:**
- Guides database table creation for utility attributes
- Sets up efficient indexing for attribute queries
- Establishes data integrity rules at the database level
- Creates the necessary foreign key relationships with character records


**Server/Infrastructure/Security/**
**InputSanitizer.cs**
**Purpose:**
- Functions as our first line of defense against potentially harmful data entering the system. Think of it as a security guard that checks every piece of information before it's allowed into our application. When players input character names, descriptions, or other text, this component ensures that input is safe and won't cause problems in our database or create security vulnerabilities.

**Data Origin:**
- Receives raw text input from players through various API endpoints (character names, descriptions, etc.)
- Takes configuration settings that define what constitutes acceptable input
- Gets regular expression patterns that help identify potentially dangerous content
- Accepts lists of allowed and disallowed characters or patterns based on our game's requirements

**Where Data is Going:**
- Sends cleaned, safe text back to the controllers for further processing
- Provides sanitization reports to our logging system for security monitoring
- Returns validation errors if input contains forbidden content
- Feeds sanitized data to our database storage system

**Server/Infrastructure/Security/**
**AuthenticationHandler.cs**
**Purpose:**
- Manages how players prove they are who they say they are when accessing the system. It's like a bouncer at a club who checks IDs - this component verifies that requests to view or modify character data come from legitimate sources. While our system might start simple, this handler is designed to scale as we add more security features.

**Data Origin:**
- Takes authentication tokens or credentials from incoming HTTP requests
- Receives configuration settings defining our authentication rules
- Gets any stored player verification data from our database
- Accepts security policies defined in our application settings

**Where Data is Going:**
- Provides authentication results to the API controllers
- Sends security-related events to our logging system
- Returns appropriate HTTP responses for failed authentication attempts
- Passes validated identity information to other parts of the application

**Server/Infrastructure/Security/**
**SecurityPolicyProvider.cs**
**Purpose:**
- Defines and enforces the rules about who can do what in our system. Imagine it as the rulebook that security guards use to decide who gets access to different areas. This component helps ensure that players can only perform actions they're allowed to do, like modifying their own characters but not others'.

**Data Origin:**
- Takes security policy definitions from our application configuration
- Receives role assignments and permissions from our user management system
- Gets feature flags that might affect security rules
- Accepts resource-specific access rules (like character ownership)

**Where Data is Going:**
- Provides access decisions to our API controllers
- Sends policy evaluation results to the authorization system
- Returns detailed access denied reasons when permissions aren't granted
- Feeds security audit information to our logging system

**Server/Infrastructure/Security/**
**DataProtector.cs**
**Purpose:**
- Safeguards sensitive information as it moves through our system. Think of it as an envelope sealer - it ensures that private data stays private, whether it's being stored in our database or sent back to players. This is especially important for any future features involving private character information or player details.

**Data Origin:**
- Receives sensitive data that needs protection
- Takes encryption keys and configuration from secure storage
- Gets protection policies from application settings
- Accepts data classification rules that determine protection levels

**Where Data is Going:**
- Provides encrypted data to our storage systems
- Returns decrypted data when appropriate access is verified
- Sends encryption/decryption events to our audit logs
- Feeds protected data to our response pipeline

Let me explain the security-related infrastructure components, showing how they work together to protect our game system.

**Server/Infrastructure/Security/**
**InputSanitizer.cs**
**Purpose:**
- Functions as our first line of defense against potentially harmful data entering the system. Think of it as a security guard that checks every piece of information before it's allowed into our application. When players input character names, descriptions, or other text, this component ensures that input is safe and won't cause problems in our database or create security vulnerabilities.

**Data Origin:**
- Receives raw text input from players through various API endpoints (character names, descriptions, etc.)
- Takes configuration settings that define what constitutes acceptable input
- Gets regular expression patterns that help identify potentially dangerous content
- Accepts lists of allowed and disallowed characters or patterns based on our game's requirements

**Where Data is Going:**
- Sends cleaned, safe text back to the controllers for further processing
- Provides sanitization reports to our logging system for security monitoring
- Returns validation errors if input contains forbidden content
- Feeds sanitized data to our database storage system

**Server/Infrastructure/Security/**
**AuthenticationHandler.cs**
**Purpose:**
- Manages how players prove they are who they say they are when accessing the system. It's like a bouncer at a club who checks IDs - this component verifies that requests to view or modify character data come from legitimate sources. While our system might start simple, this handler is designed to scale as we add more security features.

**Data Origin:**
- Takes authentication tokens or credentials from incoming HTTP requests
- Receives configuration settings defining our authentication rules
- Gets any stored player verification data from our database
- Accepts security policies defined in our application settings

**Where Data is Going:**
- Provides authentication results to the API controllers
- Sends security-related events to our logging system
- Returns appropriate HTTP responses for failed authentication attempts
- Passes validated identity information to other parts of the application

**Server/Infrastructure/Security/**
**SecurityPolicyProvider.cs**
**Purpose:**
- Defines and enforces the rules about who can do what in our system. Imagine it as the rulebook that security guards use to decide who gets access to different areas. This component helps ensure that players can only perform actions they're allowed to do, like modifying their own characters but not others'.

**Data Origin:**
- Takes security policy definitions from our application configuration
- Receives role assignments and permissions from our user management system
- Gets feature flags that might affect security rules
- Accepts resource-specific access rules (like character ownership)

**Where Data is Going:**
- Provides access decisions to our API controllers
- Sends policy evaluation results to the authorization system
- Returns detailed access denied reasons when permissions aren't granted
- Feeds security audit information to our logging system

**Server/Infrastructure/Security/**
**DataProtector.cs**
**Purpose:**
- Safeguards sensitive information as it moves through our system. Think of it as an envelope sealer - it ensures that private data stays private, whether it's being stored in our database or sent back to players. This is especially important for any future features involving private character information or player details.

**Data Origin:**
- Receives sensitive data that needs protection
- Takes encryption keys and configuration from secure storage
- Gets protection policies from application settings
- Accepts data classification rules that determine protection levels

**Where Data is Going:**
- Provides encrypted data to our storage systems
- Returns decrypted data when appropriate access is verified
- Sends encryption/decryption events to our audit logs
- Feeds protected data to our response pipeline



**Server/Infrastructure/Logging/**
**LoggingConfiguration.cs**
**Purpose:**
- Functions as the master blueprint for our application's logging system. Think of it as setting up a sophisticated security camera system - it determines what we record, how we record it, and where we store these records. Every important action in our game system gets documented based on rules set here, from character creation to combat calculations.

**Data Origin:**
- Reads logging settings from appsettings.json that specify things like minimum log levels and output formats
- Takes environment-specific configuration that might change logging behavior between development and production
- Receives structured logging templates that define how different types of events should be recorded
- Gets category filters that help us focus on specific parts of the system when needed

**Where Data is Going:**
- Sets up logging providers (like console, file, or database loggers) that will receive our log messages
- Configures log message formatting and enrichment rules for better debugging
- Establishes log storage locations and retention policies
- Provides logging configuration to all other parts of the application

**Server/Infrastructure/Logging/**
**GameEventLogger.cs**
**Purpose:**
- Specializes in recording game-specific events that are important for understanding player actions and system behavior. Imagine this as a sports commentator who watches the game and records every significant play. This logger helps us understand how players are using the character builder and identifies any issues that need attention.

**Data Origin:**
- Receives game events from various services (character creation, updates, calculations)
- Takes context information about the current state of operations
- Gets performance metrics related to game operations
- Accepts error conditions and exceptional situations that need special attention

**Where Data is Going:**
- Writes formatted log entries to our configured logging destinations
- Sends important game events to monitoring systems
- Provides debugging information when issues occur
- Feeds analytics systems that help us understand system usage

**Server/Infrastructure/Logging/**
**AuditLogger.cs**
**Purpose:**
- Records all changes made to characters and important game data, creating a detailed history of every modification. This is like having a detailed ledger that tracks every change to a bank account - we can see exactly what changed, when it changed, and who made the change. This helps with both troubleshooting and maintaining data integrity.

**Data Origin:**
- Takes detailed before and after states of modified entities
- Receives user identification for who made changes
- Gets timestamps and contextual information about changes
- Accepts categorization of changes for better organization

**Where Data is Going:**
- Creates detailed audit records in a specialized audit log storage
- Provides change history for customer support and debugging
- Sends significant changes to notification systems if configured
- Feeds into reporting systems for analysis of system usage

**Server/Infrastructure/Logging/**
**PerformanceLogger.cs**
**Purpose:**
- Monitors and records how well our system is performing, especially during important operations like character calculations or database updates. Think of this as a fitness tracker for our application - it keeps track of how fast things are running and alerts us if anything starts slowing down.

**Data Origin:**
- Takes timing measurements from key operations throughout the system
- Receives resource usage statistics (memory, CPU, etc.)
- Gets database performance metrics
- Accepts custom performance markers from various parts of the application

**Where Data is Going:**
- Records performance metrics to specialized performance monitoring storage
- Sends alerts when performance drops below acceptable levels
- Provides data for performance trend analysis
- Feeds monitoring dashboards that show system health



**Server/Infrastructure/Validation/**
**ValidationExtensions.cs**
**Purpose:**
- Acts as the central registration system for all our validation rules. When our application starts up, this component ensures that every type of validation we need - from character creation rules to combat calculations - is properly set up and ready to use. It's similar to a referee reviewing all the rulebooks before a game starts, making sure every rule is clear and will be enforced consistently.

**Data Origin:**
- Takes in validation rule definitions from across our application
- Receives FluentValidation configurations that define how rules should be applied
- Gets dependency injection settings that tell our system where to find validators
- Accepts custom validation setup requirements from different parts of our application

**Where Data is Going:**
- Registers all validators with our dependency injection system
- Configures validation behavior settings throughout the application
- Provides validation services to API controllers and business logic
- Sets up automatic validation for incoming requests

**Server/Infrastructure/Validation/**
**CharacterValidator.cs**
**Purpose:**
- Serves as the primary guardian of character creation and modification rules. This validator ensures that every character in our game follows all the rules - from basic things like having a valid name to complex rules like making sure attribute points are spent correctly. It's like having an experienced game master checking every character sheet for accuracy.

**Data Origin:**
- Takes character data from creation and update requests
- Receives game rule constants that define valid ranges and limitations
- Gets tier-specific validation rules that change based on character level
- Accepts archetype-specific rules that modify what's allowed for different character types

**Where Data is Going:**
- Returns validation results to services handling character operations
- Provides detailed error messages when validation fails
- Sends validation failures to our logging system
- Supplies validation state to response generation

**Server/Infrastructure/Validation/**
**ArchetypesValidator.cs**
**Purpose:**
- Focuses specifically on making sure character archetype selections follow the rules. This is like having a specialization counselor who knows exactly which combinations of abilities and powers are allowed. It prevents players from selecting invalid combinations while helping them understand what options are available to them.

**Data Origin:**
- Receives archetype selection requests from character creation/updates
- Takes archetype combination rules from game configuration
- Gets prerequisite requirements for different archetypes
- Accepts tier-based restrictions on archetype availability

**Where Data is Going:**
- Provides validation results for archetype selections
- Returns clear explanations of why certain combinations aren't allowed
- Sends validation events to our logging system
- Feeds validation state into character update processing

**Server/Infrastructure/Validation/**
**PointAllocationValidator.cs**
**Purpose:**
- Ensures that all point spending in the game follows the rules. This validator watches over every point spent on attributes, abilities, and special features. It's like having an accountant who makes sure you never overspend your budget and that every point is spent according to the rules.

**Data Origin:**
- Takes point allocation requests from character updates
- Receives current point totals and spending history
- Gets tier-based point allocation limits
- Accepts special rules that might modify point costs or limits

**Where Data is Going:**
- Returns validation results for point spending attempts
- Provides detailed breakdowns of point allocation issues
- Sends spending validation events to logging
- Supplies point validation state to character updates


**Server/Domain/Character/CharacterEntity.cs**
**Purpose:**
- Serves as the foundational data model for all character information in our system. Think of this as the master blueprint that defines what makes up a character. It connects all the different pieces - from basic attributes to special abilities - into one cohesive entity that our database can understand and store.

**Data Origin:**
- Takes in character creation requests from the API
- Receives updates from character modification operations
- Gets calculated values from various game rule services
- Accepts relationships with other entities like attributes and archetypes

**Where Data is Going:**
- Provides the structure for database table creation
- Feeds data to API responses through DTOs
- Supplies information to calculation services
- Drives validation rules in the validation layer

**Server/Domain/Attributes/CombatAttributesEntity.cs**
**Purpose:**
- Defines the structure for a character's combat-related attributes. This entity acts like a specialized container that holds and manages all combat-focused statistics. It ensures that important combat values like Focus, Power, Mobility, and Endurance are properly stored and maintained.

**Data Origin:**
- Receives attribute assignments during character creation
- Takes updates from character modification requests
- Gets validation results from attribute validators
- Accepts recalculations from game rule services

**Where Data is Going:**
- Provides combat values for stat calculations
- Feeds into database storage through Entity Framework
- Supplies data for character response DTOs
- Drives combat-related validation rules

**Server/Domain/Attributes/UtilityAttributesEntity.cs**
**Purpose:**
- Manages the non-combat attributes that define a character's capabilities outside of battle. Similar to CombatAttributes, but focused on social, mental, and utility-based statistics. This entity ensures that aspects like Awareness, Communication, and Intelligence are properly tracked.

**Data Origin:**
- Takes initial values from character creation
- Receives updates during character modification
- Gets validation checks from attribute validators
- Accepts recalculations based on game rules

**Where Data is Going:**
- Provides data for skill check calculations
- Feeds into database storage operations
- Supplies information for character responses
- Informs utility-based validation rules

**Server/Domain/Constants/GameRuleConstants.cs**
**Purpose:**
- Acts as a central repository for all fixed values and rules in our game system. Think of this as our rulebook in code form - it contains all the numbers, limits, and basic rules that don't change during gameplay. This ensures consistency across all calculations and validations.

**Data Origin:**
- Defines base values from game rules documentation
- Contains tier-based progression values
- Holds attribute limits and thresholds
- Stores point cost constants

**Where Data is Going:**
- Provides constants to calculation services
- Feeds values to validation rules
- Supplies limits to character creation checks
- Informs point pool calculations



I'll continue explaining the remaining domain files with the same thorough approach, building on our earlier explanations.

**Server/Domain/Dtos/Character/CombatAttributesDto.cs**
**Purpose:**
- Functions as a specialized data transfer model for combat attributes. Think of this as a secure envelope designed specifically for sending combat-related data between our client and server. It ensures we only expose the necessary combat information while maintaining proper data validation and type safety. This DTO is particularly important because it helps protect the integrity of our core game mechanics.

**Data Origin:**
- Receives combat attribute values from client requests
- Takes mapped data from CombatAttributes entities
- Gets validation results from attribute validators
- Accepts calculated totals from game rule services

**Where Data is Going:**
- Provides structured data for API responses
- Feeds into character creation/update operations
- Supplies validation rules with data to check
- Informs the frontend about combat capabilities

**Server/Domain/Dtos/Character/UtilityAttributesDto.cs**
**Purpose:**
- Serves as the data transfer model for utility-based attributes. Similar to the combat attributes DTO, but focused on the non-combat aspects of a character. This DTO ensures that social, mental, and utility-based statistics are properly formatted for transmission between client and server. It plays a crucial role in managing the broader aspects of character capabilities.

**Data Origin:**
- Takes utility attribute values from client requests
- Receives mapped data from UtilityAttributes entities
- Gets utility-focused validation results
- Accepts calculated totals for utility checks

**Where Data is Going:**
- Provides formatted data for API responses
- Feeds into character attribute updates
- Supplies data for utility-based validation
- Informs frontend about non-combat capabilities

**Server/Domain/Dtos/Archetypes/CharacterArchetypesDto.cs**
**Purpose:**
- Acts as a comprehensive data transfer model for all character archetype selections. Think of this as a specialized form that captures all the defining choices that make each character unique. This DTO manages the complex web of archetype selections, ensuring that combinations are valid and properly communicated between client and server. It's particularly important because archetypes fundamentally shape how a character functions.

**Data Origin:**
- Takes archetype selections from character creation
- Receives updates during character modification
- Gets archetype-specific validation results
- Accepts combination rules from game systems

**Where Data is Going:**
- Provides structured archetype data for API responses
- Feeds into character archetype validation
- Supplies information for calculating derived abilities
- Informs point pool and ability calculations

**Server/Domain/Dtos/Character/CharacterResponseDto.cs**
**Purpose:**
- Functions as the comprehensive response model for character data. This DTO serves as the complete package of character information sent back to clients. Think of it as a detailed character sheet that includes not just basic attributes, but also calculated statistics, archetype information, and validation results. It ensures that clients receive a full and accurate picture of a character's current state.

**Data Origin:**
- Takes mapped data from Character entities
- Receives calculated statistics from services
- Gets validation results and warnings
- Accepts point pool calculations

**Where Data is Going:**
- Provides complete character data to clients
- Feeds frontend character displays
- Supplies information for character sheet rendering
- Informs UI about character state and capabilities

Here's an example of how these DTOs work together in a character update scenario:

```csharp
// When updating a character's combat attributes
public async Task<CharacterResponseDto> UpdateCombatAttributes(
    int characterId, 
    CombatAttributesDto updatedAttributes)
{
    // First, validate the incoming DTO
    var validationResult = await _validator.ValidateAsync(updatedAttributes);
    if (!validationResult.IsValid)
    {
        throw new ValidationException(validationResult.Errors);
    }

    // Update the character entity
    var character = await _repository.GetCharacterAsync(characterId);
    _mapper.Map(updatedAttributes, character.CombatAttributes);

    // Recalculate derived statistics
    var stats = _calculator.CalculateCharacterStats(character);

    // Create and return the response DTO
    var response = new CharacterResponseDto
    {
        Id = character.Id,
        Name = character.Name,
        CombatAttributes = updatedAttributes,
        // Map calculated stats and other properties
        Avoidance = stats.Avoidance,
        Durability = stats.Durability,
        // ... other properties
    };

    return response;
}
```

Would you like me to continue with more domain files or move on to another section of the application?



**server/Domain/Character/CharacterArchetypesEntity.cs**
**Purpose:**
Functions as the central hub for managing a character's archetype selections. This entity coordinates all the different archetype choices (movement, attack type, effect type, etc.) that define a character's unique abilities and playstyle. Think of it as a character's specialization blueprint that determines how their core abilities manifest.

**Data Origin:**
- Initial archetype selections during character creation
- Updates when players modify their archetype choices
- Default values based on game rules
- Relationships from linked archetype entities

**Where Data is Going:**
- Informs ability calculations and limitations
- Feeds into point cost determinations
- Provides validation rules for character updates
- Drives specialized behavior in combat calculations

**server/Domain/Enums/ArchetypeEnums.cs**
**Purpose:**
Defines the fixed set of options available for each archetype category. This file acts as a master list of all possible character specializations, ensuring consistency across the entire system and preventing invalid selections. It's like a catalog of all possible character paths.

**Data Origin:**
- Core game rules defining available archetypes
- Movement type definitions (Swift, Skirmisher, etc.)
- Attack type categories (AOE, Direct, Single Target)
- Effect type classifications (Damage, Hybrid, Control)

**Where Data is Going:**
- Provides valid options for character creation
- Feeds validation rules for archetype selection
- Supplies type information to database mappings
- Informs UI components about available choices

**server/Domain/Errors/ErrorResponse.cs**
**Purpose:**
Standardizes how errors are communicated throughout the application. This class ensures that when things go wrong, we provide clear, consistent, and helpful information to both users and developers. It's like creating a standard language for describing problems.

**Data Origin:**
- Validation failures from business rules
- Database operation errors
- Game rule violations
- System-level exceptions

**Where Data is Going:**
- API responses when errors occur
- Logging system for error tracking
- Client applications for user feedback
- Development tools for debugging

**server/Domain/Errors/ValidationResults.cs**
**Purpose:**
Collects and organizes all validation results from various checks performed on character data. This class acts as a comprehensive report card for character validity, gathering information about what's correct and what needs attention.

**Data Origin:**
- Attribute validation checks
- Point allocation verifications
- Archetype combination validations
- Game rule compliance checks

**Where Data is Going:**
- API responses for validation failures
- User interface for showing warnings/errors
- Logging system for tracking validation issues
- Character update processing decisions

**server/Domain/ValueObjects/CombatStats.cs**
**Purpose:**
Encapsulates derived combat statistics that are calculated from base attributes and archetypes. This class handles complex combat-related calculations while ensuring immutability of the results. Think of it as a combat calculator that processes raw attributes into usable battle statistics.

**Data Origin:**
- Base attributes from character entity
- Archetype modifiers
- Game rule constants
- Equipment bonuses (if applicable)

**Where Data is Going:**
- Combat resolution calculations
- Character sheet displays
- API responses with combat capabilities
- Validation rules for combat-related checks

**server/Infrastructure/Data/Configurations/CharacterConfiguration.cs**
**Purpose:**
Defines how Entity Framework should map character data to the database. This configuration file acts as an interpreter between our domain models and the database structure, ensuring data is stored and retrieved correctly.

**Data Origin:**
- Entity Framework configuration requirements
- Database schema definitions
- Relationship mappings between entities
- Index and constraint definitions

**Where Data is Going:**
- Database table creation scripts
- Query generation patterns
- Entity loading behavior
- Relationship navigation properties


**server/Infrastructure/Data/Configurations/CombatAttributesConfiguration.cs**
**Purpose:**
Controls how combat attributes are stored and related in the database. This configuration ensures that not only are the attributes themselves properly stored, but their relationships and constraints are enforced at the database level. Think of it as a specialized set of rules that ensures our combat data maintains its integrity even at the lowest storage level.

**Data Origin:**
- Database schema requirements for combat attributes
- Entity Framework configuration patterns
- Game rule constraints that need database enforcement
- Foreign key relationship definitions with characters

**Where Data is Going:**
- Shapes database table structure
- Guides query optimization for combat data
- Establishes attribute value constraints
- Defines relationship navigation paths

**server/Infrastructure/Data/Configurations/UtilityAttributesConfiguration.cs**
**Purpose:**
Manages the database mapping for utility-focused attributes. Similar to the combat configuration, but specifically tailored for non-combat attributes. This configuration ensures that social, mental, and utility-based statistics are properly stored and related to their parent character records.

**Data Origin:**
- Database schema requirements for utility attributes
- Relationship mappings to character entities
- Constraint definitions for utility values
- Index configurations for efficient querying

**Where Data is Going:**
- Database table structure definitions
- Query performance optimizations
- Data integrity rule enforcement
- Navigation property configurations

**server/Infrastructure/Data/VitalityBuilderContext.cs**
**Purpose:**
Serves as the primary interface between our application and the database. This context class acts as a coordinator for all database operations, managing everything from simple queries to complex transactions. Think of it as a librarian who knows exactly how to find, store, and organize all our character data.

**Data Origin:**
- Entity configurations from all model builders
- Connection string from application settings
- Change tracking information from entity updates
- Query requests from services and controllers

**Where Data is Going:**
- Executes database operations
- Manages entity relationships
- Handles database transactions
- Provides query results to services

**server/Infrastructure/Logging/LoggingConfiguration.cs**
**Purpose:**
Establishes how our application tracks and records its operation. This configuration ensures that we capture important information about what's happening in our system, from routine operations to critical errors. It's like setting up a comprehensive security camera system that watches over our application.

**Data Origin:**
- Logging level settings from configuration
- Output destination preferences
- Format specifications for log entries
- Filter rules for different types of logs

**Where Data is Going:**
- Configures logging providers
- Sets up log message formatting
- Establishes log storage locations
- Defines logging behavior rules

**server/Infrastructure/Validation/ArchetypesValidator.cs**
**Purpose:**
Enforces rules about valid archetype combinations and selections. This validator ensures that when players choose their character's specializations, all selections follow the game rules and work together properly. It's like having an expert game master checking character choices for validity.

**Data Origin:**
- Archetype selection requests
- Game rules about valid combinations
- Tier-based restrictions
- Prerequisite requirements

**Where Data is Going:**
- Validation results to services
- Error messages for invalid selections
- Logging system for validation failures
- Response generation for client feedback

Let me continue documenting the remaining files, maintaining our comprehensive approach.

**server/Infrastructure/Validation/CharacterValidator.cs**
**Purpose:**
Acts as the guardian of character data integrity, ensuring that every aspect of a character follows the game rules. This validator coordinates all the individual validation rules into a cohesive system, much like a master referee who knows every rule in the game and how they interact. It's particularly important because it prevents invalid character states that could disrupt game balance.

**Data Origin:**
- Character creation and update requests
- Current character state for comparisons
- Game rule constants for validation limits
- Tier-based restrictions and requirements
- Point allocation tracking data

**Where Data is Going:**
- Validation results to character services
- Detailed error messages for UI display
- Logging system for validation failures
- Input for character state updates
- Warning flags for borderline cases

**server/Infrastructure/Validation/ValidationExtensions.cs**
**Purpose:**
Provides a centralized way to register and configure all validation rules in our application. Think of this as the validation system's setup manual - it ensures that every validator is properly configured and available when needed. This file is crucial because it establishes the connection between our validation rules and the dependency injection system.

**Data Origin:**
- Validator class registrations
- Configuration settings for FluentValidation
- Custom validation rule definitions
- Service lifetime configurations
- Global validation behavior settings

**Where Data is Going:**
- Dependency injection container
- Application startup configuration
- Validation pipeline setup
- Global validation behavior rules
- Service provider configuration

**server/Services/Calculations/CalculationService.cs**
**Purpose:**
Manages all complex game calculations related to character statistics. This service acts like a sophisticated calculator that understands all the game's mathematical rules and can apply them correctly in any situation. It ensures that derived statistics are consistently calculated across the entire application.

**Data Origin:**
- Base character attributes
- Archetype modifiers
- Equipment bonuses
- Condition effects
- Game rule constants
- Tier-based scaling factors
- Environmental modifiers

**Where Data is Going:**
- Derived statistics for characters
- Combat resolution calculations
- Movement determination
- Ability effect calculations
- Character sheet updates
- API response data

**server/Services/Calculations/PointPoolCalculator.cs**
**Purpose:**
Specializes in managing and calculating all point-based resources in the character system. This service is like an accountant for character creation points, tracking how many points are available, spent, and remaining across different pools. It's essential for maintaining the balance of character creation and progression.

**Data Origin:**
- Character tier information
- Archetype modifiers
- Spent point tracking
- Purchase history
- Special ability costs
- Limit-based refunds
- Feature prerequisites

**Where Data is Going:**
- Available point calculations
- Purchase validation checks
- Character update processing
- Creation wizard validation
- API response data
- Warning generation for near-limits
- Audit trail for point spending

Let me continue documenting the remaining service files, maintaining our detailed approach while explaining how each component fits into the larger system.

**server/Services/Character/CharacterCreationService.cs**
**Purpose:**
Orchestrates the entire character creation process from start to finish. Think of this service as a master craftsman who takes all the raw materials (attributes, archetypes, abilities) and assembles them into a fully formed character. It ensures that every new character is created following all game rules while maintaining data consistency. This service is particularly crucial because it sets the foundation for a character's entire lifecycle in the game.

**Data Origin:**
- Character creation requests from API
- Initial attribute selections
- Archetype choices
- Starting point allocations
- Default values from game rules
- Tier-based starting resources
- Validation rules and constraints
- Template data (if using character templates)

**Where Data is Going:**
- New character records in database
- Initial stat calculations
- Point pool initializations
- Validation service for rule checking
- Event logging system for auditing
- Response DTOs for client feedback
- Notification system for character creation events
- Initialization of related character components (attributes, archetypes, etc.)

**server/Services/Character/CharacterUpdateService.cs**
**Purpose:**
Manages all modifications to existing characters while maintaining game rule integrity. This service acts like a careful editor who ensures that every change to a character makes sense and follows the rules. It coordinates complex updates that might affect multiple aspects of a character, ensuring that all changes are consistent and properly validated.

**Data Origin:**
- Character update requests
- Current character state
- Point allocation changes
- Attribute modifications
- Archetype adjustments
- New ability selections
- Upgrade choices
- Validation rules for changes
- Transaction history

**Where Data is Going:**
- Updated database records
- Recalculated statistics
- Point pool adjustments
- Validation checks
- Audit logging system
- Change history tracking
- Response data for client
- Event notifications
- Warning flags for significant changes

**server/Services/Validation/ValidationService.cs**
**Purpose:**
Provides a centralized validation hub that coordinates all rule checking across the system. This service functions like a quality control department, ensuring that every character change, whether during creation or updates, maintains game balance and rule integrity. It's especially important because it prevents invalid game states that could disrupt gameplay or create unfair advantages.

**Data Origin:**
- Character state snapshots
- Proposed changes
- Game rule constants
- Tier requirements
- Point allocation rules
- Archetype restrictions
- Ability prerequisites
- Combination limitations
- Historical validation data
- Custom validation rules

**Where Data is Going:**
- Validation results to services
- Detailed error messages
- Warning notifications
- Audit trail entries
- Rule violation logs
- Client feedback messages
- Character state updates
- Cross-validation results
- Performance metrics
- Debug information

Each of these services works together in a carefully choreographed system. For example, when a player tries to upgrade their character's tier, the process might flow like this:

```csharp
// Example interaction between services
public async Task<CharacterUpdateResult> ProcessTierUpgrade(int characterId, int newTier)
{
    // CharacterUpdateService coordinates the overall process
    var character = await _repository.GetCharacterAsync(characterId);
    var currentState = new CharacterState(character);

    // ValidationService checks if the upgrade is allowed
    var validationResult = await _validationService.ValidateTierUpgrade(
        character, 
        newTier,
        currentState
    );

    if (!validationResult.IsValid)
    {
        return new CharacterUpdateResult
        {
            Success = false,
            Errors = validationResult.Errors,
            Warnings = validationResult.Warnings
        };
    }

    // PointPoolCalculator adjusts available resources
    var newPools = await _pointPoolCalculator.RecalculatePoolsForTier(
        character, 
        newTier
    );

    // CalculationService updates derived statistics
    var newStats = await _calculationService.RecalculateAllStats(
        character,
        newTier
    );

    // All changes are applied and saved
    character.ApplyTierUpgrade(newTier, newPools, newStats);
    await _repository.SaveChangesAsync();

    // Log the successful upgrade
    await _logger.LogCharacterUpgrade(character, currentState);

    return new CharacterUpdateResult
    {
        Success = true,
        UpdatedCharacter = character,
        Changes = CompareStates(currentState, character)
    };
}
```


**server/Interfaces/Repositories/ICharacterRepository.cs**
**Purpose:**
Defines the contract for all character data access operations in our system. This interface acts as a blueprint that ensures consistent data access patterns throughout the application, regardless of the underlying storage mechanism. Just as a library has standard procedures for checking out and returning books, this interface establishes standard ways to interact with character data. It's particularly important because it allows us to modify our data access implementation without affecting the rest of the application.

**Data Origin:**
- Repository implementation classes
- Database context operations
- Query specifications
- Character entity definitions
- Transaction boundaries
- Filter criteria from services
- Sort preferences from clients
- Pagination parameters

**Where Data is Going:**
- Character service implementations
- Data access patterns
- Query execution plans
- Transaction management
- Cache interactions
- Performance monitoring
- Error handling systems
- Audit logging mechanisms

**server/Interfaces/Services/ICharacterStatCalculator.cs**
**Purpose:**
Establishes the contract for calculating character statistics throughout our system. Think of this interface as a standardized set of mathematical formulas that ensures every character's capabilities are calculated consistently. Whether we're determining combat effectiveness or social abilities, this interface guarantees that the same rules apply everywhere. It's crucial because even small inconsistencies in stat calculations could create significant gameplay imbalances.

**Data Origin:**
- Character base attributes
- Archetype modifiers
- Special ability effects
- Equipment bonuses
- Condition modifiers
- Environmental factors
- Tier-based scaling rules
- Temporary buffs and debuffs

**Where Data is Going:**
- Combat resolution systems
- Character sheet displays
- Ability validation checks
- Balance monitoring tools
- Performance analysis
- Debug information
- API responses
- Game state updates

**server/Interfaces/Services/IPointPoolCalculator.cs**
**Purpose:**
Defines how point allocation and tracking should work across the system. This interface serves as the architectural blueprint for managing the game's economy of character points. Just as a bank needs standard procedures for handling transactions, this interface ensures that point spending, refunding, and calculation follow consistent rules. It's essential for maintaining game balance and preventing point exploitation.

**Data Origin:**
- Character tier information
- Archetype bonuses
- Purchase history
- Refund calculations
- Limit-based adjustments
- Prerequisite costs
- Special discount rules
- Temporary point modifiers

**Where Data is Going:**
- Character creation validation
- Purchase availability checks
- Upgrade cost calculations
- Point pool tracking
- Audit trail entries
- Warning generation
- API responses
- Balance monitoring

Here's an example of how these interfaces work together in practice:

```csharp
// Example showing the interaction between interfaces
public class CharacterUpgradeService
{
    private readonly ICharacterRepository _repository;
    private readonly ICharacterStatCalculator _statCalculator;
    private readonly IPointPoolCalculator _pointCalculator;

    public async Task<UpgradeResult> ProcessCharacterUpgrade(
        int characterId, 
        UpgradeRequest request
    )
    {
        // Use repository to access character data
        var character = await _repository.GetCharacterWithFullDetails(characterId);
        if (character == null)
        {
            throw new CharacterNotFoundException(characterId);
        }

        // Calculate point costs and check availability
        var pointRequirements = _pointCalculator.CalculateUpgradeCosts(
            character,
            request.Upgrades
        );

        if (!pointRequirements.HasSufficientPoints)
        {
            return UpgradeResult.InsufficientPoints(pointRequirements.Shortfall);
        }

        // Apply upgrades and recalculate stats
        character.ApplyUpgrades(request.Upgrades);
        var newStats = _statCalculator.RecalculateAllStats(character);

        // Save changes through repository
        await _repository.UpdateCharacter(character);

        return new UpgradeResult
        {
            Success = true,
            UpdatedStats = newStats,
            RemainingPoints = pointRequirements.RemainingPoints,
            NewCapabilities = DetermineNewCapabilities(
                character,
                request.Upgrades
            )
        };
    }
}
```



**server/Properties/launchSettings.json**
**Purpose:**
Controls how our application starts up in different environments. Think of this file as a startup manual that tells our application exactly how to begin running in various scenarios - whether we're developing locally, testing, or running in production. This configuration is particularly important because it ensures consistent behavior across different development and deployment environments.

**Data Origin:**
- Development environment settings
- HTTP port configurations
- Environment variable definitions
- Launch profile preferences
- SSL certificate settings
- Debugging configurations
- Command line arguments
- Default URL patterns

**Where Data is Going:**
- Application startup process
- Development server configuration
- HTTPS redirect settings
- Environment variable injection
- Debug configuration
- Swagger documentation setup
- Logging initialization
- Health check endpoints

**server/Services/Interface/IValidationService.cs**
**Purpose:**
Establishes the contract for how validation should be performed throughout our application. This interface acts as a standardized rulebook that ensures all character validations - from simple attribute checks to complex archetype combinations - follow consistent patterns. Just as a game referee needs clear guidelines, this interface provides explicit rules for what makes a character valid or invalid.

**Data Origin:**
- Validation rule definitions
- Character state information
- Game rule constraints
- Point allocation limits
- Archetype restrictions
- Ability prerequisites
- Combination rules
- Tier-based requirements

**Where Data is Going:**
- Character creation process
- Update validation checks
- Point spending verification
- Ability purchase validation
- Cross-component validation
- Error message generation
- Warning notifications
- Audit logging system

Here's an example showing how this interface might be implemented:

```csharp
public class ValidationService : IValidationService
{
    private readonly IPointPoolCalculator _pointCalculator;
    private readonly ICharacterStatCalculator _statCalculator;
    
    public async Task<ValidationResult> ValidateCharacterUpdate(
        Character character,
        UpdateRequest request
    )
    {
        // Create a validation context to track all checks
        var context = new ValidationContext(character)
        {
            CurrentTier = character.Tier,
            ProposedChanges = request.Changes,
            ExistingPointAllocation = await _pointCalculator
                .GetCurrentAllocation(character)
        };

        // Run through all validation rules
        var results = new List<ValidationResult>();
        
        // Validate attributes against tier limits
        results.Add(ValidateAttributeLimits(context));
        
        // Check point pool constraints
        results.Add(ValidatePointAllocation(context));
        
        // Verify archetype combinations
        results.Add(ValidateArchetypeCombinations(context));
        
        // Ensure all prerequisites are met
        results.Add(ValidatePrerequisites(context));

        // Combine all validation results
        return new ValidationResult
        {
            IsValid = results.All(r => r.IsValid),
            Errors = results.SelectMany(r => r.Errors).ToList(),
            Warnings = results.SelectMany(r => r.Warnings).ToList(),
            // Include validation context for detailed feedback
            Context = context
        };
    }

    private ValidationResult ValidateAttributeLimits(ValidationContext context)
    {
        var result = new ValidationResult();
        
        // Check each attribute against tier maximum
        foreach (var attribute in context.ProposedChanges.Attributes)
        {
            if (attribute.Value > context.CurrentTier)
            {
                result.AddError(
                    $"{attribute.Name} cannot exceed character tier"
                );
            }
            else if (attribute.Value == context.CurrentTier)
            {
                result.AddWarning(
                    $"{attribute.Name} is at maximum for current tier"
                );
            }
        }
        
        return result;
    }
}
```


