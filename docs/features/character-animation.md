# Character Animation

- **Status:** Implemented (basic)
- **Summary:** The base character spritemap has been imported and an initial set of example animations (e.g., idle, walk) were created manually. This feature demonstrates character visuals and simple animation playback in the sample scene.
- **Relevant paths:**
  - `Assets/Sprites/Characters/` (spritemaps / textures)
  - `Assets/Animations/Characters/` (animation clips / animator controllers)
  - `Scenes/SampleScene.unity`

## Current Implementation

- Base spritemap imported into the project.
- Example animation clips created by hand and assigned to a basic Animator on the character prefab used in `SampleScene`.
- Purpose: provide a working visual example to validate animation frames, pivots, and export parameters.

## Release notes

- Added a full set of eight-direction walk animation clips for a sample character, improving visual coverage for movement in all directions.
- Introduced `CharacterAnimationHandler` to coordinate animation state changes from movement/direction input.
- Updated the character `Sprites.controller` and sample scene to use the new directional clips.

Developer notes

- The current animation handler uses direct updates; consider refactoring to an event-driven approach to decouple input/character logic from animation playback.
- All new animation clips were created manually; automating this process via a custom importer remains a planned enhancement (see the Custom Asset Importer section).


## Goals / Next Steps

- Improve iteration speed by automating the spritemap -> animation pipeline.
- Add configurable importer options (grid slicing, frame order, naming conventions).

- Next work item: implement a sprite-sheet parser that automatically slices character spritemaps and generates `AnimationClip` assets and an optional `AnimatorController` (see the Custom Asset Importer section below for details).

## Custom Asset Importer (moved ? see: [SpriteSheet to Animations Pipeline](SpriteSheet-to-Animations.md))

This importer will run in the Unity Editor and convert a provided spritemap into sliced sprites and generated `AnimationClip`s and Animator setups.

### Requirements / Acceptance criteria

- Provide an editor configuration UI or per-asset metadata to set:
  - Slice grid size (columns/rows) or explicit frame rectangles
  - Frame order mapping (left-to-right, top-to-bottom, custom JSON mapping)
  - Named animation ranges (e.g., Idle: frames 0-3, Walk: frames 4-11)
  - Import settings (Pixels Per Unit, pivot, filter mode)
- Automatically create `AnimationClip` assets for each named action and optionally an `AnimatorController` with states.
- Handle common formats: single spritemap PNG + optional JSON mapping file (name matching or explicit mapping).
- Provide safe defaults and an override to re-import and overwrite generated clips when mappings change.

### Suggested Implementation Notes

- Location suggestion: `Assets/Editor/Importers/CharacterSpriteImporter.cs` (Editor script).
- Workflow example:
  1. Add 1 PNG spritemap and optional JSON mapping (e.g., `hero_spritemap.png`, `hero_spritemap.json`).
  2. Select the spritemap in the Editor and choose `Reimport with Character Importer` or set importer settings in the Inspector.
  3. The importer slices the texture into `Sprite` assets, creates `AnimationClip` assets (one per named action), and optionally generates `AnimatorController` and prefab hooks.

### Example JSON mapping (optional)

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

### Edge cases & notes

- Support varying frame sizes or empty frames by allowing explicit rectangle lists in the mapping.
- Provide a preview window in the importer to validate slices before generating assets.

## Tasks / TODO

- [x] Document current state and goals (this file)
- [ ] Implement `CharacterSpriteImporter` skeleton (Editor)
- [ ] Add inspector UI for mapping and run/import process
- [ ] Generate `AnimationClip`s, `AnimatorController`, and optional prefab wiring

- [ ] Implement sprite-sheet parser + automatic animation creation (priority next work item)

---

When you're ready I can scaffold the importer script and a minimal UI that reads the example JSON format above.

