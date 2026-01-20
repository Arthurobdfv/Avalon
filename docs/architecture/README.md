# Architecture Overview

## What is Avalon?

Avalon is a Unity-based MMO prototype designed to validate game design ideas and practice Unity development. This document provides a high-level overview of the technical architecture and key systems.

## Project Structure

### Unity Project Configuration

- **Engine:** Unity (see `ProjectSettings/` for version details)
- **Target Framework:** .NET Framework 4.7.1
- **Assemblies:**
  - `Assembly-CSharp` - Runtime game code
  - `Assembly-CSharp-Editor` - Editor-only tools and importers

### Directory Organization

```
Assets/
├── Animations/         # Animation clips and animator controllers
├── Prefabs/           # Character, enemy, and UI prefabs
├── Scenes/            # Unity scene files
├── Scripts/           # C# source code
│   ├── Character/     # Character systems (player, enemy, combat)
│   ├── Input/         # Input handling
│   ├── UI/            # User interface components
│   └── Editor/        # Editor-only scripts and tools
└── Sprites/           # Texture and sprite assets
```

## Core Systems

The Avalon prototype is organized into several interconnected systems:

### 1. Combat System

Event-driven combat with tick-based timing and damage resolution.

- **Purpose:** Handle character combat interactions, health, and damage
- **Key Components:** CombatManager, CombatCharacter, CombatBaseStats
- **Documentation:** [Combat System Architecture](combat-system.md)

### 2. Entity Management System

Centralized entity registration and lookup for characters.

- **Purpose:** Manage player and enemy entities, provide entity queries
- **Key Components:** PlayerEntitiesManager, EntityExtensions
- **Documentation:** [Entity Management Architecture](entity-management.md)

### 3. Input System

Maps player input to character actions and movement.

- **Purpose:** Handle keyboard/controller input, direction mapping, action triggers
- **Key Components:** PlayerInputHandler, PlayerInputMapper, DirectionEnum
- **Documentation:** [Input System Architecture](input-system.md)

### 4. Animation System

Coordinates character animations based on movement and state.

- **Purpose:** Drive character sprite animations from gameplay state
- **Key Components:** CharacterAnimationHandler, Animator controllers
- **Documentation:** [Animation System Architecture](animation-system.md)

### 5. Multiplayer Architecture

Client-server packet handling with observer support and map-scoped delivery.

- **Purpose:** Handle multiplayer communication, packet routing, and observer management
- **Key Components:** AvalonPacketHandler, ClientPacketHandler, ServerPacketHandler, Communication Layer Managers
- **Documentation:** [Multiplayer Architecture](multiplayer-architecture.md)

### 6. TextMesh Pro Resources

TextMesh Pro asset management and resource configuration.

- **Purpose:** Ensure text rendering works consistently across all environments and builds
- **Key Components:** TMP fonts, materials, sprites, shaders, settings
- **Documentation:** [TextMesh Pro Resources](textmeshpro-resources.md)

## System Integration

Here's how the systems work together:

```
Player Input
    |
    v
Input System --> Entity Management <--> Combat System
                         |                    |
                         v                    v
                 Animation System      Character Health/Damage
```

1. **Input Flow:** Player input is captured by the Input System and forwarded to registered entities
2. **Entity Coordination:** Entity Management tracks all characters and provides lookup/query capabilities
3. **Combat Flow:** Combat System handles tick-based combat updates, attack timing, and damage application
4. **Visual Feedback:** Animation System updates character sprites based on movement and combat state

## Design Principles

### Event-Driven Architecture

Most systems use events/delegates to decouple components:

- Combat uses `OnPerformCombatHandler` for attack events
- Health changes trigger `OnHealthChangeHandler`
- Input forwarding uses delegate patterns

**Benefits:** Easier to extend, test, and maintain; supports future multiplayer/network synchronization

### Tick-Based Systems

Combat uses a fixed-tick approach synchronized with `FixedUpdate`:

- Predictable timing for gameplay logic
- Easier to synchronize in multiplayer scenarios
- Configurable tick rate for balancing

### ScriptableObject Data

Stats and configuration use ScriptableObjects:

- `CombatBaseStats` for character stats (health, damage, speed)
- `EnemyCombatBaseStats` for enemy-specific properties
- Allows designers to create and modify data without code changes

## Planned Enhancements

### Server Architecture (Future)

The current prototype runs entirely client-side. Future plans include:

- Authoritative server for combat resolution
- Client-side prediction for responsive movement
- Network synchronization of entity state
- Tick-based server updates for deterministic gameplay

### Additional Systems (Future)

- Inventory and item system
- Quest and progression system
- Party and group management
- Chat and social features

## Code Conventions

### Namespacing

- Character-related code lives under `Assets/Scripts/Character/`
- Editor tools are in `Assets/Scripts/Editor/`
- Input handling is in `Assets/Scripts/Input/`

### Naming Conventions

- **Classes:** PascalCase (e.g., `CombatManager`, `PlayerCharacter`)
- **Fields:** Private fields use `_camelCase` with underscore prefix
- **Properties/Methods:** PascalCase
- **Events:** Suffixed with `Handler` (e.g., `OnPerformCombatHandler`)

### Unity Patterns

- MonoBehaviour lifecycle methods are used appropriately (`Start`, `Update`, `FixedUpdate`, `OnEnable`, `OnDisable`)
- Editor-only code is properly guarded for `Assembly-CSharp-Editor`
- ScriptableObjects are used for game data and configuration

## Getting Started with the Code

1. **Read the System Docs:** Start with the architecture docs for systems you're interested in
2. **Explore the Scripts:** Navigate to `Assets/Scripts/` and browse the relevant folders
3. **Run the Sample Scene:** Open `Scenes/SampleScene.unity` to see the systems in action
4. **Check the Prefabs:** Look at character prefabs in `Assets/Prefabs/Characters/` to see component setup

## Related Documentation

- [Features Index](../features/README.md) - User-facing capabilities
- [Contributing Guide](../contributing/README.md) - How to contribute code
- [Code Style Guide](../contributing/code-style.md) - Coding conventions
