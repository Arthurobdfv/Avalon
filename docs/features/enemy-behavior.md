# Enemy Behavior Manager

- Status: In Progress (basic manager implemented)
- Summary: Centralized manager for enemy behaviors. Current implementation provides a ticked refresh loop that runs AI queries periodically and assigns targets to aggressive enemies.

## Current implementation (what's in the repo)

- Location: `Assets/Scripts/Character/Enemy/EnemyBehaviourManager.cs`
- Behavior:
  - On `Start()`, discovers all `EnemyCharacter` instances and registers them under a default map (`Constants.InitialMap`).
  - Uses a tick counter (`TicksPerRefreshQuery`) to limit how often behavior queries run (to reduce per-frame cost).
  - For each enemy on each managed map, `HandleEnemyBehavior` executes simple logic:
    - If an enemy is aggressive and has no target, search players on the map and assign the closest player within `Behavior.Range` as the target.
    - Placeholder for combat behavior and for handling already-assigned targets.
  - Utility method `IsInRange` performs squared-distance range checks.

## Known limitations / TODOs

- Currently only supports a single map (`Constants.InitialMap`). Multi-map support required.
- Target selection is simplistic: picks the closest player within range. Consider more advanced selection (threat, line of sight, group targeting, or delegating to a decision/AI module).
- No handling for cases where a target dies, is removed, or leaves range — add patience timers and re-evaluation logic.
- Consider moving utility helpers (range checks, spatial queries) to a shared `AIUtils` or similar module.
- Consider server-authoritative decision making for multiplayer synchronization: the manager should support running on the server with the client receiving only authoritative NPC states.

## Integration notes

- Player list lookup uses `PlayerEntitiesManager.PlayersOnMap(mapName)` — ensure this API is available and performant.
- `EnemyCharacter.SetTarget` should be robust to nulls and state changes.

## Next steps

- Add multi-map registration and lifecycle management for enemies (spawn/despawn).
- Improve selection logic and consider introducing candidate lists and scoring.
- Add tests or debug visualizations to validate target selection and refresh frequency.

 