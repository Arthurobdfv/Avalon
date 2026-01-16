# Copilot Documentation Notes

Purpose

This file is the canonical summary of the repository structure, architecture, documentation workflow, and contributor guidance that automated tools (including Copilot automation) and humans should follow when creating prompts, commits, and pull requests.

Repository summary

- Name: `Avalon` — a Unity-based MMO prototype and learning project.
- Purpose: proof-of-concept for MMO mechanics, server ideas, and Unity development practice.
- Status: Prototype / Work in progress.
- Key documentation files present in the repository (scanned):
  - `README.md`
  - `CHANGELOG.md`
  - `.github/copilot-instructions.md` (this file)
  - `docs/CombatSystem.md`
  - `docs/COPILOT_DOCS_DOCUMENTING.md`
  - `docs/MultiplayerArchitecture.md`
  - `docs/TextMeshProResources.md`
  - `docs/feature-basic-entity-manager-and-combat.md`

Note: other documents referenced in prior guidance such as `DOCUMENTATION.md` or `FEATURES/FEATURES.md` are not present in the repository. If you rely on those files for workflow or indexing, add them to the repo or update this file to point to existing equivalents.

High-level architecture

- Unity client projects: `Assembly-CSharp` and `Assembly-CSharp-Editor` (targets: .NET Framework 4.7.1).
- Purpose-built systems in the repo include: sprite-sheet-to-animation pipeline, entity manager and combat flow, enemy behaviour manager.
- Design intent: separate high-level feature docs from implementation notes; currently implementation notes are primarily under `docs/`.

Scanned Markdown files (details)

- `README.md`
  - Project overview, purpose, status, pointers to feature docs (references `FEATURES/FEATURES.md` though that file is not present), credits, AI usage disclaimer, and brief how-to-contribute guidance.

- `CHANGELOG.md`
  - Contains an `Unreleased` section summarizing recent features and updates: sprite-sheet pipeline changes, enemy behavior manager, and miscellaneous fixes. Also lists docs updated.

- `docs/CombatSystem.md`
  - Detailed runtime and design notes for the combat subsystem: `CombatManager`, `CombatCharacter`, `CombatBaseStats`, `EnemyCombatBaseStats`, `PlayerCharacter`, `EnemyCharacter`, utilities, setup steps, known limitations, and files of interest under `Assets/Scripts/Character/...`.
  - Includes an "Unstaged / Recent runtime changes" section describing recent edits to combat scripts and example assets.

- `docs/COPILOT_DOCS_DOCUMENTING.md`
  - Process and rules Copilot should follow when documenting unstaged changes and producing commit messages or PR descriptions: inspect `git status`, read changed files, summarize runtime-relevant changes, update or create `docs/` pages with an "Unstaged / Recent runtime changes" section, and compose conventional-style commit messages.
  - Specifies expected request format from users when asking Copilot to document and commit.

- `.github/copilot-instructions.md` (this file)
  - Aggregates repository guidance for prompts, commits, PRs, and automated documentation bookkeeping. This file should be kept in sync when docs or workflow conventions change.

Updated documentation & workflow rules (clarified)

- Feature lifecycle
  - Add or update a feature doc under `docs/` or create `FEATURES/` if you want a dedicated feature index. If you create `FEATURES/FEATURES.md`, update this file to reference it.
  - Implementation notes and runtime-focused details belong in `docs/`.
  - Add a short note in `CHANGELOG.md` under `Unreleased` for notable changes.

- Commit guidance
  - Small, focused commits. Use imperative mood and reference affected files or docs. When updating docs with code, change `CHANGELOG.md` at the same time.

- Pull request guidance
  - Include summary, related docs/issue links, testing steps, affected files, and list docs updated. Add visuals for UI/visual changes.

- Prompts and Copilot automation guidance
  - Provide explicit file paths and concise behavior descriptions when requesting automated edits.
  - After automation edits: add an "Unstaged / Recent runtime changes" section to the related `docs/` page and append a one-line note to `CHANGELOG.md` under `Unreleased`.
  - Follow `docs/COPILOT_DOCS_DOCUMENTING.md` when creating commit messages or PR descriptions from unstaged changes.

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

- Update this file whenever Copilot automation performs repository-wide documentation edits.
- If files listed here are renamed/removed, update this file accordingly.

Action items for maintainers

- If you want a feature index, add `FEATURES/FEATURES.md` and update this file with its path.
- If you want a documentation structure file, add `DOCUMENTATION.md` and update references.
- To document unstaged changes and prepare commits: run `git status --porcelain --untracked-files=all`, follow `docs/COPILOT_DOCS_DOCUMENTING.md`, update the related `docs/` file and `CHANGELOG.md`, then create a commit and PR with the generated message and description.

Notes

- This file was updated after scanning the repository for all `*.md` files. It reflects the current set of Markdown documents present in the repository root and `docs/` folder.

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
