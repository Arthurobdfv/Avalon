# Features Documentation

This directory contains user-facing feature documentation for the Avalon MMO prototype. These documents describe implemented and in-progress features from a user and developer perspective.

## Available Features

### Character & Animation

#### [Character Animation](character-animation.md)
**Status:** Implemented (basic)

Character sprite rendering and animation playback with directional support.

**Key Features:**
- Base character spritemap imported
- Animation clips for idle, walk, and directional movement
- `CharacterAnimationHandler` for state coordination
- Eight-direction walk animations

**Next Steps:** Automate animation creation via sprite pipeline

---

#### [Character Movement](character-movement.md)
**Status:** Implemented

Player movement and input handling with server tick synchronization.

**Key Features:**
- Input-driven player movement
- `PlayerCharacter` abstraction for movement state
- Server tick synchronization to avoid duplicate updates
- Integration with animation system

**Next Steps:** Extract input mapping, add multi-device support

---

#### [Sprite Pipeline](sprite-pipeline.md)
**Status:** In Progress

Editor tooling to automate sprite sheet slicing and animation generation.

**Key Features:**
- Automatic sprite sheet slicing
- Grid-based frame extraction
- Animator controller generation (prototype)
- Import configuration via ScriptableObjects

**Next Steps:** Complete AnimationClip generation, add preview UI

---

### Combat & AI

#### [Combat Flow](combat-flow.md)
**Status:** Implemented (basic)

Complete combat system with player attacks, enemy targeting, and damage resolution.

**Key Features:**
- Timed attacks based on stats
- Event-driven damage application
- Player and enemy combat
- Health tracking and UI updates

**Next Steps:** Add death handling, projectiles, server-authoritative combat

---

#### [Enemy Behavior](enemy-behavior.md)
**Status:** In Progress (basic manager implemented)

Centralized enemy AI with targeting and aggro management.

**Key Features:**
- Tick-based behavior refresh
- Aggressive targeting of nearest players
- Range and vision checks
- Target assignment via `EnemyBehaviourManager`

**Next Steps:** Multi-map support, advanced AI decision making

---

## Feature Status Legend

- **Implemented:** Feature is complete and functional
- **Implemented (basic):** Core functionality works, improvements planned
- **In Progress:** Feature partially implemented, active development
- **Planned:** Feature designed but not yet started

## How Features Are Organized

Each feature document includes:

1. **Status** - Current implementation state
2. **Summary** - Brief feature description
3. **Overview** - Detailed explanation
4. **How It Works** - Usage instructions and workflows
5. **Integration Points** - How it connects with other systems
6. **Limitations** - Known issues and constraints
7. **Future Improvements** - Planned enhancements
8. **Related Documentation** - Links to architecture docs and other features

## Feature Development Workflow

### Adding a New Feature

1. **Design:** Create feature document in `docs/features/`
2. **Implement:** Build feature according to design
3. **Document:** Update feature doc with implementation details
4. **Link:** Add to this index and reference from other docs
5. **Changelog:** Add entry to root `CHANGELOG.md`

### Updating an Existing Feature

1. **Change:** Implement modifications
2. **Document:** Update feature doc with changes
3. **Status:** Update status if needed (e.g., In Progress → Implemented)
4. **Changelog:** Note changes in `CHANGELOG.md`

## Integration with Architecture

Features build on top of core architecture systems:

```
Features (user-facing functionality)
    |
    v
Architecture (core systems)
    |
    v
Unity Engine & .NET Framework
```

**Example:**
- **Combat Flow** (feature) uses **Combat System** (architecture)
- **Character Movement** (feature) uses **Input System** (architecture)
- **Enemy Behavior** (feature) uses **Entity Management** (architecture)

For technical architecture details, see [Architecture Documentation](../architecture/README.md).

## Feature Dependencies

### Character Animation
- Depends on: Sprite Pipeline (for clip creation)
- Used by: Character Movement, Combat Flow

### Character Movement
- Depends on: Input System, Entity Management
- Used by: Combat Flow, Animation

### Sprite Pipeline
- Depends on: Unity Editor, Asset Import Pipeline
- Used by: Character Animation

### Combat Flow
- Depends on: Entity Management, Input System, Combat System
- Used by: Enemy Behavior

### Enemy Behavior
- Depends on: Entity Management, Combat System
- Used by: Combat Flow

## Testing Features

### Local Testing
Most features can be tested locally in Unity Editor:
1. Open sample scene
2. Enter play mode
3. Use keyboard/mouse for player input
4. Observe feature behavior

### Multiplayer Testing
Some features require multiplayer setup:
1. Use `ClientTestSend` utility (Alt+P for player, Alt+O for observer)
2. Test with local loopback transport
3. Verify packet flow and synchronization

See [Multiplayer Architecture](../architecture/multiplayer-architecture.md) for setup details.

## Planned Features

### Near Term
- **Build Pipeline** - WebGL and standalone builds
- **UI System** - Menu, HUD, and interaction UI
- **Audio System** - Sound effects and music

### Long Term
- **Skills & Abilities** - Player skill system
- **Inventory** - Item management
- **Quests** - Quest and progression system
- **Social Features** - Parties, guilds, chat

## Related Documentation

- [Architecture Documentation](../architecture/README.md) - Technical system design
- [Contributing Guide](../contributing/README.md) - How to contribute
- [Documentation Hub](../README.md) - Main documentation index
- Root `CHANGELOG.md` - Change history and unreleased features

## File Organization

- `character-animation.md` - Character sprite and animation feature
- `character-movement.md` - Player movement and input
- `sprite-pipeline.md` - Automated sprite sheet processing
- `combat-flow.md` - Combat system feature
- `enemy-behavior.md` - Enemy AI and targeting

## Contributing to Features

When working on features:

1. **Read architecture docs first** to understand underlying systems
2. **Update feature doc** as you make changes
3. **Add tests** where applicable (manual test steps at minimum)
4. **Update changelog** with notable changes
5. **Link related docs** to help others navigate

For detailed contribution guidelines, see [Contributing Guide](../contributing/README.md).
