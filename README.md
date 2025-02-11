# Vitality System Character Builder

A comprehensive web application for creating and managing characters using the Vitality System roleplaying game rules. This application streamlines the character creation process by automating calculations, validating rule requirements, and providing an intuitive interface for character management.

## Features

- Complete character creation and management
- Automated calculation of attributes and derived statistics
- Special attack builder with limits and upgrades
- Management of unique abilities, expertise, features, senses, and descriptors
- Built-in dice rolling system
- Character data persistence and sharing
- Export functionality

## Project Structure

```
vitality-builder/
├── client/                # React frontend application
│   ├── src/              # Source files
│   ├── public/           # Static assets
│   └── package.json      # Frontend dependencies
├── server/
    ├── Controllers/
    │   └── CharacterController.cs           // Handles HTTP endpoints

    ├── Domain/
    │   ├── Entities/
    │   │   ├── CharacterEntity.cs           // Core character data
    │   │   ├── CombatAttributesEntity.cs    // Combat-related attributes
    │   │   ├── ExpertiseEntity.cs           // Character expertise
    │   │   ├── SpecialAttackEntity.cs       // Special attack definitions
    │   │   ├── UniquePowerEntity.cs         // Unique power definitions
    │   │   └── UtilityAttributesEntity.cs   // Utility-related attributes
    │   │
    │   ├── DTOs/
    │   │   ├── CharacterDto.cs              // Character data transfer
    │   │   ├── CreateCharacterDto.cs        // Character creation
    │   │   ├── CombatAttributesDto.cs       // Combat attributes transfer
    │   │   └── UtilityAttributesDto.cs      // Utility attributes transfer
    │   │
    │   ├── Enums/
    │   │   └── ArchetypeEnums.cs            // All archetype-related enums
    │   │
    │   └── Validations/
    │       ├── CharacterValidator.cs         // Character validation rules
    │       └── AttributeValidator.cs         // Attribute validation rules

    ├── Infrastructure/
    │   ├── Database/
    │   │   ├── VitalityBuilderContext.cs    // EF Core DbContext
    │   │   └── EntityConfigurations/        // Separate config files
    │   │
    │   ├── Logging/
    │   │   └── LoggingConfiguration.cs      // Logging setup
    │   │
    │   └── Validation/
    │       └── ValidationExtensions.cs       // Validation registration

    ├── Interfaces/
    │   ├── Services/
    │   │   ├── ICharacterService.cs         // Character management
    │   │   ├── ICombatService.cs            // Combat operations
    │   │   └── ICalculationService.cs       // Game rule calculations
    │   │
    │   └── Repositories/
    │       └── ICharacterRepository.cs       // Data access

    ├── Services/
    │   ├── Character/
    │   │   ├── Creation/
    │   │   │   └── CharacterCreationService.cs
    │   │   └── Management/
    │   │       └── CharacterManagementService.cs
    │   │
    │   ├── Combat/
    │   │   ├── CombatService.cs             // Combat resolution
    │   │   └── DiceRollingService.cs        // Dice rolling logic
    │   │
    │   └── Rules/
    │       ├── Points/
    │       │   └── PointPoolCalculator.cs    // Point calculations
    │       ├── Stats/
    │       │   └── CharacterStatCalculator.cs // Stat calculations
    │       └── Abilities/
    │           └── SpecialAttackCalculator.cs // Special ability math

    └── Utilities/
        ├── Extensions/
        │   └── JsonValueConverter.cs         // JSON conversion helpers
        └── Helpers/
            └── ValidationHelper.cs           // Validation utilities
```

## Getting Started

### Prerequisites
- Node.js (v18 or higher)
- .NET 9.0 SDK
- SQL Server Developer

### Development Setup



## Contributing

We welcome contributions! Please read our contributing guidelines for details on submitting pull requests.

## License

This project is licensed under the MIT License - see the LICENSE file for details.


