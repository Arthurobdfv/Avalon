# Changelog

All notable changes to this repository will be documented in this file.

## Unreleased

### Features & Updates

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
  - Added documentation for the multiplayer handler and observer architecture (`docs/MultiplayerArchitecture.md`).
  - Expanded combat system docs with tick hook order, multiplayer input flow, and server-authoritative limitations (`docs/CombatSystem.md`).
  - Documented manager responsibilities and per-map entity snapshots in `docs/feature-basic-entity-manager-and-combat.md` and updated automation notes in `.github/copilot-instructions.md`.
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

