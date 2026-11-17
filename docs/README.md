# Avalon Documentation

Welcome to the Avalon MMO prototype documentation! This hub provides access to all project documentation.

## Quick Links

- **[Architecture Documentation](architecture/README.md)** - Technical system design and implementation
- **[Features Documentation](features/README.md)** - User-facing features and guides
- **[Contributing Guide](contributing/README.md)** - How to contribute to the project
- **[Changelog](../CHANGELOG.md)** - Project history and recent changes

## What is Avalon?

Avalon is a Unity-based MMO prototype created to:
- Validate game design ideas and MMO mechanics
- Practice Unity development and game server implementation
- Explore client-server architecture patterns

**Status:** Prototype / Work in Progress

## Documentation Structure

```
docs/
├── README.md                    # This file - documentation hub
├── architecture/                # Technical architecture docs
│   ├── README.md               # Architecture overview
│   ├── combat-system.md        # Combat architecture
│   ├── multiplayer-architecture.md  # Networking and packets
│   ├── entity-management.md    # Entity registry and lifecycle
│   ├── input-system.md         # Input handling
│   └── animation-system.md     # Animation coordination
├── features/                    # User-facing feature docs
│   ├── README.md               # Feature index
│   ├── character-animation.md  # Character animation feature
│   ├── character-movement.md   # Movement feature
│   ├── sprite-pipeline.md      # Sprite sheet automation
│   ├── combat-flow.md          # Combat feature
│   └── enemy-behavior.md       # Enemy AI feature
├── contributing/                # Contribution guidelines
│   ├── README.md               # How to contribute
│   ├── documentation-guide.md  # Documentation conventions
│   └── code-style.md           # Coding conventions
└── changelog/                   # Changelog location
    └── README.md               # Points to root CHANGELOG.md
```

## For New Contributors

Start here to understand the project:

1. **[How to Contribute](contributing/README.md)** - Setup and workflow
2. **[Architecture Overview](architecture/README.md)** - Technical design
3. **[Features Index](features/README.md)** - What's implemented
4. **[Code Style Guide](contributing/code-style.md)** - Coding conventions

## For Developers

### Understanding the Systems

**Core Architecture:**
- [Multiplayer Architecture](architecture/multiplayer-architecture.md) - Client-server packet flow
- [Entity Management](architecture/entity-management.md) - Entity registry and lifecycle
- [Combat System](architecture/combat-system.md) - Event-driven combat
- [Input System](architecture/input-system.md) - Input handling and synchronization
- [Animation System](architecture/animation-system.md) - Animation coordination

**Implemented Features:**
- [Combat Flow](features/combat-flow.md) - Player and enemy combat
- [Character Movement](features/character-movement.md) - Movement and input
- [Character Animation](features/character-animation.md) - Sprite animation
- [Enemy Behavior](features/enemy-behavior.md) - Enemy AI targeting
- [Sprite Pipeline](features/sprite-pipeline.md) - Automated animation creation

### Making Changes

1. **Read relevant architecture docs** to understand the systems
2. **Follow the code style guide** for consistency
3. **Update documentation** when changing behavior
4. **Add changelog entry** for notable changes
5. **Submit PR** with clear description and test steps

See [Contributing Guide](contributing/README.md) for detailed workflow.

## For Players/Testers

### Current Features

- **Character Movement:** WASD movement with directional animations
- **Combat System:** Basic attack/defend mechanics
- **Enemy AI:** Enemies target and attack players
- **Multiplayer (Local):** Observer mode for testing

### How to Test

1. Clone repository
2. Open in Unity
3. Open sample scene
4. Enter play mode
5. Use keyboard/mouse for controls

For multiplayer testing, see [Multiplayer Architecture](architecture/multiplayer-architecture.md).

## Documentation Conventions

### File Naming
- Use **kebab-case** for all markdown files
- Example: `multiplayer-architecture.md`, `combat-flow.md`

### Linking
- Use **relative paths** for internal links
- Example: `[Combat System](architecture/combat-system.md)`

### Writing Style
- **Concise and clear** - Simple language
- **Examples included** - Code snippets where helpful
- **ASCII only** - No smart quotes or special characters

See [Documentation Guide](contributing/documentation-guide.md) for full guidelines.

## Project Status

### Implemented Systems
✅ Basic entity management  
✅ Combat system (local)  
✅ Player input handling  
✅ Character animation  
✅ Enemy AI targeting  
✅ Multiplayer packet flow (local loopback)  

### In Progress
🚧 Sprite sheet automation  
🚧 Server-authoritative combat  
🚧 Advanced enemy AI  

### Planned
📋 Network transport (beyond local loopback)  
📋 Skills and abilities  
📋 Inventory system  
📋 Quest system  

See [Changelog](../CHANGELOG.md) for detailed progress.

## Technology Stack

- **Engine:** Unity 2021+
- **Language:** C# (.NET Framework 4.7.1)
- **Architecture:** Client-Server with local loopback
- **Assets:** Custom sprite pipeline

## WebGL Build

A WebGL build is available for testing:
- Build files: `docs/Build/`, `docs/TemplateData/`
- Play in browser: `docs/index.html`

## Getting Help

### Questions About...

**Code/Implementation:**
- Check [Architecture Documentation](architecture/README.md)
- Review [Code Style Guide](contributing/code-style.md)
- Search GitHub Issues

**Features:**
- Check [Features Documentation](features/README.md)
- Review changelog for recent changes

**Contributing:**
- Read [Contributing Guide](contributing/README.md)
- Check [Documentation Guide](contributing/documentation-guide.md)

### Still Need Help?

1. Search existing documentation
2. Check GitHub Discussions
3. Open a GitHub Issue
4. Ask in project discussions

## AI Usage Disclaimer

Some portions of this documentation were generated or assisted by AI tools to enhance clarity and organization. **All code is written by humans, not AI.**

## Credits

### Assets
Game assets purchased from [PVGames (Pioneer Valley Games)](http://www.pioneervalleygames.com/)

### Contributors
See GitHub contributors page for full list.

## License

This is a prototype project for educational and validation purposes.

## Recent Updates

See [Changelog](../CHANGELOG.md) for recent changes.

**Last Updated:** Documentation restructure - January 2024

## Navigation

- 📚 [Architecture Documentation](architecture/README.md)
- 🎮 [Features Documentation](features/README.md)
- 🤝 [Contributing Guide](contributing/README.md)
- 📝 [Changelog](../CHANGELOG.md)
- 🏠 [Project README](../README.md)
