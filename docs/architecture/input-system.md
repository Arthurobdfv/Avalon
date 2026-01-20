# Input System Architecture

## Overview

The input system handles player keyboard and controller input, maps it to character actions, and manages directional movement. It provides a layer between raw input and game entities, enabling input remapping and supporting multiple input devices.

## Purpose

- Capture and process player input from keyboard/controller
- Map raw input to character movement directions
- Forward action inputs to appropriate entities
- Support input state tracking and validation
- Enable future input remapping and customization

## Core Components

### PlayerInputHandler

**Type:** MonoBehaviour  
**Purpose:** Main input processor that captures raw input and forwards it to entities

**Responsibilities:**
- Poll input each frame (keyboard, controller)
- Detect movement input (WASD, arrow keys, joystick)
- Capture action inputs (attack, interact, jump)
- Forward processed input to PlayerCharacter
- Maintain input state for the current frame

**Input Capture:**
```csharp
void Update() {
    // Capture movement
    float horizontal = Input.GetAxis("Horizontal");
    float vertical = Input.GetAxis("Vertical");
    
    // Capture actions
    bool attackPressed = Input.GetButtonDown("Fire1");
    
    // Forward to entity
    playerCharacter.ProcessInput(horizontal, vertical, attackPressed);
}
```

### PlayerInputMapper

**Type:** Static utility class  
**Purpose:** Maps raw input values to game-specific directions and states

**Key Methods:**

#### `DirectionEnum MapToDirection(float horizontal, float vertical)`

Maps 2D input axes to 8-directional movement.

**Input:** 
- `horizontal` - X-axis input (-1 to 1)
- `vertical` - Y-axis input (-1 to 1)

**Output:**
- `DirectionEnum` value (N, NE, E, SE, S, SW, W, NW, or None)

**Implementation:**
- Uses angle calculation from input vector
- Quantizes to nearest 45-degree direction
- Handles deadzone for "None" direction

**Example:**
```csharp
DirectionEnum direction = PlayerInputMapper.MapToDirection(horizontal, vertical);
character.Move(direction);
```

### DirectionEnum

**Type:** Enumeration  
**Purpose:** Represents the eight cardinal and intercardinal directions plus a neutral state

**Values:**
```csharp
public enum DirectionEnum {
    None,   // No movement
    N,      // North (Up)
    NE,     // Northeast
    E,      // East (Right)
    SE,     // Southeast
    S,      // South (Down)
    SW,     // Southwest
    W,      // West (Left)
    NW      // Northwest
}
```

**Usage:**
- Movement direction for characters
- Animation state selection
- Targeting and facing direction
- Collision and interaction direction

## How It Works

### Input Processing Flow

```
1. PlayerInputHandler.Update()
   |
2. Poll Unity Input System
   - GetAxis("Horizontal")
   - GetAxis("Vertical")
   - GetButtonDown("Fire1")
   |
3. Map to game directions
   - PlayerInputMapper.MapToDirection()
   |
4. Package input state
   - Direction: DirectionEnum
   - Actions: bool flags
   |
5. Forward to PlayerCharacter
   - character.ProcessInput()
   |
6. Character processes input
   - Apply movement
   - Trigger actions
   - Update animations
```

### Direction Mapping Algorithm

The direction mapping uses angle calculation to quantize input:

```
Input Vector (horizontal, vertical)
   |
Calculate Angle: atan2(vertical, horizontal)
   |
Quantize to 45-degree sectors:
   -22.5° to 22.5° -> E
    22.5° to 67.5° -> NE
    67.5° to 112.5° -> N
   ... etc
   |
Return DirectionEnum
```

**Special Cases:**
- Input magnitude < deadzone threshold -> DirectionEnum.None
- Exactly on sector boundary -> rounds to nearest direction

### Input State Tracking

The input handler maintains state for:
- **Current frame input** - What's being pressed now
- **Input down events** - Buttons pressed this frame
- **Input up events** - Buttons released this frame
- **Held duration** - How long buttons have been held

This enables:
- Hold-to-charge mechanics
- Double-tap detection
- Input buffering
- Action combos

## Integration Points

### Character Movement

Input direction is forwarded to character for movement:

```csharp
// In PlayerInputHandler
DirectionEnum moveDirection = PlayerInputMapper.MapToDirection(h, v);
playerCharacter.Move(moveDirection);

// In PlayerCharacter
public void Move(DirectionEnum direction) {
    Vector3 movement = DirectionToVector(direction);
    transform.Translate(movement * speed * Time.deltaTime);
}
```

See [Character Movement Feature](../features/character-movement.md)

### Animation System

Direction enum drives character sprite animations:

```csharp
// In CharacterAnimationHandler
public void UpdateAnimation(DirectionEnum direction) {
    switch (direction) {
        case DirectionEnum.N:
            animator.Play("Walk_North");
            break;
        case DirectionEnum.E:
            animator.Play("Walk_East");
            break;
        // ... other directions
    }
}
```

See [Animation System Architecture](animation-system.md)

### Combat System

Action inputs trigger combat actions:

```csharp
// In PlayerInputHandler
if (Input.GetButtonDown("Fire1")) {
    playerCharacter.PerformAttack();
}

// In PlayerCharacter
public void PerformAttack() {
    if (CanAttack()) {
        TriggerCombatEvent();
    }
}
```

See [Combat System Architecture](combat-system.md)

## Input Configuration

### Unity Input Manager

The system relies on Unity's Input Manager settings:

**Axes configured:**
- `Horizontal` - A/D keys, Left/Right arrows, Left stick X
- `Vertical` - W/S keys, Up/Down arrows, Left stick Y

**Buttons configured:**
- `Fire1` - Left mouse, Ctrl, Gamepad button 0 (A)
- `Fire2` - Right mouse, Alt, Gamepad button 1 (B)
- `Jump` - Space, Gamepad button 2 (X)

### Customization

To add new inputs:
1. Add axis/button to Unity Input Manager
2. Update `PlayerInputHandler` to capture the input
3. Add corresponding action method to `PlayerCharacter`
4. Update UI to show the new input

## Design Decisions

### Why DirectionEnum?

**Pros:**
- Clear, type-safe direction representation
- Easy to use in switch statements
- Works well with 2D sprite animations
- Matches common game control schemes

**Cons:**
- Limited to 8 directions (no free 360° movement)
- Requires mapping for isometric games
- Not suitable for analog movement

**Alternative:** Vector2 for free movement (may be added later)

### Why Separate Mapper?

Separating input mapping from input handling:
- Easier to test direction logic
- Can be reused for AI "virtual" input
- Simplifies input remapping
- Cleaner architecture

### Polling vs Events

Current system uses polling in `Update()`:

**Pros:**
- Simple to implement
- Predictable timing
- Works with Unity's input system

**Future:** Consider new Unity Input System for:
- Event-based input
- Better multi-device support
- Input action maps
- Rebindable controls

## Usage Guide

### Setting Up Input

1. **Add PlayerInputHandler to scene:**
```csharp
// Attach to a GameObject (e.g., GameManager)
GameObject inputManager = new GameObject("InputManager");
inputManager.AddComponent<PlayerInputHandler>();
```

2. **Assign player reference:**
```csharp
PlayerInputHandler handler = GetComponent<PlayerInputHandler>();
handler.playerCharacter = FindObjectOfType<PlayerCharacter>();
```

3. **Configure Input Manager:**
- Open Edit > Project Settings > Input Manager
- Verify Horizontal/Vertical axes are configured
- Add any custom buttons needed

### Mapping Custom Directions

To add custom direction mapping:

```csharp
public static DirectionEnum MapToCustomDirection(float h, float v) {
    // Custom mapping logic
    // e.g., snap to 4 directions instead of 8
    
    if (Mathf.Abs(h) > Mathf.Abs(v)) {
        return h > 0 ? DirectionEnum.E : DirectionEnum.W;
    } else if (Mathf.Abs(v) > 0.1f) {
        return v > 0 ? DirectionEnum.N : DirectionEnum.S;
    }
    
    return DirectionEnum.None;
}
```

## Known Limitations

### No Input Buffering

Currently no support for:
- Input buffering (queue inputs during busy states)
- Input prediction
- Rollback for missed inputs

### Single Player Only

Current limitations:
- Only supports one player
- No splitscreen or local multiplayer
- No separate input contexts

### Legacy Input System

Uses Unity's legacy Input Manager:
- No action-based mapping
- Limited rebinding support
- Device-specific handling required

## Future Enhancements

### New Input System Migration

Plans to migrate to Unity's new Input System:
- Input Action Assets for cleaner configuration
- Event-based input processing
- Better controller support and rumble
- Runtime rebinding UI

### Input Features

Planned additions:
- Input buffering for combo systems
- Dead zone configuration
- Input sensitivity settings
- Accessibility options (hold to toggle, etc.)

### Multi-Input Support

Future multiplayer support:
- Per-player input contexts
- Multiple simultaneous inputs
- Input device assignment
- Splitscreen input isolation

## File Reference

- `Assets/Scripts/Input/Handler/PlayerInputHandler.cs`
- `Assets/Scripts/Input/PlayerInputMapper.cs`
- `Assets/Scripts/Input/DirectionEnum.cs`
- `Assets/Scripts/Character/Player/PlayerCharacter.cs` (input consumer)

## Related Documentation

- [Character Movement Feature](../features/character-movement.md)
- [Animation System Architecture](animation-system.md)
- [Combat System Architecture](combat-system.md)
- [Architecture Overview](README.md)
