# Enemy Behavior

- **Status:** Implemented (basic)
- **Summary:** Centralized enemy behavior management system that handles AI target selection, aggressive enemy behavior, and periodic behavior updates through a tick-based system.

## Overview

This feature provides basic AI for enemy characters through a centralized behavior manager. The system periodically evaluates enemy states and assigns targets based on aggression settings, range, and player proximity.

The behavior manager uses a tick-based approach to reduce per-frame AI costs, making it scalable for multiple enemies in the scene.

## User Experience

Players encounter:
- **Aggressive enemies** - Automatically detect and pursue nearby players
- **Passive enemies** - Remain idle unless attacked
- **Range-based detection** - Enemies only engage within their vision range
- **Automatic targeting** - Enemies select closest valid targets

## Implementation

### Key Components

**EnemyBehaviorManager:**
- Central coordinator for all enemy AI
- Tick-based update system to reduce CPU cost
- Per-map enemy registration and tracking
- Target assignment for aggressive enemies

**EnemyCharacter:**
- Represents individual enemy entities
- References `EnemyCombatBaseStats` for behavior properties
- Exposes aggression and vision range settings
- Receives target assignments from manager

**EnemyCombatBaseStats:**
- Extends `CombatBaseStats` with enemy-specific properties:
  - `Aggressive` - Whether enemy actively seeks targets
  - `VisionRange` - Distance at which enemy detects players

### Behavior Flow

```
1. EnemyBehaviorManager.Start()
   - Discover all EnemyCharacter instances
   - Register to map (currently Constants.InitialMap)
   |
2. Each fixed update (tick-based):
   - Increment tick counter
   - If counter >= TicksPerRefreshQuery: Execute AI
   |
3. For each registered enemy:
   - Check if aggressive AND no current target
   - If true: Query players on same map
   - Find closest player within VisionRange
   - Assign as target via SetTarget()
   |
4. EnemyCharacter receives target
   - Begin moving toward target
   - Engage in combat when in AttackRange
```

### Tick-Based Updates

To optimize performance with many enemies:

```csharp
// In EnemyBehaviorManager
[SerializeField] private int TicksPerRefreshQuery = 10;
private int _tickCounter = 0;

void FixedUpdate() {
    _tickCounter++;
    if (_tickCounter >= TicksPerRefreshQuery) {
        _tickCounter = 0;
        RefreshBehaviors();
    }
}
```

**Benefits:**
- Reduces AI cost from every frame to every N frames
- Configurable refresh rate per gameplay needs
- Scalable to larger numbers of enemies

### Target Selection Logic

Current implementation uses simple closest-player selection:

```csharp
void HandleEnemyBehavior(EnemyCharacter enemy) {
    if (!enemy.BaseStats.Aggressive) return;
    if (enemy.Target != null) return; // Already has target
    
    var players = PlayerEntitiesManager.PlayersOnMap(mapName);
    foreach (var player in players) {
        if (IsInRange(enemy, player, enemy.BaseStats.VisionRange)) {
            enemy.SetTarget(player);
            break; // Assign closest player
        }
    }
}
```

### Range Checking

Uses optimized squared-distance checks:

```csharp
bool IsInRange(EnemyCharacter enemy, PlayerCharacter player, float range) {
    float sqrDistance = (enemy.transform.position - player.transform.position).sqrMagnitude;
    return sqrDistance <= (range * range);
}
```

## Architecture

The enemy behavior system integrates with:
- **Entity Management** - Queries for players on map
- **Combat System** - Sets targets for combat engagement
- **Map System** - Multi-map support (partially implemented)

See [Entity Management Architecture](../architecture/entity-management.md) for entity queries.

## Usage

### Setting Up Enemy Behavior

1. **Add EnemyBehaviorManager to scene:**
```csharp
// Attach to persistent GameObject
GameObject manager = new GameObject("EnemyBehaviorManager");
manager.AddComponent<EnemyBehaviorManager>();
```

2. **Create enemy stats:**
   - Right-click in Project window
   - Create > Character > Enemy Combat Base Stats
   - Configure properties:
     ```
     Health: 50
     AttackDamage: 5
     AttackSpeed: 2.0
     AttackRange: 1.5
     MovementSpeed: 2.0
     Aggressive: true
     VisionRange: 10.0
     ```

3. **Create enemy prefab:**
   - Add EnemyCharacter component
   - Assign EnemyCombatBaseStats
   - Configure visuals and animations

4. **Place enemies in scene:**
   - Drag enemy prefab into scene
   - On Start, enemies auto-register with manager
   - Aggressive enemies will detect and pursue players

### For Players

- Aggressive enemies will detect you when you enter their vision range
- Stay outside vision range to avoid detection
- Passive enemies won't attack unless provoked

### For Developers

**Creating custom enemy behaviors:**

Extend or modify `HandleEnemyBehavior` for custom AI:

```csharp
void HandleEnemyBehavior(EnemyCharacter enemy) {
    if (enemy.BaseStats.Aggressive) {
        // Custom target selection
        var bestTarget = FindBestTarget(enemy);
        if (bestTarget != null) {
            enemy.SetTarget(bestTarget);
        }
    } else {
        // Passive behavior
        if (enemy.CurrentHealth < enemy.BaseStats.Health * 0.5f) {
            enemy.Flee(); // Custom flee behavior
        }
    }
}
```

**Adjusting AI refresh rate:**

Configure in Inspector:
- Lower `TicksPerRefreshQuery` for more responsive AI (higher CPU cost)
- Higher values for better performance (less responsive AI)
- Typical range: 5-20 ticks

## Known Limitations

### Single Map Support

Currently only supports `Constants.InitialMap`:
- All enemies registered to one map
- No multi-map or multi-scene support
- Can't have different enemy groups per area

**TODO:** Add multi-map registration and lifecycle management

### Simplistic Target Selection

Basic closest-player logic:
- No threat or aggro system
- No line-of-sight checks
- No group coordination
- Doesn't consider player level or health

**TODO:** Implement more advanced selection (threat, LOS, group targeting)

### No Target Re-evaluation

Once target is assigned, it's never re-evaluated:
- Doesn't handle target death
- Doesn't handle target out of range
- No patience timer for unreachable targets
- No switching to better targets

**TODO:** Add patience timers and re-evaluation logic

### No Behavior States

Missing common enemy AI states:
- No patrol behavior
- No flee when low health
- No call for help
- No return to spawn point

### Performance Considerations

With many enemies:
- All enemies on all maps processed each refresh
- No spatial partitioning
- No frustum culling for offscreen enemies

**Future:** Consider spatial partitioning or active/inactive enemy pools

## Next Steps

### Core Behavior

- [ ] Multi-map support with per-map enemy registration
- [ ] Target re-evaluation (death, range, better target)
- [ ] Patience timers for unreachable targets
- [ ] Line-of-sight checks for detection
- [ ] Aggro and threat system

### Advanced AI

- [ ] Patrol behavior for non-combat state
- [ ] Flee behavior when low health
- [ ] Call for help to nearby enemies
- [ ] Return to spawn point after combat
- [ ] Group coordination (packs, formations)

### Optimization

- [ ] Spatial partitioning for large enemy counts
- [ ] Active/inactive enemy pools
- [ ] Frustum culling for offscreen enemies
- [ ] Async/job system for AI calculations

### Multiplayer

- [ ] Server-authoritative AI decisions
- [ ] Network synchronization of enemy state
- [ ] Client-side prediction for movement
- [ ] Bandwidth optimization for many enemies

## Related Documentation

- [Combat Flow](combat-flow.md) - How enemies engage in combat
- [Combat System Architecture](../architecture/combat-system.md) - Combat mechanics
- [Entity Management Architecture](../architecture/entity-management.md) - Entity queries
- [Features Index](README.md) - All features
