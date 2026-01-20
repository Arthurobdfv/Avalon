# Features

This is the central index of all implemented and planned features for the Avalon MMO prototype. Features describe **what** the game can do from a user perspective, while architecture documents describe **how** systems work technically.

## Feature Status Legend

- ✅ **Implemented** - Feature is complete and working
- 🚧 **In Progress** - Feature is partially implemented
- 📋 **Planned** - Feature is designed but not yet implemented
- ❌ **Deprecated** - Feature has been removed or replaced

## Implemented Features

### ✅ Character Animation
**Status:** Implemented (basic)

Character sprite-based animations with support for directional movement.

- Eight-direction walk and idle animations
- Manual animation creation from sprite sheets
- Animator controller integration
- See [Character Animation](character-animation.md) for details

### ✅ Character Movement
**Status:** Implemented

Player movement with input integration and server-tick synchronization.

- WASD and arrow key movement
- Eight-directional movement
- Server-tick synchronized updates
- Input mapper for direction calculation
- See [Character Movement](character-movement.md) for details

### ✅ Combat Flow
**Status:** Implemented (basic)

Basic entity manager and combat system with timed attacks.

- Event-driven combat events
- Attack timing and damage application
- Health tracking and change events
- Player and enemy combat interactions
- See [Combat Flow](combat-flow.md) for details

### ✅ Enemy Behavior
**Status:** Implemented (basic)

Enemy AI with basic behavior management.

- Aggressive and passive enemy types
- Vision and attack range detection
- Movement toward targets
- Enemy behavior manager
- See [Enemy Behavior](enemy-behavior.md) for details

## In Progress Features

### 🚧 Sprite Pipeline
**Status:** In Progress

Automated sprite sheet to animation pipeline for faster iteration.

- ✅ Sprite sheet slicing implemented
- ✅ Animator controller generation (prototype)
- ❌ Automatic animation clip creation
- ❌ Configuration UI
- See [Sprite Pipeline](sprite-pipeline.md) for details

## Planned Features

### 📋 Build Pipeline
**Status:** Planned

Automated CI/CD using GitHub Actions for builds and deployment.

- GitHub Actions workflow for builds
- Deployment to GitHub Pages on merge to master
- Multi-environment support
- See [Build Pipeline](build-pipeline.md) for details

## Adding a New Feature

When implementing a new feature:

1. **Create feature document:**
   - Create `docs/features/<feature-name>.md`
   - Use kebab-case for the filename
   - Follow the feature template (see below)

2. **Update this index:**
   - Add the feature to the appropriate section
   - Include a one-line summary
   - Link to the feature document

3. **Update changelog:**
   - Add entry to `CHANGELOG.md` under `Unreleased`
   - Note the feature addition

4. **Update architecture docs:**
   - If the feature involves new systems, document them in `docs/architecture/`
   - Link feature docs to architecture docs

## Feature Document Template

Each feature document should include:

```markdown
# Feature Name

- **Status:** Draft | In Progress | Implemented | Deprecated
- **Summary:** One-line description of the feature

## Overview
Brief explanation of what this feature does and why it exists.

## User Experience
How users interact with this feature.

## Implementation
Current implementation details and key components.

## Architecture
Link to relevant architecture documentation.

## Usage
How to use or test this feature.

## Known Limitations
Current limitations or issues.

## TODOs / Next Steps
What remains to be done.

## Related Documentation
Links to related features and architecture docs.
```

## Feature vs Architecture

**When to create a feature doc:**
- Describes a user-facing capability
- Explains what the game can do
- Focuses on gameplay experience
- Documents acceptance criteria

**When to create an architecture doc:**
- Describes a technical system
- Explains how something works
- Focuses on implementation details
- Documents design decisions

**Example:**
- **Feature:** "Character Movement" - Players can move using WASD keys
- **Architecture:** "Input System" - How input is captured, mapped, and forwarded to entities

## Related Documentation

- [Architecture Overview](../architecture/README.md) - Technical system designs
- [Contributing Guide](../contributing/README.md) - How to contribute features
- [Documentation Guide](../contributing/documentation-guide.md) - Documentation standards
- [Changelog](../../CHANGELOG.md) - Project history
