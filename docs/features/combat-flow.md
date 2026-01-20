# Combat Flow

- **Status:** Implemented (basic)
- **Summary:** Basic entity management system with event-driven combat flow that handles player and enemy interactions through timed attacks and damage application.

## Overview

This feature introduces a minimal but functional combat system that brings together entity management, input handling, and timed combat mechanics. It provides the foundation for player-enemy interactions and combat gameplay.

The system uses an event-driven architecture to separate attack initiation from damage resolution, making it easier to extend and eventually support multiplayer gameplay.

## User Experience

Players experience:
- **Combat targeting** - Select enemies to attack
- **Automatic combat** - Once targeted, characters automatically attack when in range
- **Visual feedback** - Health bars update as damage is dealt
- **Death handling** - Characters can defeat enemies (basic implementation)

## Implementation

### Key Systems

**Entity Management:**
- `PlayerEntitiesManager` - Centralized entity registration and lookup
- `EntityExtensions` - Helper methods for entity operations (range checks, closest entity)
- Provides query capabilities for finding targets

**Combat System:**
- `CombatCharacter` - Base class for combat-capable entities
- `CombatBaseStats` - ScriptableObject for character stats
- `CombatManager` - Event-driven damage resolution coordinator
- Attack timing based on tick system
- Health tracking with change events

**Player and Enemy:**
- `PlayerCharacter` - Player-specific combat behavior
- `EnemyCharacter` - Enemy-specific combat and AI
- Both inherit from `CombatCharacter`

### Combat Flow

```
1. Player/Enemy selects target
   |
2. Combat tick fires from CombatManager
   |
3. CombatCharacter accumulates attack timer
   |
4. When timer > AttackSpeed AND in range:
   - Fire OnPerformCombatHandler event
   - Reset timer
   |
5. CombatManager receives event
   |
6. Apply damage: target.CurrentHealth -= damage
   |
7. Target fires OnHealthChangeHandler
   |
8. UI/Systems react to health change
```

### Event-Driven Architecture

Combat uses events to decouple components:

**Attack Events:**
```csharp
// CombatCharacter fires when attacking
public static OnPerformCombat OnPerformCombatHandler;

// CombatManager subscribes and resolves damage
void OnPerformCombat(PerformCombatEventArgs args) {
    args.TargetCharacter.CurrentHealth -= args.SourceCharacter.BaseStats.AttackDamage;
}
```

**Health Events:**
```csharp
// CombatCharacter fires when health changes
public OnHealthChange OnHealthChangeHandler;

// UI subscribes for updates
character.OnHealthChangeHandler += (args) => {
    healthBar.UpdateHealth(args.NewHealth, args.MaxHealth);
};
```

This separation allows:
- Different damage calculation systems
- Hit animations and effects
- Network synchronization (future)
- Flexible combat rules

### Integration

The combat flow integrates:
- **Entity Management** - Target finding and entity queries
- **Input System** - Player can trigger target selection
- **Animation System** - Attack events can trigger animations
- **UI System** - Health bars react to health changes

## Architecture

See related architecture documentation:
- [Combat System Architecture](../architecture/combat-system.md) - Combat tick, damage resolution
- [Entity Management Architecture](../architecture/entity-management.md) - Entity registration and queries

## Usage

### Setting Up Combat

1. **Add CombatManager to scene:**
```csharp
// Attach to persistent GameObject
GameObject gameManager = new GameObject("GameManager");
gameManager.AddComponent<CombatManager>();
```

2. **Create combat stats:**
   - Right-click in Project window
   - Create > Character > Combat Base Stats
   - Configure health, damage, speed, range

3. **Assign to characters:**
   - Assign BaseStats to PlayerCharacter prefab
   - Assign BaseStats to EnemyCharacter prefab

4. **Set targets:**
```csharp
// Player targets enemy
playerCharacter.SetTarget(enemyCharacter);

// Enemy targets player
enemyCharacter.SetTarget(playerCharacter);
```

### For Players

1. Click on an enemy to target it (if targeting UI is implemented)
2. Character automatically attacks when in range
3. Watch health bars to see combat progress
4. Defeat enemies to clear the area

### For Developers

**Subscribing to combat events:**

```csharp
// React to attacks
CombatCharacter.OnPerformCombatHandler += (args) => {
    Debug.Log($"{args.SourceCharacter.name} attacked {args.TargetCharacter.name}");
    PlayHitAnimation(args.TargetCharacter);
};

// React to health changes
character.OnHealthChangeHandler += (args) => {
    if (args.NewHealth <= 0) {
        PlayDeathAnimation(character);
    }
};
```

**Creating custom damage calculations:**

```csharp
// Override damage application in CombatManager
void OnPerformCombat(PerformCombatEventArgs args) {
    int damage = args.SourceCharacter.BaseStats.AttackDamage;
    
    // Add armor calculation
    int armor = args.TargetCharacter.BaseStats.Armor;
    int finalDamage = Mathf.Max(1, damage - armor);
    
    // Apply critical hits
    if (Random.value < critChance) {
        finalDamage *= 2;
    }
    
    args.TargetCharacter.CurrentHealth -= finalDamage;
}
```

## Known Limitations

### Basic Damage Application

Current damage is simple subtraction:
- No armor or resistance calculations
- No damage types (physical, magical, etc.)
- No critical hits or variance
- No damage over time effects

### Minimal Death Handling

`OnHealthReachZero()` only clears the target:
- No death animations
- No corpse or loot handling
- No removal from scene
- No respawn mechanics

**TODO:** Implement proper death handling

### Placeholder PerformAttack Method

`CombatManager.PerformAttack()` exists but isn't used:
- Damage is handled via events
- Method could be used for programmatic attacks
- Consider implementing for AI or skill systems

### No Combat UI

Missing combat feedback:
- No damage numbers
- No hit effects or particles
- No sound effects
- No screen shake or impact feedback

## Next Steps

### Core Combat

- [ ] Add armor and resistance calculations
- [ ] Implement critical hits and damage variance
- [ ] Add damage types (physical, magical, true)
- [ ] Support damage over time effects
- [ ] Add healing and regeneration

### Death and Respawn

- [ ] Implement death animations
- [ ] Add corpse handling and cleanup
- [ ] Create loot drop system
- [ ] Implement respawn mechanics
- [ ] Add death penalties (XP loss, etc.)

### Combat Features

- [ ] Add special abilities and skills
- [ ] Implement cooldown system
- [ ] Support combo attacks
- [ ] Add status effects (stun, slow, etc.)
- [ ] Create aggro and threat system

### Multiplayer Preparation

- [ ] Decouple tick system for server authority
- [ ] Add network synchronization for combat events
- [ ] Implement client-side prediction
- [ ] Add lag compensation for hits

## Related Documentation

- [Combat System Architecture](../architecture/combat-system.md) - Technical implementation
- [Entity Management Architecture](../architecture/entity-management.md) - Entity queries and tracking
- [Enemy Behavior](enemy-behavior.md) - Enemy AI and targeting
- [Features Index](README.md) - All features
