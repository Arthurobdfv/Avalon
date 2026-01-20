# Animation System Architecture

## Overview

The animation system manages character animations based on movement, direction, and game state. It coordinates between the input system, character state, and Unity's Animator component.

## Core Components

### CharacterAnimationHandler

The `CharacterAnimationHandler` coordinates animation state changes from movement and direction input.

**Responsibilities:**
- Listens to character movement and direction changes
- Updates Animator parameters based on character state
- Manages directional animation blending (8-direction movement)
- Synchronizes animation state with gameplay

**Animation Parameters:**
- Movement speed/velocity
- Direction (8-way or continuous)
- Character state (idle, walking, attacking, etc.)
- Additional gameplay states

**Location:** `Assets/Scripts/Character/Animation/CharacterAnimationHandler.cs`

### Animation States

The system supports multiple animation states:

- **Idle:** Character is stationary
- **Walk:** Character is moving at normal speed
- **Sprint:** Character is moving at increased speed (if implemented)
- **Attack:** Character is performing combat action
- **Directional Variants:** Each state may have directional variants (N, NE, E, SE, S, SW, W, NW)

## Directional Animation

The system supports 8-direction animation:

```
NW    N    NE
  \   |   /
   \  |  /
W ---+--- E
   /  |  \
  /   |   \
SW    S    SE
```

Animation clips are selected based on:
1. Character's current state (idle/walk/attack)
2. Character's facing direction
3. Appropriate directional variant of the state

## Animation Update Flow

```
Input System
    |
    v
Character State Update
    |
    v
CharacterAnimationHandler
    |
    v
Animator Parameters Update
    |
    v
Unity Animator
    |
    v
Animation Playback
```

## Integration Points

- **Input System:** Direction and movement speed from player input
- **Character System:** Character state (idle/moving/attacking)
- **Sprite Pipeline:** Animation clips generated from sprite sheets
- **Combat System:** Attack animations triggered by combat events

## Animator Controller Structure

The Animator Controller contains:

1. **States:** One per animation (idle, walk, attack, etc.)
2. **Transitions:** Rules for moving between states
3. **Parameters:** Controlled by `CharacterAnimationHandler`
4. **Blend Trees:** Optional, for smooth directional blending

## Animation Creation Pipeline

Animations are created through the sprite sheet pipeline:

1. Import sprite sheet texture
2. Slice into individual sprites
3. Generate AnimationClips from sprite sequences
4. Wire into AnimatorController with transitions

See [Sprite Pipeline](../features/sprite-pipeline.md) for details.

## Current Implementation Notes

### Strengths
- Clean separation between input, state, and animation
- Support for directional animations
- Integrates with Unity's Animator for complex state machines

### Known Issues
- Direct updates from handler (consider event-driven approach)
- Manual animation clip creation (being automated via sprite pipeline)
- No animation event system for gameplay triggers

## Performance Considerations

- Animator updates are handled by Unity's internal systems
- Animation state changes should be minimized
- Consider animator culling for off-screen characters
- Sprite animation is generally lightweight

## Future Improvements

- Refactor to event-driven approach to decouple input from animation
- Implement animation events for gameplay triggers (footsteps, attack impacts)
- Add animation blending for smoother transitions
- Support for additive animations (e.g., looking while walking)
- Animation state debugging/visualization tools
- Network synchronization of animation state for multiplayer

## Example Usage

```csharp
// CharacterAnimationHandler automatically updates based on:
// - Character movement (from PlayerInputHandler)
// - Character direction (from input or AI)
// - Character state (idle, moving, attacking)

// No direct API calls needed - system responds to character state changes
```

## Developer Notes

From the codebase:
- Current animation handler uses direct updates
- Consider refactoring to event-driven approach to decouple input/character logic from animation playback
- All animation clips currently created manually; automation via sprite pipeline is in progress

## Related Documentation

- [Sprite Pipeline](../features/sprite-pipeline.md) - Automated animation creation
- [Character Animation](../features/character-animation.md) - Feature documentation
- [Input System](input-system.md) - Input driving animation state
- [Character Movement](../features/character-movement.md) - Movement integration
