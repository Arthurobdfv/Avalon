# Sprite Pipeline

- **Status:** In Progress
- **Summary:** Automated editor tooling that slices sprite sheets into sprites and generates animation clips and animator controllers for characters.

## Overview

The sprite pipeline automates the repetitive task of converting character sprite sheets into usable Unity animations. Instead of manually slicing sprites and creating animation clips, this editor tool automatically processes sprite sheets based on configuration and generates all necessary assets.

This significantly improves iteration speed when working with new character art or updating existing sprite sheets.

## User Experience

### For Artists

Artists can:
- Drop a new sprite sheet into the project
- Provide a simple configuration (grid size, action names)
- Automatically get sliced sprites, animation clips, and animator controller

### For Developers

Developers get:
- Consistent naming conventions across all character animations
- Automatically generated animator controllers with standard states
- Easy re-import when sprite sheets are updated
- Less time spent on tedious manual setup

## Implementation

### Current State (What's Done)

**✅ Sprite Slicing:**
- Implemented in `SpriteSheetParser` (AssetPostprocessor)
- Automatically sets texture import properties
- Uses grid-based rectangle generation
- Orders frames consistently (Y-descending, then X-ascending)
- Applies sprite metadata via Unity's SpriteDataProvider API

**✅ Import Configuration:**
- Texture settings auto-configured on import:
  - Compression: Uncompressed
  - Sprite mode: Multiple
  - Filter mode: Trilinear
  - Texture type: Sprite
  - Pixels per unit: 50 (configurable)

**✅ Data Model:**
- `AnimationSetDefinition` with character name metadata
- Configuration lookup via `SpriteSheetDefinitionsLookup`
- Grid-based frame layout definitions

**✅ Animator Controller Prototype:**
- Basic animator generation implemented
- Follows Avalon-specific conventions (prototype)
- Needs generalization and configuration

### Not Yet Implemented

**❌ Animation Clip Creation:**
- Automatic generation of AnimationClip assets from sliced sprites
- Mapping sprite frames to named animations (idle, walk, attack, etc.)
- Hook in `OnPostprocessSprites` for clip generation

**❌ Configuration UI:**
- Inspector UI for editing mapping rules
- Grid size and pivot configuration
- Frame range specification
- JSON import support

**❌ Generalized Animator Generation:**
- Configurable transition rules
- Template-based state machine creation
- Parameter customization

**❌ Preview and Validation:**
- Preview window for slice verification
- Visual feedback for frame mapping
- Error reporting for invalid configurations

**❌ Idempotent Re-import:**
- Safe overwrite strategy for generated assets
- Preserve manual changes when possible
- Clear user prompts for conflicts

### Technical Implementation

**Location:** `Assets/Scripts/Editor/SpriteSheet2Anim/`

**Key Classes:**
- `SpriteSheetParser` - AssetPostprocessor for texture import
- `AnimationSetDefinition` - Configuration data model
- `SpriteSheetDefinitionsLookup` - Configuration registry

**Import Flow:**
```
1. Sprite sheet imported or re-imported
   |
2. SpriteSheetParser.OnPreprocessTexture()
   - Set import properties
   - Configure sprite mode, PPU, filter, etc.
   |
3. Unity imports texture
   |
4. SpriteSheetParser.OnPostprocessTexture()
   - Look up configuration from definitions
   - Generate grid rectangles
   - Order frames consistently
   - Apply sprite metadata
   |
5. [TODO] OnPostprocessSprites()
   - Create AnimationClip assets
   - Generate Animator Controller
   - Wire up transitions
```

## Architecture

The sprite pipeline is an **Editor-only** system that extends Unity's asset import pipeline.

**Design Principles:**
- **Convention over configuration** - Sensible defaults for common use cases
- **Non-destructive** - Original sprite sheets are never modified
- **Configurable** - Override defaults when needed
- **Idempotent** - Re-importing produces consistent results

**Integration:**
- Hooks into Unity's AssetPostprocessor system
- Uses SpriteDataProviderFactories for sprite metadata
- Leverages InternalSpriteUtility for grid generation
- Generates standard Unity assets (AnimationClip, AnimatorController)

See [Animation System Architecture](../architecture/animation-system.md) for how generated assets are used.

## Usage

### Current Usage (Slicing Only)

1. **Prepare sprite sheet:**
   - Standard grid layout (e.g., 64x64 pixels per frame)
   - Consistent frame ordering

2. **Create configuration:**
   - Define animation set in `SpriteSheetDefinitionsLookup`
   - Specify grid size and character name

3. **Import texture:**
   - Drop sprite sheet into Unity project
   - Import settings automatically applied
   - Sprites automatically sliced

### Planned Usage (Full Pipeline)

```
1. Import sprite sheet
2. Select texture in Inspector
3. Configure in custom import UI:
   - Grid size (64x64)
   - Pixels per unit (50)
   - Action definitions:
     - "Idle": frames 0-3, loop
     - "Walk": frames 4-11, loop
     - "Attack": frames 12-16, no loop
4. Click "Generate Animations"
5. Generated assets:
   - Sprites/Characters/Hero/ (sliced sprites)
   - Animations/Characters/Hero/ (animation clips)
   - Animations/Controllers/Hero.controller
```

### Configuration Format (Planned)

**Example JSON sidecar:**
```json
{
  "pixelsPerUnit": 100,
  "actions": {
    "idle": { 
      "startFrame": 0, 
      "endFrame": 3, 
      "loop": true 
    },
    "walk": { 
      "startFrame": 4, 
      "endFrame": 11, 
      "loop": true 
    },
    "attack": {
      "startFrame": 12,
      "endFrame": 16,
      "loop": false
    }
  },
  "frameOrder": "left-to-right"
}
```

## Known Limitations

### Prototype Animator Generation

Current animator generation is Avalon-specific:
- Hardcoded transition rules
- Fixed state machine structure
- Not configurable or generalizable

**Needs:** Template system or configuration for custom state machines

### No JSON Sidecar Support

Configuration is hardcoded in editor scripts:
- No per-asset configuration files
- Can't version control sprite configurations separately
- Hard to share configurations between artists

### Inconsistent Frame Layouts

Art packs with non-uniform frame sizes or blank frames not supported:
- Assumes consistent grid layout
- No support for explicit rectangle lists
- No handling of irregular sprite sheets

**Solution:** Add support for explicit frame definitions

### No Validation or Previews

Missing user feedback:
- No preview of sprite slicing before import
- No validation of frame mappings
- Error messages are generic Unity import errors

## Next Steps

### Phase 1: Animation Clip Generation

- [ ] Implement clip creation in `OnPostprocessSprites`
- [ ] Map sliced sprites to animation clips based on frame ranges
- [ ] Generate clips with proper loop settings and frame rate
- [ ] Save clips to organized folder structure

### Phase 2: Configuration UI

- [ ] Create custom import UI in Inspector
- [ ] Add grid size and PPU controls
- [ ] Implement action definition editor
- [ ] Support JSON sidecar import

### Phase 3: Animator Generation

- [ ] Generalize animator controller generation
- [ ] Add configurable transition rules
- [ ] Support custom state machine templates
- [ ] Expose animator parameters configuration

### Phase 4: Polish

- [ ] Add preview window for slice verification
- [ ] Implement validation and error reporting
- [ ] Support idempotent re-import with conflict resolution
- [ ] Add example configurations and documentation

## Related Documentation

- [Character Animation](character-animation.md) - Uses generated animations
- [Animation System Architecture](../architecture/animation-system.md) - How animations work
- [Features Index](README.md) - All features
- [Contributing Guide](../contributing/README.md) - How to contribute
