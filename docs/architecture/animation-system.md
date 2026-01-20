# Animation System Architecture

## Overview

The animation system coordinates character sprite animations based on movement direction and game state. It bridges the input and character systems with Unity's Animator, ensuring characters display the correct animations for their current actions.

## Purpose

- Drive character sprite animations from gameplay state
- Map movement directions to appropriate animation clips
- Handle animation state transitions
- Support directional animations (8-way movement)
- Integrate with Unity's Animator system

## Core Components

### CharacterAnimationHandler

**Type:** MonoBehaviour  
**Purpose:** Coordinates animation state based on character movement and actions

**Responsibilities:**
- Receive direction updates from character controller
- Map directions to animation states
- Update Animator parameters
- Handle animation transitions
- Manage animation state machine

**Integration:**
```csharp
// Called by PlayerCharacter when direction changes
public void UpdateDirection(DirectionEnum direction) {
    currentDirection = direction;
    UpdateAnimatorState();
}

private void UpdateAnimatorState() {
    // Map direction to animator parameter
    animator.SetInteger("Direction", (int)currentDirection);
    animator.SetBool("IsMoving", currentDirection != DirectionEnum.None);
}
```

### Animation Clips

**Type:** Unity AnimationClip assets  
**Purpose:** Individual sprite frame sequences for character actions

**Organization:**
- `Assets/Animations/Characters/<CharacterName>/` - Character-specific clips
- Naming convention: `<Action>_<Direction>.anim` (e.g., `Walk_North.anim`, `Idle_East.anim`)

**Common Clips:**
- **Idle animations** - One per direction (N, NE, E, SE, S, SW, W, NW)
- **Walk animations** - One per direction
- **Attack animations** - One per direction (planned)
- **Hit/Hurt animations** - Directional or omnidirectional
- **Death animations** - Typically one or omnidirectional

### Animator Controllers

**Type:** Unity AnimatorController assets  
**Purpose:** State machines that manage animation transitions

**Location:** `Assets/Animations/Controllers/<CharacterName>.controller`

**State Machine Structure:**
```
Idle States (8 directions)
    |
    +-- Transition on IsMoving = true -> Walk States
    |
Walk States (8 directions)
    |
    +-- Transition on IsMoving = false -> Idle States
    |
    +-- Transition on AttackTrigger -> Attack States
    |
Attack States (8 directions)
    |
    +-- Transition on animation complete -> Previous state
```

**Animator Parameters:**
- `Direction` (int) - Current facing direction (0-8 mapped from DirectionEnum)
- `IsMoving` (bool) - Whether character is moving
- `AttackTrigger` (trigger) - Triggers attack animation
- `IsAlive` (bool) - Controls death state transitions

## How It Works

### Animation Update Flow

```
1. Character movement/action occurs
   |
2. Character updates direction
   - PlayerCharacter.Move(direction)
   |
3. CharacterAnimationHandler receives update
   - UpdateDirection(direction)
   |
4. Handler maps direction to animator parameters
   - animator.SetInteger("Direction", directionValue)
   |
5. Animator transitions to appropriate state
   - State machine evaluates conditions
   |
6. Animation clip plays
   - Sprite renderer updates each frame
```

### Direction to Animation Mapping

DirectionEnum values map to animation states:

| Direction | Enum Value | Animation Suffix | Example Clip |
|-----------|-----------|------------------|--------------|
| None | 0 | - | Idle_South (default) |
| N | 1 | North | Walk_North |
| NE | 2 | NorthEast | Walk_NorthEast |
| E | 3 | East | Walk_East |
| SE | 4 | SouthEast | Walk_SouthEast |
| S | 5 | South | Walk_South |
| SW | 6 | SouthWest | Walk_SouthWest |
| W | 7 | West | Walk_West |
| NW | 8 | NorthWest | Walk_NorthWest |

### Animation Transition Logic

Transitions between states use conditions:

**Idle to Walk:**
```
Condition: IsMoving == true
Duration: 0.1s
Exit Time: None
```

**Walk to Idle:**
```
Condition: IsMoving == false
Duration: 0.1s
Exit Time: None
```

**Any to Attack:**
```
Condition: AttackTrigger
Duration: 0s
Exit Time: None
Priority: High
```

**Attack to Previous:**
```
Condition: Animation complete
Duration: 0.1s
Exit Time: 1.0 (end of animation)
```

## Integration Points

### Input System

Input direction drives animation selection:

```csharp
// In PlayerCharacter
public void Move(DirectionEnum direction) {
    // Update movement
    ApplyMovement(direction);
    
    // Update animation
    animationHandler.UpdateDirection(direction);
}
```

See [Input System Architecture](input-system.md)

### Character System

Characters notify animation handler of state changes:

```csharp
// In CombatCharacter
public void PerformAttack() {
    animationHandler.TriggerAttack(currentDirection);
    // ... combat logic
}

public void TakeDamage(int damage) {
    CurrentHealth -= damage;
    animationHandler.TriggerHit();
    // ... damage logic
}
```

See [Combat System Architecture](combat-system.md)

### Sprite Pipeline

Animation clips are created from sprite sheets:

- Manual creation for now
- Future: Automated via custom importer
- Sprite slicing feeds into animation creation

See [Sprite Pipeline Feature](../features/sprite-pipeline.md)

## Usage Guide

### Setting Up Character Animations

1. **Import sprite sheet:**
   - Place in `Assets/Sprites/Characters/<CharacterName>/`
   - Configure sprite import settings (Multiple mode, PPU, pivot)

2. **Slice sprites:**
   - Use Unity Sprite Editor
   - Create grid slicing (e.g., 64x64 px per frame)
   - Name slices consistently

3. **Create animation clips:**
   - Select sprite frames for an action
   - Create animation clip via Animation window
   - Save in `Assets/Animations/Characters/<CharacterName>/`
   - Repeat for each direction and action

4. **Create Animator Controller:**
   - Create controller in `Assets/Animations/Controllers/`
   - Add parameters (Direction, IsMoving, etc.)
   - Create states for each animation clip
   - Add transitions with conditions

5. **Attach to character:**
   - Add Animator component to character prefab
   - Assign Animator Controller
   - Add CharacterAnimationHandler script
   - Link Animator reference

### Adding New Animation Actions

To add a new action (e.g., "Jump"):

1. **Create animation clips:**
   - Create `Jump_North.anim`, `Jump_East.anim`, etc.
   - Set up frame sequences in Animation window

2. **Update Animator:**
   - Add new parameter (e.g., `JumpTrigger`)
   - Create new Jump states
   - Add transitions from locomotion states to Jump
   - Add transitions from Jump back to locomotion

3. **Update CharacterAnimationHandler:**
```csharp
public void TriggerJump() {
    animator.SetTrigger("JumpTrigger");
}
```

4. **Call from character:**
```csharp
// In PlayerCharacter
if (Input.GetButtonDown("Jump")) {
    animationHandler.TriggerJump();
}
```

## Design Decisions

### Why DirectionEnum Mapping?

Using enum values for directions:

**Pros:**
- Type-safe direction representation
- Easy to map to animation clips
- Works well with sprite-based games
- Clear naming in animator states

**Cons:**
- Requires animation clip for each direction
- More animator states to manage
- Asset bloat with many actions

**Alternative:** Flip sprites horizontally for opposite directions (reduces clips by half)

### Event-Driven vs Direct Updates

Current implementation uses direct updates:

```csharp
// Direct approach (current)
character.Move(direction);
animationHandler.UpdateDirection(direction);
```

**Future consideration:** Event-driven approach:

```csharp
// Event-driven approach (potential)
character.OnDirectionChanged += animationHandler.UpdateDirection;
character.Move(direction); // Fires event internally
```

Benefits of events:
- Decouples character from animation handler
- Multiple systems can react to direction changes
- Easier to add/remove animation handlers

### Animator vs Script-Based

Using Unity Animator instead of script-based animation:

**Pros:**
- Visual state machine editing
- Built-in transition blending
- Animation events support
- State behavior hooks

**Cons:**
- More complex setup
- Harder to debug
- Performance overhead for simple animations
- Black-box behavior

## Known Limitations

### Manual Animation Creation

Currently requires manually creating all animation clips:
- Time-consuming for many directions/actions
- Error-prone (easy to miss frames)
- Hard to maintain consistency

**Solution:** Implement automated sprite-to-animation pipeline (in progress)

### No Animation Blending

Current setup uses instant transitions:
- No smooth blending between animations
- Can look snappy or jarring
- Limited by sprite-based rendering

### Direction Changes During Actions

Animation doesn't always reflect direction changes:
- If attacking, direction change queued until attack completes
- Can lead to facing wrong direction briefly

### No Animation Events Integration

Missing integration with animation events:
- No sound effect triggers from animation
- No hit-frame detection for attacks
- No footstep synchronization

## Future Enhancements

### Automated Pipeline

Sprite-to-animation automation:
- Auto-slice sprite sheets based on config
- Auto-generate animation clips for each direction
- Auto-create animator controller with states
- See [Sprite Pipeline Feature](../features/sprite-pipeline.md)

### Animation Events

Add animation event support:
- Sound effect triggers
- Hit detection frames
- Particle effect spawning
- Camera shake timing

### Advanced Transitions

Improve animation transitions:
- Blend trees for smooth direction changes
- Animation layers (upper/lower body)
- Additive animations (e.g., breathing)
- IK for weapon holding

### State-Driven Animations

Expand state-driven approach:
- Combat state animations
- Emotion/expression overlays
- Equipment visual changes
- Damage/buff visual effects

## File Reference

- `Assets/Scripts/Character/CharacterAnimationHandler.cs`
- `Assets/Animations/Characters/` - Animation clips
- `Assets/Animations/Controllers/` - Animator controllers
- `Assets/Sprites/Characters/` - Character sprite sheets

## Related Documentation

- [Input System Architecture](input-system.md)
- [Character Animation Feature](../features/character-animation.md)
- [Sprite Pipeline Feature](../features/sprite-pipeline.md)
- [Architecture Overview](README.md)
