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
- `PlayerInputHandler` (client): builds `PlayerInputState` from raw input, sends to server via `ClientCommunicationLayerManager`. Does NOT mutate entity state.
- `PlayersInputManager` (server): receives `PlayerInputState` via `ServerPacketHandler`, aggregates one per client per cycle, processes movement/interaction, mutates entity state.
- `EntitySpawner` (client): receives `EntitySpawnPacket` from server, spawns/updates entities with authoritative position, direction, and movement state.
- `DirectionEnumHelper`: shared static utility for Vector2 to `DirectionEnum` conversion, used by server-side input processing.
- Combat is currently local/instance-authoritative; server-authoritative combat is a future goal.

Documentation generation note: avoid inserting non-ASCII or special punctuation characters (for example: smart quotes, em-dashes, non-breaking spaces, and other locale-specific symbols) in generated Markdown files. Prefer ASCII characters and simple punctuation (straight quotes, hyphen-minus `-`, plain spaces). This reduces the risk of encoding errors during static site builds or when tools assume UTF-8 encoding.


Conventions and coding notes

- Follow existing code style. Keep changes minimal and consistent.
- Prefer existing helpers and avoid new third-party dependencies unless necessary.
- Guard editor-only code for `Assembly-CSharp-Editor`.

Current unstaged summary (feature/Client_Server_Handling_Reestructure)
- Added full TextMesh Pro default resources under `Assets/TextMesh Pro/` (fonts, materials, shaders, sprite assets, settings, line breaking tables) and documented in `docs/TextMeshProResources.md` with links from `README.md` and `FEATURES/FEATURES.md`.
- Documented multiplayer architecture, manager responsibilities, and map-scoped entity snapshots (`docs/MultiplayerArchitecture.md`, `docs/feature-basic-entity-manager-and-combat.md`), including GlobalEntitiesManager, connection/observer flow, and map data notes.
- Expanded combat docs to clarify tick hook order, multiplayer input flow, and server-authoritative limitations (`docs/CombatSystem.md`).
- Refactored player input handling into client/server responsibilities:
  - `PlayerInputHandler` (client): builds and sends `PlayerInputState`, does NOT mutate entity state.
  - `PlayersInputManager` (server): receives, aggregates (one per client per cycle), and processes input states, mutating entity state.
  - `EntitySpawner` (client): receives `EntitySpawnPacket` and applies authoritative entity state.
  - `DirectionEnumHelper`: shared static utility for Vector2 to DirectionEnum conversion.
- Added comprehensive packets and handlers documentation (`docs/PacketsAndHandlers.md`).
- Added Mermaid diagrams to all architecture documentation files.
- Updated documentation in `docs/MultiplayerArchitecture.md`, `docs/CombatSystem.md`, `docs/feature-basic-entity-manager-and-combat.md`, `FEATURES/Character-Movement.md`, and `CHANGELOG.md`.

## Session Summary (Copilot Context)

This section summarizes all work completed in the current Copilot session for reference in future prompts.

### Code Changes

1. **PlayerInputHandler refactored** (`Assets/Scripts/Input/Handler/PlayerInputHandler.cs`)
   - Removed all entity mutation code (SetMovement, SetDirection, position updates)
   - Now only collects input and sends `PlayerInputState` to server
   - Removed dependencies on `PlayerEntitiesManager`, `ClientPacketHandler` for reconciliation
   - Simplified to ~80 lines focused purely on client input collection

2. **PlayersInputManager implemented** (`Assets/Scripts/Multiplayer/Server/PlayersInputManager.cs`)
   - Server-side input aggregation (one input per client per cycle)
   - Processes movement and interaction, mutating entity state
   - Uses `DirectionEnumHelper` for direction conversion
   - Entity state broadcast handled by `GlobalEntitiesManager` via `EntitySpawnPacket`

3. **DirectionEnumHelper created** (`Assets/Scripts/Input/Common/DirectionEnumHelper.cs`)
   - Shared static utility for Vector2 to DirectionEnum conversion
   - Used by server-side `PlayersInputManager`

### Architecture Established

```
Client                              Server
  |                                   |
  | PlayerInputHandler                |
  | (collects input)                  |
  |                                   |
  |---PlayerInputState--------------->| PlayersInputManager
  |                                   | (processes input, mutates entities)
  |                                   |
  |                                   | GlobalEntitiesManager
  |                                   | (broadcasts EntitySpawnPacket per map)
  |                                   |
  |<------EntitySpawnPacket-----------| 
  |                                   |
  | EntitySpawner                     |
  | (applies authoritative state)     |
```

### Documentation Created/Updated

1. **New: `docs/PacketsAndHandlers.md`** - Comprehensive packet and handler reference with Mermaid diagrams
2. **Updated: `docs/MultiplayerArchitecture.md`** - Added architecture diagram, sequence diagrams, cross-references
3. **Updated: `docs/CombatSystem.md`** - Added class diagram, combat tick sequence diagram
4. **Updated: `docs/feature-basic-entity-manager-and-combat.md`** - Added entity management diagrams, attack flow
5. **Updated: `FEATURES/Character-Movement.md`** - Added input flow sequence diagram, architecture graph
6. **Updated: `CHANGELOG.md`** - Documented all changes
7. **Updated: `.github/copilot-instructions.md`** - Added documentation review process, architecture overview

### Key Design Decisions Documented

- Client never mutates entity state - only sends input
- Server is authoritative - processes input and mutates entities
- Entity state broadcast via `EntitySpawnPacket` per map
- Input aggregation: one input per client per `FixedUpdate` cycle
- Combat tick is local/instance-authoritative (server-authoritative combat is future work)

## Suggested Next Steps

### 1. TCP Server/Client Transport Layer

Replace local loopback with real network transport:

```
Priority: High
Files to create:
- Assets/Scripts/Multiplayer/Client/Sender/Implementations/TcpClientPacketSender.cs
- Assets/Scripts/Multiplayer/Server/Receiver/Implementation/TcpServerPacketReceiver.cs
- Assets/Scripts/Multiplayer/Server/Sender/Implementation/TcpServerPacketSender.cs
```

Key considerations:
- Implement `IAvalonClientPacketSender`, `IAvalonPacketServerReceiver`, `IAvalonPacketServerSender`
- Use async/await for non-blocking I/O
- Implement packet serialization (JSON, MessagePack, or custom binary)
- Handle connection lifecycle (connect, disconnect, reconnect)

### 2. TCP Relay Server (for NAT traversal) - IMPLEMENTED

A TCP Relay Server has been created as a separate .NET 8 project **outside the Unity project** to avoid compilation conflicts:

```
Location: C:\Users\arthu\Documents\Github\AvalonRelayServer\
         (sibling directory to Avalon)

Solution: AvalonRelayServer.sln

Projects:
- AvalonRelayServer: Main relay server application (.NET 8)
- AvalonShared: Shared packet types and serialization (netstandard2.1 for Unity compatibility)
```

Build and run:
```bash
cd ../AvalonRelayServer
dotnet build
dotnet run --project AvalonRelayServer -- --port 7777
```

Features:
- Room-based routing (clients join rooms, packets route within rooms)
- Game server designation (one client per room can be authoritative)
- Flexible targeting (broadcast, game server only, specific clients)
- Heartbeat/keepalive with automatic timeout
- Length-prefixed JSON packets
- Docker support for cloud deployment

See `../AvalonRelayServer/README.md` for full documentation.

### 3. Unity Client Integration (Next Step)

To connect Unity to the relay server, implement:

```
Files to create:
- Assets/Scripts/Multiplayer/Client/Sender/Implementations/TcpRelayClientPacketSender.cs
- Assets/Scripts/Multiplayer/Relay/RelayConnection.cs
```

Integration approach:
1. Reference AvalonShared.dll (netstandard2.1 build) in Unity
2. Create `RelayConnection` class that manages TCP connection to relay
3. Wrap game packets in `RelayDataPacket` before sending
4. Unwrap received `RelayDataPacket` to dispatch game packets

### 4. Packet Serialization

Current packets are plain C# objects. For network transport, add serialization:

```
Options:
- Newtonsoft.Json (simple, human-readable, slower)
- MessagePack (binary, fast, smaller payloads)
- Custom binary (fastest, most control, more work)
```

### 5. Server-Authoritative Combat

Move combat resolution to server:

```
Files to modify:
- Assets/Scripts/Character/Combat/CombatManager.cs (make server-only)
- Create CombatResultPacket for broadcasting combat outcomes
- Clients receive damage/health changes via packets, not local calculation
```

### 6. Client-Side Prediction (Optional)

For better responsiveness with network latency:

```
- Client applies input locally (prediction)
- Server sends authoritative state
- Client reconciles (snap or interpolate to server state)
- Requires input sequence numbers and state history
```

### Recommended Order

1. **Packet serialization** - Required for any network transport
2. **TCP transport** - Basic networking without relay
3. **TCP relay server** - Solve NAT issues
4. **Server-authoritative combat** - Complete the server authority model
5. **Client-side prediction** - Polish for better feel

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

# Documentation Review Process (automation directive)

When the user asks Copilot to review or update documentation to ensure accuracy, the automated agent should follow these steps:

## Step 1: Enumerate repository documentation files

- Use a terminal command to list all `.md` files in the repository:
  ```powershell
  Get-ChildItem -Path "<repo_root>" -Recurse -Filter "*.md" | Where-Object { $_.FullName -notlike "*\Library\*" } | Select-Object -ExpandProperty FullName
  ```
- **Important**: Filter out the `Library/` directory (Unity package cache) to analyze only repository-owned documentation. The `Library/PackageCache/` folder contains third-party package docs that are not part of this project.

## Step 2: Identify key documentation files

Group the discovered files by purpose:
- **Root-level docs**: `README.md`, `CHANGELOG.md`, `DOCUMENTATION.md`, `FEATURES.md`
- **Process/workflow docs**: `.github/copilot-instructions.md`, `docs/COPILOT_DOCS_DOCUMENTING.md`
- **Architecture/feature docs**: `docs/*.md`, `FEATURES/*.md`
- **Templates**: `.github/ISSUE_TEMPLATE/*.md`, `.github/pull_request_template.md`

## Step 3: Read and cross-reference documentation with code

For each architecture/feature documentation file:
1. Read the documentation file to understand what it claims about the codebase.
2. Use `get_symbols_by_name` or `code_search` to locate the actual code being documented.
3. Read the relevant code files to verify the documentation is accurate.
4. Note any discrepancies (outdated TODOs, incorrect descriptions, missing new features).

## Step 4: Identify outdated statements

Look for common issues:
- TODOs or "not yet implemented" notes for features that have been implemented.
- Incorrect class/method names or file paths.
- Missing documentation for new classes, utilities, or architectural changes.
- Stale "Unstaged / Recent runtime changes" sections that should be updated or cleared.

## Step 5: Update documentation files

For each discrepancy found:
1. Update the relevant documentation file with accurate information.
2. Add new classes/utilities to relevant sections (e.g., Manager summary, Files of interest).
3. Update or remove outdated TODOs.
4. Add entries to `CHANGELOG.md` under `Unreleased` for significant documentation updates.

## Step 6: Update this file

After completing a documentation review:
- Update the "Current unstaged summary" section in this file.
- Update the "Manager summary" if new managers or utilities were added.
- Update the "Scanned Markdown files" section if new docs were created.

## Files to always check during documentation review

| File | Purpose | Check for |
|------|---------|-----------|
| `docs/MultiplayerArchitecture.md` | Multiplayer layer architecture | Packet handlers, managers, input flow |
| `docs/CombatSystem.md` | Combat tick system | Combat managers, tick hooks, multiplayer notes |
| `docs/feature-basic-entity-manager-and-combat.md` | Entity management | Entity managers, spawn flow, combat integration |
| `FEATURES/Character-Movement.md` | Player movement | Input handlers, client/server split |
| `CHANGELOG.md` | Change history | Recent changes documented |
| `.github/copilot-instructions.md` | Automation guidance | Manager summary, architecture overview |

## Project architecture overview (for reference during reviews)

### Client-side components
- `PlayerInputHandler`: builds `PlayerInputState` from raw input, sends to server via `ClientCommunicationLayerManager`. Does NOT mutate entity state.
- `ClientCommunicationLayerManager`: routes packets to/from server.
- `ClientPacketHandler`: dispatches incoming packets to registered handlers.
- `EntitySpawner`: receives `EntitySpawnPacket` from server, spawns/updates entities with authoritative state (position, direction, movement).

### Server-side components
- `PlayersInputManager`: aggregates player input (one per client per cycle), processes movement/interaction, mutates entity state.
- `ServerCommunicationLayerManager`: routes packets to/from clients, supports map-scoped delivery.
- `ServerPacketHandler`: dispatches incoming packets to registered handlers.
- `GlobalEntitiesManager`: global entity registry, broadcasts entity snapshots per map via `EntitySpawnPacket`.
- `PlayerEntitiesManager`: map-aware player lookup and interaction helper.
- `EnemyBehaviourManager`: enemy AI targeting, hooks into combat ticks.
- `CombatManager`: fixed-step combat tick dispatcher, damage resolution.

### Shared utilities
- `DirectionEnumHelper`: Vector2 to DirectionEnum conversion.
- `EntityExtensions`: range checks, closest entity helpers.

### Multiplayer transport (local loopback)
- `LocalClientPacketSender`, `LocalServerPacketReceiver`, `LocalServerPacketSender`: in-editor packet routing.

### Key design decisions
- Input is collected on client, sent to server, processed once per cycle.
- Server mutates entity state; client receives authoritative state via `EntitySpawnPacket`.
- Entity snapshots are broadcast per-map to reduce unnecessary network traffic.
- Combat tick is local/instance-authoritative; server-authoritative combat is a future goal.
