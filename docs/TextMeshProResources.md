# TextMesh Pro Resources

## Overview
This document tracks the TextMesh Pro resources that are now checked into source control so text rendering works in all environments and builds without relying on local package defaults.

## Included assets
- Default TMP font asset `LiberationSans SDF` with outline and drop shadow materials and a fallback asset.
- TMP settings asset and style sheet for default text styles.
- Emoji sprite asset (with JSON data, PNG texture, and attribution file) under `Assets/TextMesh Pro/Sprites/`.
- Line breaking rules tables and sprite asset folder metadata.
- Complete set of TMP shaders and shader includes used by the default materials.

## Unstaged / Recent runtime changes
- Added the full set of TextMesh Pro default resources (fonts, materials, sprites, shaders, settings, and line breaking tables) under `Assets/TextMesh Pro/` so builds and other environments have the necessary text rendering assets.
