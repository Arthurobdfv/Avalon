# Combat Flow Feature

## Status
Implemented (basic)

## Summary
This feature implements the basic combat flow including player attacks, enemy targeting, and damage application. It provides the foundation for MMO combat interactions.

## Overview

The combat flow feature integrates player input, entity management, and combat resolution to create a functional combat system. Players can engage enemies, and enemies can target and attack players.

## Key Features

### Player Combat
- Timed attacks based on `AttackSpeed` stat
- Automatic target acquisition
- Attack animation triggers (integrated with animation system)
- Health tracking with UI updates

### Enemy Combat
- Aggressive behavior targeting nearest players
- Range-based attack initiation
- Movement toward targets when out of range
- Vision and aggro range configuration

### Damage Resolution
- Event-driven damage application via `CombatManager`
- Direct damage calculation (AttackDamage - CurrentHealth)
- Health change notifications for UI updates

## How It Works

### Combat Initialization

1. **Setup:** Add `CombatManager` component to a persistent GameObject in the scene
2. **Stats:** Create and assign `CombatBaseStats` or `EnemyCombatBaseStats` assets to characters
3. **Targets:** Set targets for characters at runtime via `SetTarget(CombatCharacter target)`

### Attack Flow

**Player Attack:**
```
Player Input
    |
    v
PlayerInputHandler (detects interact/attack)
    |
    v
PlayerCharacter (checks attack timer)
    |
    v
CombatCharacter.OnPerformCombatHandler (event)
    |
    v
CombatManager.OnPerformCombat (applies damage)
    |
    v
Target.CurrentHealth reduced
    |
    v
OnHealthChangeHandler (UI update)
```

**Enemy Attack:**
```
EnemyBehaviourManager (tick-based refresh)
    |
    v
Find nearest player within vision range
    |
    v
Set as target
    |
    v
Move toward target if out of attack range
    |
    v
CombatCharacter (attack when in range & timer ready)
    |
    v
Damage application (same as player)
```

### Targeting System

The combat flow uses the entity management system for targeting:

- **Players:** Use `PlayerEntitiesManager` to find nearest enemy on the same map
- **Enemies:** `EnemyBehaviourManager` assigns closest player within vision range
- **Combat:** `CombatCharacter` attacks assigned target when in range

## Event-Driven Design

Combat uses events to decouple attack initiation from damage resolution:

**Benefits:**
- Attack logic (timing, range checks) separate from damage calculation
- Easy to add effects, animations, network sync at damage points
- Multiple systems can listen to combat events (UI, audio, VFX)

**Events:**
- `CombatCharacter.OnPerformCombatHandler` - Attack initiated
- `CombatCharacter.OnHealthChangeHandler` - Health changed

## Integration Points

### Entity Management
- Uses `GlobalEntitiesManager` for entity tracking
- Uses `PlayerEntitiesManager` for player queries
- Uses `EntityExtensions` for range checks and spatial queries

### Input System
- Player attacks triggered by input (interact key)
- Attack timing synchronized with fixed update

### Animation System
- Attack events can trigger animation state changes
- Health changes can trigger hit/death animations

### Multiplayer System
- Combat events occur locally (instance-authoritative)
- Future: Server-authoritative combat with input validation

## Usage Example

### Setting Up Combat

1. **Add CombatManager to scene:**
   ```
   GameObject → Add Component → CombatManager
   ```

2. **Create stats assets:**
   ```
   Create → Character → Combat Base Stats (for players)
   Create → Character → Enemy Combat Base Stats (for enemies)
   ```

3. **Assign to prefabs:**
   ```
   Player Prefab → CombatCharacter component → BaseStats
   Enemy Prefab → EnemyCharacter component → BaseStats
   ```

4. **Set targets at runtime:**
   ```csharp
   // Automatic for enemies (via EnemyBehaviourManager)
   // Manual or via interaction for players
   playerCharacter.SetTarget(enemy);
   ```

### Monitoring Combat

Subscribe to health change events:
```csharp
combatCharacter.OnHealthChangeHandler += (args) => {
    Debug.Log($"Health: {args.NewHealth}/{args.MaxHealth}");
    UpdateHealthBar(args.NewHealth, args.MaxHealth);
};
```

## Configuration

### Combat Stats (ScriptableObject)

**CombatBaseStats:**
- `Health` - Maximum health points
- `AttackDamage` - Damage per attack
- `AttackSpeed` - Time between attacks (seconds)
- `AttackRange` - Maximum attack distance
- `MovementSpeed` - Character movement speed

**EnemyCombatBaseStats (extends CombatBaseStats):**
- `Aggressive` - Whether enemy auto-targets players
- `VisionRange` - Distance to detect players

### Combat Manager

- `_tickInterval` - Number of FixedUpdate calls between combat ticks
- Affects attack timing granularity

## Current Limitations

### Known Issues
1. No explicit death handling beyond clearing target
2. Attack resolution is immediate (no projectiles, hit detection)
3. No combat UI or sound behavior wired
4. Tick system tied to FixedUpdate frequency
5. Not server-authoritative (multiplayer limitation)

### Multiplayer Limitations
- Combat is local/instance-authoritative
- `PlayersInputManager` server-side handler unimplemented
- No client prediction or interpolation
- Combat state not synchronized to observers

## Testing Notes

### Local Testing
1. Ensure `CombatManager` in scene
2. Assign stats to player and enemy prefabs
3. Set target for player character
4. Watch console for attack timer logs
5. Verify health changes in UI

### Multiplayer Testing
1. Current combat is local only
2. Each client resolves combat independently
3. No server validation or synchronization
4. Future: Implement server-authoritative combat

## Future Improvements

### Short Term
- Add death handling (animations, removal, respawn)
- Wire combat UI (health bars, damage numbers)
- Add combat audio/VFX
- Implement projectile/ability system

### Long Term
- Server-authoritative combat resolution
- Client prediction and interpolation
- Advanced damage calculation (armor, crits, resistances)
- Combat abilities and skill system
- Combo and timing-based mechanics

## Files of Interest

### Core Combat
- `Assets/Scripts/Character/Combat/CombatManager.cs`
- `Assets/Scripts/Character/CombatCharacter.cs`
- `Assets/Scripts/Character/Combat/CombatBaseStats.cs`
- `Assets/Scripts/Character/Combat/EnemyCombatBaseStats.cs`

### Characters
- `Assets/Scripts/Character/Player/PlayerCharacter.cs`
- `Assets/Scripts/Character/Enemy/EnemyCharacter.cs`
- `Assets/Scripts/Character/Enemy/EnemyBehaviourManager.cs`

### Utilities
- `Assets/Scripts/Character/Extensions/EntityExtensions.cs`

### UI
- `Assets/Scripts/Character/UI/HealthBar.cs`

## Related Documentation

- [Combat System Architecture](../architecture/combat-system.md) - Technical architecture details
- [Entity Management](../architecture/entity-management.md) - Entity tracking system
- [Enemy Behavior](enemy-behavior.md) - Enemy AI and targeting
- [Multiplayer Architecture](../architecture/multiplayer-architecture.md) - Network integration

## Changelog

See root `CHANGELOG.md` under "Unreleased" for recent updates to combat flow.
