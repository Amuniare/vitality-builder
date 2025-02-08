# Contributing to Vitality Builder

Thank you for your interest in contributing to Vitality Builder! This document provides guidelines for contributing to any part of the project.

## Repository Structure

Our monorepo contains multiple packages, each with its own purpose:

- `vitality-builder-client`: React frontend application
- `vitality-builder-server`: Backend server (future)
- `shared`: Shared utilities and types (future)

## Development Workflow

1. Fork the repository
2. Clone your fork:
   ```bash
   git clone https://github.com/yourusername/vitality-builder.git
   ```
3. Create a new branch for your feature:
   ```bash
   git checkout -b feature/amazing-feature
   ```
4. Make your changes
5. Run tests in the affected packages
6. Submit a pull request

## Package-Specific Guidelines

### Client Application
- Follow React best practices
- Write meaningful component documentation
- Include unit tests for new features
- Use TypeScript for type safety

### Server (Future)
- Follow RESTful API design principles
- Document all endpoints
- Include integration tests
- Use TypeScript for type safety

## Code Style
- Use consistent formatting
- Write meaningful commit messages
- Include comments for complex logic
- Follow the established patterns in each package
