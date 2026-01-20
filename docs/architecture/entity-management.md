# Entity Management Architecture

## Overview

A centralized entity management system that handles character registration, lookup, and common entity operations. This system provides a foundation for managing player and enemy entities in the game world.

## Purpose

- Centralize entity registration and tracking
- Provide query and lookup capabilities for entities
- Offer common entity operations through extension methods
- Support future multiplayer entity synchronization

## Core Components

### PlayerEntitiesManager

**Type:** MonoBehaviour  
**Purpose:** Central manager for player entity registration and queries

**Responsibilities:**
- Register and unregister player entities
- Track active player entities in the game world
- Provide query methods for finding players
- Coordinate player-related entity operations

**Usage Pattern:**
```csharp
// Register a player entity
PlayerEntitiesManager.Instance.RegisterPlayer(playerCharacter);

// Query players
var allPlayers = PlayerEntitiesManager.Instance.GetAllPlayers();
var nearbyPlayers = PlayerEntitiesManager.Instance.GetPlayersInRange(position, radius);
```

### EntityExtensions

**Type:** Static utility class  
**Purpose:** Common operations and queries for entities

**Key Methods:**

#### `IsInRange<T>(this T source, T target, float range)`
Checks if two entities are within a specified range.

**Implementation:**
- Uses squared magnitude distance check for performance
- Returns `true` if distance <= range
- Generic to work with any entity type

**Example:**
```csharp
if (player.IsInRange(enemy, attackRange)) {
    // Perform attack
}
```

#### `ClosestTo<T>(IEnumerable<T> entities, Vector3 position)`
Finds the closest entity to a given position.

**Implementation:**
- Iterates through entities to find minimum distance
- Returns the closest entity or default if collection is empty

**Example:**
```csharp
var closestEnemy = EntityExtensions.ClosestTo(enemies, player.transform.position);
player.SetTarget(closestEnemy);
```

## Character Hierarchy

The entity system uses a hierarchical character structure:

```
Character (base class)
    |
    +-- CombatCharacter
            |
            +-- PlayerCharacter
            |
            +-- EnemyCharacter
```

### Character

**Base class** for all character entities in the game.

**Common Properties:**
- Transform and position
- Character type identification
- Base movement capabilities

### CombatCharacter

**Inherits from Character**  
Adds combat capabilities:
- Health tracking
- Combat stats reference
- Attack timing
- Target management
- Combat event handling

See [Combat System Architecture](combat-system.md) for details.

### PlayerCharacter

**Inherits from CombatCharacter**  
Player-specific behavior:
- Input handling integration
- Player-specific combat logic
- Camera following
- Player state management

### EnemyCharacter

**Inherits from CombatCharacter**  
Enemy-specific behavior:
- AI movement toward targets
- Enemy-specific stats (aggression, vision range)
- Enemy behavior patterns
- Gizmo visualization for ranges

## How It Works

### Entity Registration Flow

```
1. Character spawns in scene
   |
2. Character Start() method calls Register
   |
3. EntityManager adds to internal tracking
   |
4. Character is now queryable by other systems
   |
5. On destroy: Character unregisters itself
```

### Entity Query Flow

```
1. System needs to find entities
   |
2. Calls EntityManager query method
   |
3. EntityManager returns matching entities
   |
4. System processes results
```

### Distance Check Optimization

The `IsInRange` method uses squared magnitude to avoid expensive square root calculations:

```csharp
// Instead of:
Vector3.Distance(source.position, target.position) <= range

// We use:
(source.position - target.position).sqrMagnitude <= range * range
```

This provides better performance for frequent distance checks in combat and AI systems.

## Integration Points

### Combat System

- Combat system queries entities to find targets
- `IsInRange` used for attack range checks
- Entity lookup for targeting and damage application
- See [Combat System Architecture](combat-system.md)

### Input System

- Input system uses entity manager to find player entities
- Player input forwarded to registered player entities
- Entity queries for interaction targeting
- See [Input System Architecture](input-system.md)

### Enemy Behavior

- Enemy AI queries for nearby players
- Uses `ClosestTo` to find optimal targets
- Entity tracking for enemy spawning and despawning
- See [Enemy Behavior Feature](../features/enemy-behavior.md)

## Usage Patterns

### Finding Targets for Combat

```csharp
// Enemy finds nearest player
var players = PlayerEntitiesManager.Instance.GetAllPlayers();
var nearestPlayer = EntityExtensions.ClosestTo(players, enemy.transform.position);
if (nearestPlayer != null) {
    enemy.SetTarget(nearestPlayer);
}
```

### Range-Based Interactions

```csharp
// Check if player is in interaction range
if (player.IsInRange(npc, interactionRange)) {
    ShowInteractionPrompt();
}
```

### Area-of-Effect Queries

```csharp
// Find all entities in explosion radius
var affectedEntities = EntityManager.GetEntitiesInRange(explosionCenter, explosionRadius);
foreach (var entity in affectedEntities) {
    entity.TakeDamage(explosionDamage);
}
```

## Design Decisions

### Why Centralized Management?

**Pros:**
- Single source of truth for entity tracking
- Easier to implement multiplayer synchronization
- Consistent query interface across systems
- Better performance than scene-wide FindObjectsOfType calls

**Cons:**
- Additional registration/unregistration overhead
- Need to maintain registration state
- Potential for registration bugs if not handled properly

### Why Extension Methods?

Extension methods provide:
- Fluent, readable API
- Works on any entity type
- No need to modify base classes
- Easy to discover with IntelliSense

### Generic Constraints

Methods use generic type constraints to ensure type safety while remaining flexible:

```csharp
public static bool IsInRange<T>(this T source, T target, float range) 
    where T : Component
```

## Known Limitations

### Manual Registration Required

Characters must remember to register/unregister:
- If `OnEnable`/`OnDisable` not properly called, tracking breaks
- No automatic detection of new entities
- Consider implementing automatic registration via interfaces

### No Entity Groups

Currently no support for:
- Team/faction grouping
- Entity tags or categories
- Priority or importance levels

### Single Player Focus

Current implementation assumes single-player:
- No network entity synchronization
- No server-authoritative entity state
- No entity ownership tracking

## Future Enhancements

### Multiplayer Support

Plans for network entity synchronization:
- Server-authoritative entity registration
- Client-side prediction for movement
- Entity ownership and authority
- Network ID assignment

### Spatial Partitioning

For better performance with many entities:
- Grid-based spatial partitioning
- Quadtree or octree structures
- Cached range queries
- Frustum culling integration

### Entity Events

System-wide entity lifecycle events:
- OnEntitySpawned
- OnEntityDestroyed
- OnEntityDamaged
- OnEntityStateChanged

## File Reference

- `Assets/Scripts/Character/Player/PlayerEntitiesManager.cs`
- `Assets/Scripts/Character/Extensions/EntityExtensions.cs`
- `Assets/Scripts/Character/Character.cs` (base class)
- `Assets/Scripts/Character/CombatCharacter.cs`
- `Assets/Scripts/Character/Player/PlayerCharacter.cs`
- `Assets/Scripts/Character/Enemy/EnemyCharacter.cs`

## Related Documentation

- [Combat System Architecture](combat-system.md)
- [Input System Architecture](input-system.md)
- [Combat Flow Feature](../features/combat-flow.md)
- [Architecture Overview](README.md)
