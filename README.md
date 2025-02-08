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
├── server/               # .NET Web API
│   ├── src/             # Source files
│   └── tests/           # Backend tests
├── shared/              # Shared types and utilities
│   ├── types/           # TypeScript type definitions
│   └── utils/           # Shared helper functions
├── docs/                # Documentation
│   └── rules/           # Game rule references
└── tools/               # Development tools and scripts
```

## Getting Started

### Prerequisites
- Node.js (v18 or higher)
- .NET 7.0 SDK
- SQL Server LocalDB or SQLite

### Development Setup

1. Clone the repository:
```bash
git clone https://github.com/yourusername/vitality-builder.git
cd vitality-builder
```

2. Install frontend dependencies:
```bash
cd client
npm install
```

3. Start the development server:
```bash
npm run dev
```

4. In a new terminal, start the backend:
```bash
cd ../server
dotnet run
```

## Contributing

We welcome contributions! Please read our contributing guidelines for details on submitting pull requests.

## License

This project is licensed under the MIT License - see the LICENSE file for details.
