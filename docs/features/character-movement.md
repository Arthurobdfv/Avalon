# Character Movement

- **Status:** Implemented
- **Summary:** Player movement with keyboard input integration and server-tick synchronization to avoid redundant updates.

## Overview

This feature enables players to control their character using keyboard input (WASD or arrow keys). Movement is synchronized with server ticks to ensure consistent updates and avoid multiple movement calculations per game tick.

## User Experience

Players can:
- Move their character using WASD keys or arrow keys
- Move in eight directions (N, NE, E, SE, S, SW, W, NW)
- See smooth character movement with appropriate animations
- Change direction instantly by pressing different keys

## Implementation

### Current State

**Input Handling:**
- `PlayerInputHandler` captures keyboard input each frame
- Input axes (Horizontal, Vertical) are polled from Unity Input Manager
- Raw input is forwarded to the character controller

**Direction Mapping:**
- `PlayerInputMapper` converts raw input to `DirectionEnum`
- Uses angle calculation to quantize to eight directions
- Handles deadzone for "no movement" state

**Movement Application:**
- `PlayerCharacter` receives direction input
- Movement is synchronized to server tick
- Only one movement update per tick to avoid redundant calculations
- Movement updates are synchronized to prevent double updates

**Character Abstraction:**
- `PlayerCharacter` class encapsulates movement logic
- Separates input processing from movement application
- Maintains movement state for the current tick

### Key Components

**PlayerInputHandler:**
```csharp
void Update() {
    float h = Input.GetAxis("Horizontal");
    float v = Input.GetAxis("Vertical");
    DirectionEnum direction = PlayerInputMapper.MapToDirection(h, v);
    playerCharacter.Move(direction);
}
```

**PlayerInputMapper:**
```csharp
public static DirectionEnum MapToDirection(float horizontal, float vertical) {
    // Angle-based quantization to 8 directions
    // Handles deadzone for DirectionEnum.None
}
```

**PlayerCharacter:**
```csharp
public void Move(DirectionEnum direction) {
    // Apply movement based on direction
    // Update animation
    // Synchronize with server tick
}
```

### Server Tick Synchronization

Movement updates are synchronized to avoid redundant calculations:
- Only one movement update processed per game tick
- Prevents multiple network messages for the same movement
- Ensures consistent timing across clients (future multiplayer)

## Architecture

See [Input System Architecture](../architecture/input-system.md) for technical details on:
- Input capture and processing flow
- Direction mapping algorithm
- Input state tracking
- Integration with character system

## Usage

### For Players

1. Use **WASD** or **arrow keys** to move:
   - W / Up - Move North
   - D / Right - Move East
   - S / Down - Move South
   - A / Left - Move West
   - Combinations (W+D) - Move diagonally (NE, SE, SW, NW)

2. Release keys to stop moving

3. Character automatically faces the direction of movement

### For Developers

**Configuring movement speed:**

Currently hardcoded in character scripts. Planned to be configurable via CharacterStats ScriptableObject:

```csharp
// Future configuration
[SerializeField] private CharacterStats stats;
float speed = stats.MovementSpeed;
```

**Adding new input sources:**

To add gamepad support:
1. Configure gamepad axes in Unity Input Manager
2. Update `PlayerInputHandler` to read gamepad input
3. Merge keyboard and gamepad input before mapping

## Known Limitations

### Hardcoded Movement Speed

Movement speed is currently hardcoded in the PlayerCharacter script:
- Should be configurable via ScriptableObject
- Different characters should have different speeds
- No runtime speed modifications (buffs, debuffs)

**TODO:** Extract movement speed to configurable character attribute

### No Input Device Selection

Input system doesn't distinguish between multiple input sources:
- Keyboard and gamepad both control the same character
- No per-device player assignment
- Not suitable for local multiplayer

### Limited to 8 Directions

Movement is quantized to eight cardinal directions:
- No free 360-degree analog movement
- Gamepad analog sticks are reduced to 8 directions
- May feel restrictive with analog input

**Alternative:** Could add toggle for analog movement mode

### No Movement Modifiers

Missing common movement features:
- No sprint/run modifier
- No crouch or slow walk
- No jump or dodge
- No swimming or climbing

## Developer Notes

**TODOs found in code:**

From `PlayerInputHandler`:
- Extract input helpers into separate modules for clarity
- Replace hardcoded movement speed with configurable character attribute

**Suggested improvements:**
- Document server tick synchronization strategy for contributors
- Create wiki page explaining the tick synchronization pattern
- Add examples of how to follow the same pattern for other systems

## Next Steps

### Input Improvements

- [ ] Extract movement speed to CharacterStats ScriptableObject
- [ ] Add sprint modifier (hold Shift for faster movement)
- [ ] Support gamepad input with proper analog stick handling
- [ ] Add input buffering for smoother movement
- [ ] Implement deadzone configuration for analog sticks

### Movement Features

- [ ] Add movement acceleration/deceleration
- [ ] Implement collision detection and blocking
- [ ] Add pathfinding for click-to-move
- [ ] Support movement prediction for multiplayer
- [ ] Add terrain effects (slow in water, fast on roads)

### Testing

- [ ] Add editor tests for direction mapping
- [ ] Validate server tick synchronization behavior
- [ ] Test with different fixed update rates
- [ ] Profile movement performance

## Related Documentation

- [Input System Architecture](../architecture/input-system.md) - Input processing details
- [Character Animation](character-animation.md) - How movement affects animations
- [Architecture Overview](../architecture/README.md) - Overall system design
- [Features Index](README.md) - All features
