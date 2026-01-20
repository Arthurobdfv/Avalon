# SpriteSheet to Animations Pipeline

- Status: In Progress (slicing implemented, animator-controller transitions prototype added)
- Summary: Editor tooling/importer that slices sprite sheet assets into `Sprite`s and will generate `AnimationClip`s and an optional `AnimatorController` for characters. Current work includes a prototype to wire animator transitions for this project's conventions; this prototype needs generalization and UI.

## Motivation

Automate repetitive and error-prone manual creation of animation clips and controllers from character sprite sheets, producing consistent outputs and a repeatable import workflow.

## Scope

- Unity Editor-only workflow to slice textures into `Sprite`s
- Generate `AnimationClip`s for named actions (idle, walk, attack, etc.)
- Optional generation/wiring of an `AnimatorController` and states
- Re-import support when mappings or settings change

Out of scope (for now):
- Runtime loading from external locations
- Complex, user-authored state machines (these will be supported later via configuration)

## Current implementation (what's actually in the repo)

- Location: `Assets/Scripts/Editor/SpriteSheet2Anim/`
- Slicing and import adjustments:
  - Implemented in `SpriteSheetParser` (an `AssetPostprocessor`) which:
    - On preprocess: sets import properties (e.g., `textureCompression = Uncompressed`, `spriteImportMode = Multiple`, `filterMode = Trilinear`, `textureType = Sprite`, `spritePixelsPerUnit = 50`) and reimports the texture.
    - On postprocess: reads per-asset settings from `SpriteSheet2AnimCustomEditor.SpriteSheetDefinitionsLookup`, computes grid rectangles using `InternalSpriteUtility.GenerateGridSpriteRectangles`, orders rectangles by Y(desc) then X to produce consistent frame ordering, and applies `SpriteRect[]` via `SpriteDataProviderFactories` / the sprite editor data provider.
  - `OnPostprocessSprites` exists but currently only logs sprite counts; mapping to clips is not yet implemented.
- Data model changes:
  - `CharacterName` field was moved into `AnimationSetDefinition` (so character-level metadata is kept with the animation set).
- Animator controller generation:
  - A prototype exists that generates animator transitions and wires them into a controller following the project's current conventions (Avalon-specific). This is a temporary implementation intended to be made configurable and generalized in following work.

## What is not implemented yet (and should be tracked)

- Automatic creation of `AnimationClip` assets from sliced sprites and named action ranges
- Mapping from sliced sprites to named animations (OnPostprocessSprites ? clip generation)
- UI/inspector to edit mapping rules (grid size, PPU, pivot, frame ranges, JSON import)
- Generalized animator transition rules and exposed configuration (current transitions are hardcoded/prototype)
- Preview window for slice/clip verification
- Idempotent re-import behavior for generated assets (overwrite strategy, safe re-import)

## Example JSON mapping (for future sidecar mappings)

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
- Allow explicit rectangle lists when frames are uneven or contain blanks.
- The JSON shape above is a suggested sidecar format; the implementation should validate fields and provide clear error messages.

## Output layout (suggested)

- `Assets/Animations/Characters/<CharacterName>/` for generated clips
- `Assets/Animations/Controllers/<CharacterName>.controller` when controller generation is enabled

## Integration notes

- `CharacterAnimationHandler` and runtime code expect consistent clip names and animator parameters. Keep naming stable or provide a mapping layer in the importer.
- Validate generated controllers in-editor after import; the current prototype follows Avalon conventions and should be reviewed before wider use.

## Related documentation

- Root changelog: `../CHANGELOG.md`
- Project README: `../README.md`

## Tasks / TODO (updated)

- [x] Slice to `Sprite`s based on grid config (implemented)
- [ ] Implement automatic generation of `AnimationClip` assets from frame ranges
- [ ] Implement mapping logic to convert sliced sprites into named animations (hook up in `OnPostprocessSprites`)
- [ ] Expose inspector UI for grid size, PPU, pivot, mapping source (inline/JSON)
- [ ] Generalize animator-controller generation and expose transition rules (current prototype is Avalon-specific)
- [ ] Re-import/overwrite strategy and idempotency improvements
- [ ] Minimal preview window for slices and generated clips
- [ ] Sample JSON mappings and documentation

## Risks / Open questions

- Art packs with inconsistent frame sizes/order will require either explicit rectangle lists or robust JSON mappings; include validation and helpful error messages.
- Decide and document canonical clip naming and animator parameter conventions so runtime handlers remain generic.
