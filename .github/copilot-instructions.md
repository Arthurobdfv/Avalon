# Copilot Documentation Notes

Purpose

This file is the canonical summary of the repository structure, architecture, documentation workflow, and contributor guidance that automated tools (including Copilot automation) and humans should follow when creating prompts, commits, and pull requests.

Repository summary

- Name: `Avalon` - a Unity-based MMO prototype and learning project.
- Purpose: proof-of-concept for MMO mechanics, server ideas, and Unity development practice.
- Status: Prototype / Work in progress.

Documentation structure (updated January 2024)

The documentation has been restructured into a clear hierarchy:

```
docs/
├── README.md                    # Documentation hub - start here
├── architecture/                # Technical architecture docs
│   ├── README.md               # Architecture overview
│   ├── combat-system.md        # Combat system architecture
│   ├── multiplayer-architecture.md  # Multiplayer and packet flow
│   ├── entity-management.md    # Entity registry and lifecycle
│   ├── input-system.md         # Input handling architecture
│   └── animation-system.md     # Animation coordination
├── features/                    # User-facing feature docs
│   ├── README.md               # Feature index
│   ├── combat-flow.md          # Combat feature documentation
│   ├── character-movement.md   # Movement feature
│   ├── character-animation.md  # Animation feature
│   ├── sprite-pipeline.md      # Sprite sheet automation
│   └── enemy-behavior.md       # Enemy AI feature
├── contributing/                # Contribution guidelines
│   ├── README.md               # How to contribute
│   ├── documentation-guide.md  # Documentation conventions
│   └── code-style.md           # Coding conventions
└── changelog/                   # Changelog reference
    └── README.md               # Points to root CHANGELOG.md
```

Key documentation files:
- `README.md` - Project overview with links to new docs structure
- `CHANGELOG.md` - Unreleased changes and version history
- `docs/README.md` - Documentation hub (entry point)
- `docs/architecture/` - Technical system documentation
- `docs/features/` - User-facing feature documentation
- `docs/contributing/` - Contribution and style guides

High-level architecture

- Unity client projects: `Assembly-CSharp` and `Assembly-CSharp-Editor` (targets: .NET Framework 4.7.1).
- Purpose-built systems in the repo include: sprite-sheet-to-animation pipeline, entity manager and combat flow, enemy behaviour manager.
- Design intent: Architecture docs (technical) separate from feature docs (user-facing).

Documentation workflow (updated)

Documentation workflow (updated)

- Feature lifecycle:
  - Add architecture docs to `docs/architecture/` for technical systems
  - Add feature docs to `docs/features/` for user-facing functionality
  - Use kebab-case for all documentation filenames (e.g., `combat-system.md`, `character-movement.md`)
  - Update relevant `README.md` index files when adding new docs
  - Add a short note in `CHANGELOG.md` under `Unreleased` for notable changes

- Documentation types:
  - **Architecture docs** (`docs/architecture/`): Technical design, implementation details, system interactions
  - **Feature docs** (`docs/features/`): User-facing features, usage guides, how-to instructions
  - **Contributing docs** (`docs/contributing/`): How to contribute, code style, documentation guide

- Commit guidance:
  - Small, focused commits. Use imperative mood and reference affected files or docs.
  - When updating docs with code, change `CHANGELOG.md` at the same time.
  - Format: `docs(scope): imperative summary` for documentation changes

- Pull request guidance:
  - Include summary, related docs/issue links, testing steps, affected files, and list docs updated.
  - Add visuals for UI/visual changes.
  - Reference the documentation hub (`docs/README.md`) for navigation

- Prompts and Copilot automation guidance:
  - Provide explicit file paths and concise behavior descriptions when requesting automated edits.
  - After automation edits: update or create relevant docs in `docs/architecture/` or `docs/features/`
  - Follow `docs/contributing/documentation-guide.md` when creating commit messages or PR descriptions from unstaged changes.
  - Add "Unstaged / Recent runtime changes" section for work-in-progress documentation

Manager summary (runtime & multiplayer)
- `GlobalEntitiesManager`: global registry, listens to spawn/despawn, instantiates players on connect, and broadcasts per-map `EntitySpawnPacket` snapshots via `ServerCommunicationLayerManager.SendMap` in `LateUpdate`.
- `PlayerEntitiesManager`: map-aware player lookup and interaction helper (assigns nearest enemy on the player map).
- `EnemyBehaviourManager`: hooks `CombatManager.BeforeCombatTickHandler` to refresh enemy targeting/aggro per combat tick using map-aware queries.
- `CombatManager`: fixed-step combat tick dispatcher; resolves `OnPerformCombatHandler` by applying damage; exposes before/after tick hooks.
- Multiplayer layers: `MultiplayerConnectionManager`, `ServerCommunicationLayerManager`, `ClientCommunicationLayerManager`, and packet handlers (`ServerPacketHandler`/`ClientPacketHandler`) orchestrate connect/observe/map routing; local loopback uses `Local*PacketSender/Receiver` for in-editor play.
- TODO: `PlayersInputManager` server-side input aggregation is unimplemented; combat is currently local/instance-authoritative.

Documentation generation note: avoid inserting non-ASCII or special punctuation characters (for example: smart quotes, em-dashes, non-breaking spaces, and other locale-specific symbols) in generated Markdown files. Prefer ASCII characters and simple punctuation (straight quotes, hyphen-minus `-`, plain spaces). This reduces the risk of encoding errors during static site builds or when tools assume UTF-8 encoding.

Conventions and coding notes

- Follow existing code style. Keep changes minimal and consistent.
- Prefer existing helpers and avoid new third-party dependencies unless necessary.
- Guard editor-only code for `Assembly-CSharp-Editor`.

Current unstaged summary (feature/Client_Server_Handling_Reestructure)
- Added full TextMesh Pro default resources under `Assets/TextMesh Pro/` (fonts, materials, shaders, sprite assets, settings, line breaking tables) and documented in `docs/TextMeshProResources.md` with links from `README.md` and `FEATURES/FEATURES.md`.
- Documented multiplayer architecture, manager responsibilities, and map-scoped entity snapshots (`docs/MultiplayerArchitecture.md`, `docs/feature-basic-entity-manager-and-combat.md`), including GlobalEntitiesManager, connection/observer flow, and map data notes.
- Expanded combat docs to clarify tick hook order, multiplayer input flow, and server-authoritative limitations (`docs/CombatSystem.md`).

Automation bookkeeping

- This file was updated after the January 2024 documentation restructure.
- Documentation is now organized into `docs/architecture/`, `docs/features/`, and `docs/contributing/`
- Update this file whenever Copilot automation performs repository-wide documentation edits.
- If files listed here are renamed/removed, update this file accordingly.

Action items for maintainers

- To document unstaged changes and prepare commits: run `git status --porcelain --untracked-files=all`, follow `docs/contributing/documentation-guide.md`, update the related docs file and `CHANGELOG.md`, then create a commit and PR with the generated message and description.
- When adding new documentation, update the appropriate `README.md` index in `docs/architecture/`, `docs/features/`, or `docs/contributing/`
- Major documentation changes should be noted in root `CHANGELOG.md` under `Unreleased`

Notes

- Documentation structure follows kebab-case naming convention
- All docs use relative links for cross-references
- Documentation hub is at `docs/README.md`
- Old scattered documentation has been consolidated and reorganized

# PR Message Requests (automation directive)

When the user asks Copilot to produce a PR message, the automated agent should perform the following and include the results in the response whenever possible:

- Summarize the diff between the source and target branches:
  - List files added, modified, and removed.
  - For each changed file (or grouped by area), include a one-line summary of the change (e.g., "Updated combat tick handling in `Assets/Scripts/Character/Combat/CombatManager.cs`").
  - If available, include the number of commits and a short summary of their intents.

- Produce a conventional PR title and body.

- Provide the PR message inside a Markdown code block (```markdown ... ```), ready for copy/paste into the GitHub UI.

- Optionally include a one-line `CHANGELOG.md` entry under `Unreleased` formatted as a suggested addition.

- If the diff is large or spans unrelated changes, suggest splitting into multiple PRs and explain why briefly.

Notes for automated agents

- Use repository tools (`git` or hosted API) to compute the diff when possible. If the agent cannot access branch diffs, explain why and provide the best possible summary from available information.
- Keep the PR message concise and focused; include testing steps and files of interest in the PR body.
