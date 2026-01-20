# Character Animation

- **Status:** Implemented (basic)
- **Summary:** Character sprite-based animations with directional movement support and manual animation creation from imported sprite sheets.

## Overview

This feature provides visual representation for characters through sprite-based animations. Characters display different animation frames based on their movement direction and actions, creating a more engaging visual experience.

The current implementation supports eight-directional movement animations (idle and walk states) created manually from imported sprite sheets.

## User Experience

Players see:
- Smooth character animations when moving
- Different sprites for each movement direction (N, NE, E, SE, S, SW, W, NW)
- Idle animations when standing still
- Proper character facing direction based on input

## Implementation

### Current State

- Base character sprite sheets imported into `Assets/Sprites/Characters/`
- Manual animation clips created in `Assets/Animations/Characters/`
- Animator controllers configured in `Assets/Animations/Controllers/`
- Eight-direction walk animation clips for sample characters
- Eight-direction idle animation clips
- `CharacterAnimationHandler` component coordinates animation state changes

### Key Components

**CharacterAnimationHandler:**
- Receives direction updates from character movement
- Maps `DirectionEnum` values to animator parameters
- Manages animation state transitions
- Handles animation triggers for actions

**Animation Clips:**
- Manual frame-by-frame animation creation
- Naming convention: `<Action>_<Direction>.anim` (e.g., `Walk_North.anim`)
- Located in character-specific folders

**Animator Controllers:**
- State machine with idle and walk states for each direction
- Transitions based on `IsMoving` and `Direction` parameters
- Future support for attack and hit animations

### Integration

The animation system integrates with:
- **Input System** - Direction from player input drives animations
- **Character System** - Character state changes trigger animation updates
- **Combat System** - Combat actions will trigger attack animations (planned)

## Architecture

See [Animation System Architecture](../architecture/animation-system.md) for technical details on:
- Animation update flow
- Direction to animation mapping
- Animator state machine structure
- Integration with other systems

## Usage

### For Players

Simply move your character using WASD or arrow keys. The character automatically plays the appropriate animation for the direction of movement.

### For Developers

**Adding animations to a new character:**

1. Import sprite sheet to `Assets/Sprites/Characters/<CharacterName>/`
2. Slice sprites using Unity Sprite Editor
3. Create animation clips in Animation window
4. Set up Animator Controller with states and transitions
5. Add CharacterAnimationHandler to character prefab
6. Assign Animator Controller

**Example clip creation:**
```
1. Select character sprites for "Walk North" action
2. Window > Animation > Create New Clip
3. Save as "Walk_North.anim"
4. Drag sprite frames into animation timeline
5. Adjust frame rate (typically 8-12 FPS for walk cycles)
6. Repeat for other directions
```

## Known Limitations

### Manual Creation Only

Currently, all animation clips must be created manually:
- Time-consuming for many directions and actions
- Error-prone and inconsistent
- Hard to iterate when sprite sheets change

**Mitigation:** Sprite pipeline automation is in progress (see [Sprite Pipeline](sprite-pipeline.md))

### Limited Action Set

Only idle and walk animations are implemented:
- No attack animations yet
- No hit/hurt reactions
- No death animations
- No emote or special actions

### No Animation Blending

Transitions are instant with no blending:
- Can look abrupt when changing directions quickly
- No smooth rotation or turning animations

### No Runtime Animation Events

Missing integration with animation events:
- No footstep sound synchronization
- No particle effects on specific frames
- No hit detection timing for attacks

## Release Notes

**Initial Implementation:**
- Imported base character sprite sheet
- Created example idle and walk animations by hand
- Set up basic Animator Controller

**Eight-Direction Update:**
- Added full set of eight-direction walk animations
- Added eight-direction idle animations
- Introduced `CharacterAnimationHandler` to coordinate state changes
- Updated sample scene to demonstrate directional animations

## Developer Notes

The current animation handler uses direct updates from the character controller:

```csharp
// In PlayerCharacter
public void Move(DirectionEnum direction) {
    animationHandler.UpdateDirection(direction);
    // ... movement logic
}
```

**Future consideration:** Refactor to event-driven approach to decouple input/character logic from animation playback:

```csharp
// Event-driven approach
character.OnDirectionChanged += animationHandler.UpdateDirection;
```

This would make the system more flexible and easier to extend with multiple animation handlers or visual effects.

## Next Steps

### Immediate Goals

- [ ] Add attack animations for combat
- [ ] Add hit/hurt reaction animations
- [ ] Add death animations
- [ ] Implement animation events for sounds and effects

### Automation Goals

- [ ] Integrate with automated sprite pipeline (see [Sprite Pipeline](sprite-pipeline.md))
- [ ] Auto-generate animation clips from sliced sprites
- [ ] Auto-create animator controllers with standard states
- [ ] Support configuration for frame rates and clip naming

### Polish Goals

- [ ] Add animation blending for smoother transitions
- [ ] Implement facing direction "memory" (keep facing last move direction)
- [ ] Add special animations (emotes, gestures, etc.)
- [ ] Support animation layers for equipment/effects

## Related Documentation

- [Animation System Architecture](../architecture/animation-system.md) - Technical implementation details
- [Character Movement](character-movement.md) - How movement drives animations
- [Sprite Pipeline](sprite-pipeline.md) - Automated animation creation (in progress)
- [Input System Architecture](../architecture/input-system.md) - How input determines direction
- [Features Index](README.md) - All features
