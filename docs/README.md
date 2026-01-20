# Avalon Documentation

Welcome to the Avalon documentation hub. This is your entry point for understanding the repository structure, architecture, features, and how to contribute.

## Getting Started

New to the project? Start here:

1. Read the [Project Overview](../README.md) to understand what Avalon is
2. Review the [Architecture Overview](architecture/README.md) to understand the system design
3. Check out [Features](features/README.md) to see what's implemented
4. Want to contribute? See the [Contributing Guide](contributing/README.md)

## Documentation Structure

All documentation lives under the `docs/` folder with a clear organization:

### Architecture (`docs/architecture/`)

Technical documentation explaining **how** the systems work:

- [Architecture Overview](architecture/README.md) - High-level Unity MMO prototype architecture
- [Combat System](architecture/combat-system.md) - Event-driven combat, tick system, damage resolution
- [Entity Management](architecture/entity-management.md) - PlayerEntitiesManager, character hierarchy
- [Input System](architecture/input-system.md) - PlayerInputMapper, input handling, direction mapping
- [Animation System](architecture/animation-system.md) - CharacterAnimationHandler, animation integration
- [Multiplayer Architecture](architecture/multiplayer-architecture.md) - Client-server packet handling, observer support
- [TextMesh Pro Resources](architecture/textmeshpro-resources.md) - TMP asset management and configuration

### Features (`docs/features/`)

User-facing capabilities explaining **what** the game can do:

- [Features Index](features/README.md) - Complete list of implemented and planned features
- [Character Animation](features/character-animation.md) - Character sprite animation system
- [Character Movement](features/character-movement.md) - Player movement and input integration
- [Sprite Pipeline](features/sprite-pipeline.md) - SpriteSheet to Animations automation
- [Combat Flow](features/combat-flow.md) - Basic entity manager and combat interactions
- [Enemy Behavior](features/enemy-behavior.md) - Enemy AI and behavior management
- [Build Pipeline](features/build-pipeline.md) - CI/CD automation (planned)

### Contributing (`docs/contributing/`)

Guides for contributors:

- [Contributing Overview](contributing/README.md) - How to contribute to the project
- [Documentation Guide](contributing/documentation-guide.md) - How to write and update documentation
- [Code Style Guide](contributing/code-style.md) - Coding conventions and best practices

### Changelog (`docs/changelog/`)

- [Changelog](../CHANGELOG.md) - Project changelog (kept at root for visibility)

## Quick Navigation

- **Understanding the code?** Start with [Architecture Overview](architecture/README.md)
- **Looking for a specific feature?** Check the [Features Index](features/README.md)
- **Want to add a feature?** Read the [Contributing Guide](contributing/README.md)
- **Updating documentation?** Follow the [Documentation Guide](contributing/documentation-guide.md)

## Documentation Standards

### File Naming

- Use `kebab-case.md` for all documentation files
- Feature docs: `docs/features/<feature-name>.md`
- Architecture docs: `docs/architecture/<system-name>.md`

### Adding New Documentation

1. Determine if it's a **feature** (user-facing capability) or **architecture** (technical system)
2. Create file in the appropriate `docs/` subfolder using kebab-case
3. Follow the template structure (see [Documentation Guide](contributing/documentation-guide.md))
4. Add entry to the relevant `README.md` index
5. Update `CHANGELOG.md` under `Unreleased`

## Questions or Issues?

If you have questions about the documentation or find something unclear:

1. Check if there's an existing issue in the GitHub issue tracker
2. Create a new issue using the appropriate template
3. Tag it with the `documentation` label
