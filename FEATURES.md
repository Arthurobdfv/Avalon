# Features

A concise TODO-style list of implemented and planned features. Click an item to jump to details below.
---


## Todo

- [x] [Character Animation](FEATURES/Character-Animation.md) — base spritemap imported; example animations created
- [x] [Character Movement](FEATURES/Character-Movement.md) — player movement + server-tick sync; input integration
- [x] [Basic Entity Manager & Combat Flow](docs/feature-basic-entity-manager-and-combat.md) — implemented (basic)
- [ ] [SpriteSheet to Animations Pipeline](FEATURES/SpriteSheet-to-Animations.md) — automatically slice spritemap and generate animation clips
- [ ] Build Pipeline - Whenever merging from DEV -> Master, will trigger a GH Actiion to build the project.



---

## Details

### Character Animation

- **Status:** Implemented (basic)
- **Summary:** The base character spritemap has been imported and an initial set of example animations were created by hand (simple walk/idle frames).
- **Relevant paths:** `Assets/Sprites/Characters/`, `Assets/Animations/Characters/`, `Scenes/SampleScene.unity`
- **Notes / Docs:** Use this section to paste PR links, commit hashes, or external docs.

### SpriteSheet to Animations Pipeline

- **Status:** Planned
- **Goal:** Implement an editor-side custom asset importer that automatically slices the character spritemap into animation frames and creates animation clips and animator setups.
- **Acceptance criteria:**
   - Configurable slice grid and frame order mapping
   - Automatic creation of `AnimationClip`s for named actions (idle, walk, attack, etc.)
   - Proper import settings (pixels per unit, pivot, filter mode)
   - Optional naming convention or JSON mapping file support
- **Suggested location:** `Assets/Editor/Importers/CharacterSpriteImporter.cs`
- **Docs:** See [SpriteSheet to Animations Pipeline](FEATURES/SpriteSheet-to-Animations.md)

### Build Pipeline

- **Status:** Planned
- **Goal:** Automated CI/CD using github Actions to deploy to a GH Pages branch whenever merged to master
- **Acceptance criteria:**
   - Modular enough for in the future having it for multiple envs
   - For builds from 'master', deploy to a GH Pages branch
---

If you want, I can also create the starter editor script for the custom importer (skeleton) and add example configuration. Should I add that now?


