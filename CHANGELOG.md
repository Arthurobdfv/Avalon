# Changelog

All notable changes to this repository will be documented in this file.

## Unreleased

### Features & Updates

- Player input handling refactored into client/server split with proper authority separation
- `PlayerInputHandler` (client): builds `PlayerInputState` from raw input, sends to server. Does NOT mutate entity state.
- `PlayersInputManager` (server): receives and aggregates input per client (one per cycle), processes movement/interaction, mutates entity state.
- `EntitySpawner` (client): receives `EntitySpawnPacket` from server, applies authoritative entity state (position, direction, movement).
- `DirectionEnumHelper`: shared static utility for Vector2 to DirectionEnum conversion.
- Updated documentation across `docs/architecture/multiplayer-architecture.md`, `docs/architecture/combat-system.md`, `docs/architecture/entity-management.md`, and `docs/features/character-movement.md`.

- Player UI and equipment sync
  - Added `PlayerUI` script and associated player UI prefab assets (in-scene wiring and data placeholders).
  - Added `PlayerEquipmentPacket` to support equipment synchronization across clients/servers.

- Cleanup
  - Removed `ClientEntityStateHandler` (client-side entity state handler was deleted as part of the input/authority refactor).

- TCP Relay Server (new separate project - outside Unity project)
  - Created `../AvalonRelayServer/` - .NET 8 TCP relay server for NAT traversal
  - Room-based packet routing with game server designation
  - Flexible targeting (broadcast, game server only, specific clients)
  - Heartbeat/keepalive with automatic timeout
  - Length-prefixed JSON packet serialization
  - Docker support for cloud deployment
  - `AvalonShared` library targets netstandard2.1 for Unity compatibility

### Documentation

- **Documentation accuracy review:** Fixed discrepancies between documentation and actual code:
  - Fixed `PlayersInputManager` file path in `docs/architecture/input-system.md` (was `Assets/Scripts/Input/PlayersInputManager.cs`, corrected to `Assets/Scripts/Multiplayer/Server/PlayersInputManager.cs`)
  - Fixed `PlayerInputMapper` file path (was `Assets/Scripts/Input/PlayerInputMapper.cs`, corrected to `Assets/Scripts/Input/PlayerInput/PlayerInputMapper.cs`)
  - Fixed `PlayerEntitiesManager` file path in `docs/architecture/entity-management.md` (was `Assets/Scripts/Character/Player/PlayerEntitiesManager.cs`, corrected to `Assets/Scripts/Character/EntityManagers/PlayerEntitiesManager.cs`)
  - Fixed `EnemyBehaviourManager` file path in `docs/features/enemy-behavior.md` (was `Assets/Scripts/Character/Enemy/EnemyBehaviourManager.cs`, corrected to `Assets/Scripts/Character/EntityManagers/EnemyBehaviourManager.cs`)
  - Updated `PlayersInputManager` status from "not implemented" to "implemented" across all docs
  - Updated `EnemyBehaviourManager` docs to reflect multi-map support and combat tick hook integration
  - Corrected old file path references in `CHANGELOG.md` and `.github/copilot-instructions.md` to use new kebab-case structure

- Added `docs/PacketsAndHandlers.md`: comprehensive reference of all packets, handlers, and transport interfaces with Mermaid diagrams.
- Added Mermaid diagrams to feature documentation:
- `docs/architecture/multiplayer-architecture.md`: architecture overview and player input flow sequence diagram.
- `docs/architecture/combat-system.md`: class diagram and combat tick flow sequence diagram.
- `docs/architecture/entity-management.md`: entity management architecture and attack flow diagrams.
- `docs/features/character-movement.md`: input flow sequence diagram and architecture graph.

- SpriteSheet ? Animations
  - Moved `CharacterName` field into `AnimationSetDefinition` for better model locality and usability.
  - Cleanup and fixes in the sprite-sheet parsing pipeline (`SpriteSheetParser` and editor integration).
  - Prototype implementation for generating animator transitions and wiring a generated `AnimatorController` with project-specific conventions (Avalon). This is a temporary approach and will be made configurable in a follow-up.
  - Updated importer defaults in `SpriteSheetParser`: `textureCompression = Uncompressed`, `spriteImportMode = Multiple`, `filterMode = Trilinear`, `textureType = Sprite`, `spritePixelsPerUnit = 50`.

- Enemy behavior
  - Added/updated `EnemyBehaviourManager` to centralize enemy AI behavior refresh and basic target selection:
    - Tick-based refresh (`TicksPerRefreshQuery`) to limit CPU usage.
    - Basic aggressive targeting: selects closest player within a given range and assigns as target.
    - Utility range-check helper (`IsInRange`).
    - Notes and TODOs left in code for multi-map support, improved selection logic and target handling.

- Misc
  - Various small fixes and adjustments across editor scripts and runtime handlers to integrate the above changes.
  - Added documentation for the multiplayer handler and observer architecture (`docs/architecture/multiplayer-architecture.md`).
  - Expanded combat system docs with tick hook order, multiplayer input flow, and server-authoritative limitations (`docs/architecture/combat-system.md`).
  - Documented manager responsibilities and per-map entity snapshots in `docs/architecture/entity-management.md` and updated automation notes in `.github/copilot-instructions.md`.
  - Added the full set of TextMesh Pro default resources (fonts, materials, shaders, emoji sprites) to version control so text rendering works consistently across environments.

### Documentation

- **Major documentation restructure (January 2024):**
  - Reorganized all documentation into clear structure: `docs/architecture/`, `docs/features/`, `docs/contributing/`
  - Created comprehensive documentation hub at `docs/README.md`
  - Migrated and consolidated scattered documentation into organized structure
  - Added new architecture docs: `input-system.md`, `animation-system.md`, `entity-management.md`
  - Consolidated contributing guides: `documentation-guide.md`, `code-style.md`
  - Created feature documentation: `combat-flow.md` and migrated existing feature docs
  - Updated root `README.md` with links to new documentation structure
  - Updated `.github/copilot-instructions.md` with new documentation workflow
  - All documentation now uses kebab-case naming convention
- Updated `FEATURES/SpriteSheet-to-Animations.md` to reflect the current implementation, status, and remaining work.
- Added this `CHANGELOG.md` to track Unreleased changes and provide a single place for release notes.

## Notes / Next steps

- Generalize animator transition generation and expose transition rules via an importer UI or external mapping files (JSON).
- Implement automatic `AnimationClip` generation from sliced sprites and name-to-frame-range mapping.
- Add a preview window and idempotent re-import strategy for generated assets.
- Improve enemy targeting logic, multi-map support, and integrate with any server-authoritative decision modules for multiplayer contexts.

