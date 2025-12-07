# SpriteSheet to Animations Pipeline

- Status: In Progress
- Summary: Editor tooling/importer that prepares sprite sheet assets into sliced sprites and generated `AnimationClip`s and optional `AnimatorController` states for each character.

## Motivation

Hand-authoring clips for every new character is slow and error-prone. This pipeline automates the import of character sprite sheets (spritemaps), producing consistent clips and animator setups, with configurable mappings for different art packs.

## Scope

- Unity Editor-only workflow to slice textures into `Sprite`s
- Generate `AnimationClip`s for named actions (idle, walk, attack, etc.)
- Optional generation/wiring of an `AnimatorController` and states
- Re-import support when mappings/settings change

Out of scope (for now):
- Runtime loading from external locations
- Complex state machines beyond a basic directional locomotion set

## Requirements / Acceptance Criteria

- Import configuration options:
  - Slice grid size (columns/rows) or explicit frame rectangles
  - Frame order mapping (left-to-right, top-to-bottom, or custom via JSON)
  - Named animation ranges (e.g., Idle: frames 0–3, Walk: frames 4–11)
  - Import settings (Pixels Per Unit, pivot, filter mode)
- Creates `AnimationClip` assets for each named action and optional `AnimatorController` with states and transitions
- Supports input files: single PNG spritemap + optional JSON mapping file (matched by name)
- Safe defaults and re-import flow to overwrite generated clips when mappings change

## Proposed Implementation

- Location: `Assets/Editor/Importers/CharacterSpriteImporter.cs`
- Options storage: `ScriptableObject` or importer serialized fields; optional sidecar JSON per asset
- Workflow:
  1. Add a PNG spritemap and optional JSON mapping (e.g., `hero_spritemap.png`, `hero_spritemap.json`).
  2. Select the texture and configure importer options in the Inspector, or choose a custom "Reimport with Character Importer" menu.
  3. Importer slices texture into `Sprite`s, creates `AnimationClip`s per action, and optionally generates an `AnimatorController` (+ states) and saves assets under a predictable folder structure.
- Directional animations:
  - Support 4/8-direction sets consistent with `DirectionEnum` used at runtime
  - Naming convention examples: `walk_up`, `walk_down`, `walk_left`, `walk_right`, etc.

## Progress update — Step 1: SpriteSheet splitting

- Status: Implemented (initial)
- Relevant paths:
  - `Assets/Scripts/Editor/SpriteSheet2Anim/SpriteSheetParser.cs`
  - `Assets/Scripts/Editor/SpriteSheet2Anim/InspectorCustomEditor/SpriteSheet2AnimCustomEditor.cs`
  - `Assets/Scripts/Editor/SpriteSheet2Anim/Models/`
  - `Assets/Sprites/Characters/` (updated `.png.meta` files reflect multiple-sprite slicing)
- Summary of implementation:
  - Uses an `AssetPostprocessor` (`SpriteSheetParser`) to configure sprite import and slice textures into a grid of sprites.
  - `OnPreprocessTexture` sets `SpriteImportMode.Multiple`, `FilterMode.Point`, `TextureImporterType.Sprite`, `spritePixelsPerUnit = 50`, and disables mipmaps.
  - `OnPostprocessTexture` reads per-asset settings from `SpriteSheet2AnimCustomEditor.SpriteSheetDefinitionsLookup` and calls `InternalSpriteUtility.GenerateGridSpriteRectangles(texture, ...)` using `SpriteWidth`/`SpriteHeight`.
  - Rectangles are ordered by Y (desc) then X to produce a consistent frame order; sprite rects are named `<file>_<index>`.
  - Uses `SpriteDataProviderFactories`/`SpriteEditorDataProvider` to set the generated `SpriteRect[]` and apply them to the importer.
- Notes:
  - `OnPostprocessSprites` is wired for future use (e.g., mapping sprites to animation sets) and currently logs the sprite count when settings exist.
  - Meta changes observed under `Assets/Sprites/Characters/Female_Musketeer/` indicate slicing is active.

## Example JSON mapping (optional)

```
{
  "pixelsPerUnit": 100,
  "actions": {
    "idle": { "startFrame": 0, "endFrame": 3, "loop": true },
    "walk": { "startFrame": 4, "endFrame": 11, "loop": true }
  },
  "frameOrder": "left-to-right"
}
```

Notes:
- Allow explicit rectangle lists when frames are uneven or contain blanks
- Provide a simple preview during import to validate slices

## Output Layout (suggested)

- `Assets/Animations/Characters/<CharacterName>/` for generated clips
- `Assets/Animations/Controllers/<CharacterName>.controller` when controller generation is enabled

## Integration Notes

- At runtime, `CharacterAnimationHandler` reads direction and updates animator params. The importer should standardize clip names/parameters so handlers remain generic.
- Keep import-time names and folder structure stable to avoid breaking references.

## Tasks / TODO

- [x] Slice to `Sprite`s based on config (grid-based)
- [ ] Create `CharacterSpriteImporter` editor script (skeleton)
- [ ] Inspector UI for grid size, PPU, pivot, mapping source (inline/JSON)
- [ ] Generate `AnimationClip`s for named actions (looping, frame rates)
- [ ] Optional `AnimatorController` generation (states + parameters)
- [ ] Re-import/overwrite strategy and idempotency
- [ ] Minimal preview window for slices
- [ ] Sample JSON mappings and documentation

## Risks / Open Questions

- Different art packs may use inconsistent frame orders and sizes — mitigated via JSON mapping and explicit rectangles
- Clip naming conventions vs. animator parameter names — document and enforce via importer defaults
