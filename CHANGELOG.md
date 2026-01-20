# Changelog

All notable changes to this repository will be documented in this file.

## Unreleased

### Documentation Restructuring

- **Complete documentation overhaul** - Reorganized all documentation under `docs/` with clear structure
  - Created `docs/README.md` as central documentation hub
  - Organized docs into `architecture/`, `features/`, `contributing/`, and `changelog/` folders
  - All documentation files now use kebab-case naming convention
  
- **Architecture Documentation** - New technical system documentation
  - `docs/architecture/README.md` - High-level architecture overview
  - `docs/architecture/combat-system.md` - Combat mechanics (migrated from `docs/CombatSystem.md`)
  - `docs/architecture/entity-management.md` - Entity registration and queries
  - `docs/architecture/input-system.md` - Input handling and direction mapping
  - `docs/architecture/animation-system.md` - Animation coordination

- **Feature Documentation** - Consolidated user-facing feature docs
  - `docs/features/README.md` - Feature index with status tracking
  - `docs/features/character-animation.md` - Character animations (migrated from `FEATURES/`)
  - `docs/features/character-movement.md` - Player movement (migrated from `FEATURES/`)
  - `docs/features/sprite-pipeline.md` - Sprite-to-animation automation (migrated from `FEATURES/`)
  - `docs/features/combat-flow.md` - Combat interactions (migrated from `docs/`)
  - `docs/features/enemy-behavior.md` - Enemy AI (migrated from `FEATURES/`)
  - `docs/features/build-pipeline.md` - CI/CD plans

- **Contributing Documentation** - New contribution guides
  - `docs/contributing/README.md` - How to contribute overview
  - `docs/contributing/documentation-guide.md` - Documentation standards and templates
  - `docs/contributing/code-style.md` - Coding conventions and best practices

- **Updated Files**
  - `README.md` - Updated to point to new documentation structure
  - `.github/copilot-instructions.md` - Consolidated and updated with new doc structure
  
- **Removed Files**
  - `FEATURES.md` - Replaced by `docs/features/README.md`
  - `DOCUMENTATION.md` - Replaced by `docs/README.md`
  - `FEATURES/` folder - Content migrated to `docs/features/`
  - `docs/CombatSystem.md` - Migrated to `docs/architecture/combat-system.md`
  - `docs/feature-basic-entity-manager-and-combat.md` - Migrated to `docs/features/combat-flow.md`
  - `docs/COPILOT_DOCS_DOCUMENTING.md` - Consolidated into `docs/contributing/documentation-guide.md`
  - `docs/copilot-doc-update-pattern.md` - Consolidated into `docs/contributing/documentation-guide.md`
  - `.github/COPILOT_INSTRUCTIONS.md` - Consolidated into `.github/copilot-instructions.md`

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

### Documentation

- Updated `FEATURES/SpriteSheet-to-Animations.md` to reflect the current implementation, status, and remaining work.
- Added this `CHANGELOG.md` to track Unreleased changes and provide a single place for release notes.

## Notes / Next steps

- Generalize animator transition generation and expose transition rules via an importer UI or external mapping files (JSON).
- Implement automatic `AnimationClip` generation from sliced sprites and name-to-frame-range mapping.
- Add a preview window and idempotent re-import strategy for generated assets.
- Improve enemy targeting logic, multi-map support, and integrate with any server-authoritative decision modules for multiplayer contexts.

