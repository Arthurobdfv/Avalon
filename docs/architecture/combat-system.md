# Combat System Architecture

## Overview

A lightweight, event-driven combat system for character interactions. The system uses a tick-based approach synchronized with Unity's `FixedUpdate` to manage attack timing, damage application, and health tracking.

## Purpose

- Manage character health and combat stats
- Handle attack timing using a fixed-tick system
- Resolve damage through event-driven architecture
- Support both player and enemy combat interactions

## Core Components

### CombatManager

**Type:** MonoBehaviour  
**Purpose:** Global combat coordinator that manages the tick system and damage resolution

**Configuration:**
- `_tickInterval` - Number of FixedUpdate calls between combat ticks

**Events:**
- `static OnCombatTick CombatTickHandler(float previousTickTime, float currentTickTime, float delta)` - Invoked periodically with tick timing information

**Behavior:**
- Drives the global combat tick from `FixedUpdate`
- Subscribes to `CombatCharacter.OnPerformCombatHandler` to handle attack events
- Applies damage directly when characters attack: `target.CurrentHealth -= source.BaseStats.AttackDamage`

### CombatCharacter

**Type:** MonoBehaviour (inherits from `Character`)  
**Purpose:** Base class for characters that participate in combat

**Fields:**
- `BaseStats` - Reference to `CombatBaseStats` ScriptableObject
- `CurrentHealth` - Current health value (runtime)
- `_currentTime` - Attack timer accumulator (private)
- `Target` - Current combat target (CombatCharacter)

**Events:**
- **Instance:** `OnHealthChangeHandler(HealthChangeEventArgs)` - Fired when CurrentHealth changes
- **Static:** `OnPerformCombatHandler(PerformCombatEventArgs)` - Fired when a character performs an attack

**Behavior:**
- Subscribes to `CombatManager.CombatTickHandler` in `OnEnable`
- Accumulates tick delta into `_currentTime` each tick
- Checks if attack is ready: `_currentTime > BaseStats.AttackSpeed && IsInRange(Target, BaseStats.AttackRange)`
- Performs attack when conditions are met:
  1. Invokes `OnPerformCombatHandler` with source and target
  2. Resets `_currentTime` to 0
- Triggers `OnHealthChangeHandler` when health changes

### CombatBaseStats

**Type:** ScriptableObject  
**Purpose:** Data container for character combat statistics

**Fields:**
- `Health` - Maximum health
- `AttackDamage` - Damage dealt per attack
- `AttackSpeed` - Time between attacks (in seconds)
- `AttackRange` - Maximum attack distance
- `MovementSpeed` - Movement speed

**Creation:** Unity menu `Character/Combat Base Stats`

### EnemyCombatBaseStats

**Type:** ScriptableObject (inherits `CombatBaseStats`)  
**Purpose:** Enemy-specific combat statistics

**Additional Fields:**
- `Aggressive` - Whether enemy actively seeks targets
- `VisionRange` - Distance at which enemy detects targets

**Creation:** Unity menu `Character/Enemy Combat Base Stats`

### PlayerCharacter

**Type:** MonoBehaviour (inherits `CombatCharacter`)  
**Purpose:** Player-specific combat behavior

**Behavior:**
- Initializes `CurrentHealth` and `_currentTime` in `Start`
- Overrides `OnCombatTick` for player-specific combat logic (currently similar to base)

### EnemyCharacter

**Type:** MonoBehaviour (inherits `CombatCharacter`)  
**Purpose:** Enemy-specific combat and movement behavior

**Behavior:**
- Exposes `BaseStats` as `EnemyCombatBaseStats`
- Initializes health and attack timer in `Start`
- Draws gizmos for attack and vision ranges in the editor
- Moves toward target when out of attack range using `MovementSpeed`

## How It Works

### Combat Tick System

1. **CombatManager** counts `FixedUpdate` calls using `_tickInterval`
2. When interval is reached, fires `CombatTickHandler` with:
   - `previousTickTime` - Last tick timestamp
   - `currentTickTime` - Current tick timestamp
   - `delta` - Time difference between ticks
3. All `CombatCharacter` instances receive the tick event

### Attack Flow

```
1. CombatCharacter receives tick event
   |
2. Accumulate delta: _currentTime += delta
   |
3. Check attack conditions:
   - _currentTime > BaseStats.AttackSpeed?
   - Target exists?
   - Target in range (AttackRange)?
   |
4. If all true: Perform Attack
   |
5. Fire OnPerformCombatHandler(source, target)
   |
6. CombatManager receives attack event
   |
7. Apply damage: target.CurrentHealth -= source.BaseStats.AttackDamage
   |
8. Reset attack timer: _currentTime = 0
```

### Health Changes

When `CurrentHealth` changes:
1. Create `HealthChangeEventArgs` with `OldHealth`, `NewHealth`, `MaxHealth`
2. Fire `OnHealthChangeHandler` instance event
3. Subscribers (UI, death handlers, etc.) react to the change
4. If health reaches zero, call `OnHealthReachZero()` which clears the target

## Integration Points

### Entity Management

- Characters register with `PlayerEntitiesManager`
- Entity queries help find targets and closest enemies
- See [Entity Management Architecture](entity-management.md)

### Input System

- Player attacks can be triggered by input
- Input system sets combat targets
- See [Input System Architecture](input-system.md)

### Animation System

- Health changes can trigger hit/death animations
- Attack events can trigger attack animations
- See [Animation System Architecture](animation-system.md)

## Utilities

### EntityExtensions

Helper extension methods for combat calculations:

- `IsInRange<T>(this T source, T target, float range)` - Returns true if entities are within range (uses squared magnitude for performance)
- `ClosestTo<T>(IEnumerable<T> entities, Vector3 position)` - Finds closest entity to a position

## Usage Guide

### 1. Setup CombatManager

Add `CombatManager` component to a persistent GameObject in your scene (e.g., `GameManager`):

```csharp
// Configure tick interval in Inspector
// _tickInterval = 5 means combat ticks every 5 FixedUpdate calls
```

### 2. Create Combat Stats

Right-click in Project window:
- `Create > Character > Combat Base Stats` for player stats
- `Create > Character > Enemy Combat Base Stats` for enemy stats

Configure the stats:
```
Health: 100
AttackDamage: 10
AttackSpeed: 1.5
AttackRange: 2.0
MovementSpeed: 3.0
```

### 3. Assign Stats to Characters

In your character prefabs:
- Assign the appropriate `BaseStats` ScriptableObject
- Health is initialized from `BaseStats.Health` on Start

### 4. Set Combat Targets

```csharp
playerCharacter.SetTarget(enemyCharacter);
enemyCharacter.SetTarget(playerCharacter);
```

### 5. Subscribe to Events

For UI updates:
```csharp
character.OnHealthChangeHandler += (args) => {
    healthBar.UpdateHealth(args.NewHealth, args.MaxHealth);
};
```

For death handling:
```csharp
character.OnHealthChangeHandler += (args) => {
    if (args.NewHealth <= 0) {
        // Trigger death animation
        // Remove character
    }
};
```

## Known Limitations

### No Death Handling

`OnHealthReachZero()` only clears the target. You need to implement:
- Death animations
- Character removal from scene
- Loot drops
- Respawn logic

### Simple Damage Application

Damage is immediate subtraction with no:
- Armor/resistance calculations
- Critical hits
- Damage types
- Damage over time effects

### Fixed Tick Dependency

Combat timing is tied to `FixedUpdate` frequency:
- May not be ideal for deterministic multiplayer
- Consider decoupling for server authority

### No Combat UI Integration

System fires events but doesn't include:
- Damage numbers
- Hit effects
- Sound effects
- Screen shake

## TODOs / Next Steps

- [ ] Add death handling (animations, removal, respawn)
- [ ] Implement hit/impact animations and effects
- [ ] Add damage calculation system (armor, crits, types)
- [ ] Create combat UI (damage numbers, health bars)
- [ ] Add sound effects for hits and attacks
- [ ] Consider attack queuing and combo systems
- [ ] Decouple tick system for multiplayer support
- [ ] Add projectile support for ranged attacks

## File Reference

- `Assets/Scripts/Character/Combat/CombatManager.cs`
- `Assets/Scripts/Character/CombatCharacter.cs`
- `Assets/Scripts/Character/Combat/CombatBaseStats.cs`
- `Assets/Scripts/Character/Combat/EnemyCombatBaseStats.cs`
- `Assets/Scripts/Character/Player/PlayerCharacter.cs`
- `Assets/Scripts/Character/Enemy/EnemyCharacter.cs`
- `Assets/Scripts/Character/Extensions/EntityExtensions.cs`

## Related Documentation

- [Entity Management Architecture](entity-management.md)
- [Combat Flow Feature](../features/combat-flow.md)
- [Architecture Overview](README.md)
