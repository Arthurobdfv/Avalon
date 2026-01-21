# Enemy Behavior Manager

- Status: Implemented (basic manager with multi-map support)
- Summary: Centralized manager for enemy behaviors. Hooks into the combat tick system to refresh AI queries periodically and assigns targets to aggressive enemies across all available maps.

## Current implementation (what's in the repo)

- Location: `Assets/Scripts/Character/EntityManagers/EnemyBehaviourManager.cs`
- Behavior:
  - On `OnEnable()`, subscribes to `CombatManager.BeforeCombatTickHandler` to trigger behavior refresh each combat tick.
  - Iterates over all maps in `MapManager.AvailableMaps` and processes enemies on each map.
  - For each enemy on each managed map, `HandleEnemyBehavior` executes simple logic:
    - If an enemy is aggressive and has no target, search players on the map and assign the closest player within `VisionRange` as the target.
    - Placeholder for combat behavior and for handling already-assigned targets.
  - Static helper `EnemiesOnMap(string map)` queries enemies by map using `MapManager.GetEntityByType`.
  - Static helper `FindClosestEnemy(Vector3 position, string mapId, Predicate)` finds the closest enemy to a position with optional filtering.
  - Range checks use `EntityExtensions.IsInRange` for squared-distance calculations.

## Known limitations / TODOs

- Target selection is simplistic: picks the closest player within vision range. Consider more advanced selection (threat, line of sight, group targeting, or delegating to a decision/AI module).
- No handling for cases where a target dies, is removed, or leaves range - add patience timers and re-evaluation logic.
- Consider moving utility helpers (range checks, spatial queries) to a shared `AIUtils` or similar module.
- Consider server-authoritative decision making for multiplayer synchronization: the manager should support running on the server with the client receiving only authoritative NPC states.

## Integration notes

- Player list lookup uses `MapManager.GetEntityByType` and `GlobalEntitiesManager.allEntities` for efficient map-scoped queries.
- `EnemyCharacter.SetTarget` should be robust to nulls and state changes.
- Hooks into `CombatManager.BeforeCombatTickHandler` so enemy targeting refreshes before combat ticks are processed.

## Next steps

- Improve selection logic and consider introducing candidate lists and scoring.
- Add tests or debug visualizations to validate target selection and refresh frequency.
- Add patience timers and target re-evaluation logic when targets die or leave range.

 