# Copilot Instructions

## Purpose

This file is the canonical guide for automated tools (including Copilot) and humans to follow when creating prompts, commits, and pull requests for the Avalon repository.

## Repository Summary

- **Name:** `Avalon` - Unity-based MMO prototype and learning project
- **Purpose:** Proof-of-concept for MMO mechanics, server ideas, and Unity development practice
- **Status:** Prototype / Work in progress
- **Unity Version:** See `ProjectSettings/ProjectVersion.txt`
- **Target Framework:** .NET Framework 4.7.1

## Documentation Structure

All documentation now lives under the `docs/` folder with clear organization:

### Documentation Organization

```
docs/
├── README.md                    # Documentation hub - start here
├── architecture/                # Technical system documentation
│   ├── README.md               # Architecture overview
│   ├── combat-system.md        # Combat mechanics
│   ├── entity-management.md    # Entity registration and queries
│   ├── input-system.md         # Input handling
│   └── animation-system.md     # Animation coordination
├── features/                    # User-facing feature documentation
│   ├── README.md               # Feature index
│   ├── character-animation.md  # Character animations
│   ├── character-movement.md   # Player movement
│   ├── combat-flow.md          # Combat interactions
│   ├── enemy-behavior.md       # Enemy AI
│   ├── sprite-pipeline.md      # Sprite-to-animation automation
│   └── build-pipeline.md       # CI/CD (planned)
├── contributing/                # Contribution guides
│   ├── README.md               # How to contribute
│   ├── documentation-guide.md  # Documentation standards
│   └── code-style.md           # Coding conventions
└── changelog/                   # Points to root CHANGELOG.md
```

### Root Level Files

- `README.md` - Project overview with links to documentation
- `CHANGELOG.md` - Project history (kept at root for visibility)

## High-Level Architecture

- **Unity Projects:** `Assembly-CSharp` (runtime), `Assembly-CSharp-Editor` (editor tools)
- **Target Framework:** .NET Framework 4.7.1
- **Key Systems:**
  - Combat System - Event-driven combat with tick-based timing
  - Entity Management - Centralized character registration
  - Input System - Player input mapping and handling
  - Animation System - Sprite-based character animations
  - Sprite Pipeline - Automated sprite-to-animation (in progress)
  - Enemy Behavior - AI targeting and behavior management


## Documentation Standards

### File Naming

- Use **kebab-case.md** for all documentation files
- Feature docs: `docs/features/<feature-name>.md`
- Architecture docs: `docs/architecture/<system-name>.md`

### Document Types

**Feature Documentation** (`docs/features/`):
- Describes **what** the game can do (user-facing capabilities)
- Focus on user experience and functionality
- Include usage examples and screenshots
- Link to related architecture docs for technical details

**Architecture Documentation** (`docs/architecture/`):
- Describes **how** systems work (technical implementation)
- Focus on design decisions and implementation
- Include code examples and diagrams
- Link to related features

**Contributing Documentation** (`docs/contributing/`):
- Guides for contributors
- Process documentation
- Coding standards and conventions

### Adding New Documentation

1. **Determine type:** Feature (what) vs Architecture (how)
2. **Create file** in appropriate `docs/` subfolder using kebab-case
3. **Follow template** (see `docs/contributing/documentation-guide.md`)
4. **Update index** - Add entry to relevant `README.md`
5. **Update CHANGELOG** - Add note under `Unreleased`

## Workflow Rules

### Feature Lifecycle

1. Create feature doc in `docs/features/<feature-name>.md`
2. Create architecture doc in `docs/architecture/<system-name>.md` if needed
3. Update `docs/features/README.md` to include feature
4. Update `docs/architecture/README.md` if new system added
5. Add entry to `CHANGELOG.md` under `Unreleased`
6. Link related documentation

### Commit Guidance

Use Conventional Commits format:

```
type(scope): short description

Longer description if needed

- Bullet points for details
- Related to #issue-number
```

**Types:**
- `feat` - New feature
- `fix` - Bug fix
- `docs` - Documentation only
- `refactor` - Code refactoring
- `style` - Code style (formatting)
- `test` - Adding tests
- `chore` - Maintenance tasks

**Examples:**
```
feat(combat): add critical hit system
fix(animation): correct direction mapping for diagonal movement
docs(architecture): add combat system documentation
```

### Pull Request Guidance

Include in PR description:
- Summary of changes
- Related issues/docs links
- Testing steps
- Affected files
- List of docs updated
- Screenshots for UI/visual changes

### Documentation Generation

When automated tools update documentation:
- Inspect `git status --porcelain --untracked-files=all`
- Read changed files and extract runtime-relevant changes
- Update or create appropriate documentation
- Add "Recent Changes" section if documenting unstaged work
- Update `CHANGELOG.md` under `Unreleased`
- Use Conventional Commits for commit messages

**Important:** Avoid non-ASCII or special punctuation characters (smart quotes, em-dashes, non-breaking spaces) in generated Markdown. Use ASCII characters and simple punctuation (straight quotes, hyphen-minus `-`, plain spaces).

## Coding Conventions

### Naming

- **Classes/Structs:** PascalCase (`CombatManager`, `HealthData`)
- **Methods/Properties:** PascalCase (`PerformAttack`, `CurrentHealth`)
- **Private Fields:** `_camelCase` with underscore (`_currentHealth`, `_target`)
- **Local Variables:** camelCase (`damageAmount`, `target`)
- **Constants:** PascalCase (`MaxPlayers`, `AttackCooldown`)
- **Events:** Suffix with `Handler` (`OnCombatTickHandler`)

### Code Style

- Follow existing code patterns
- Keep changes minimal and consistent
- Use meaningful variable names
- Add XML comments for public APIs
- Guard editor-only code with `#if UNITY_EDITOR`
- Prefer existing helpers over new dependencies

See `docs/contributing/code-style.md` for complete guidelines.

## PR Message Requests (Automation Directive)

When user asks Copilot to produce a PR message:

1. **Summarize diff** between source and target branches:
   - List files added, modified, removed
   - One-line summary per changed file or area
   - Include commit count and intent summary

2. **Produce PR title and body:**
   - Use Conventional Commits style
   - Include testing steps
   - List files of interest

3. **Format as Markdown code block:**
   - Ready for copy/paste to GitHub UI

4. **Include CHANGELOG entry:**
   - Suggested one-line addition under `Unreleased`

5. **Suggest splitting if needed:**
   - If diff is large or unrelated changes
   - Explain why briefly

## Automation Bookkeeping

- Update this file when documentation structure changes
- Keep file references current when files are renamed/removed
- Record automated documentation steps in commit messages
- Maintain consistency between documentation and code

## Related Documentation

For detailed guidelines, see:
- **[Documentation Guide](../docs/contributing/documentation-guide.md)** - How to write and update documentation
- **[Code Style Guide](../docs/contributing/code-style.md)** - Coding conventions
- **[Contributing Guide](../docs/contributing/README.md)** - How to contribute
- **[Documentation Hub](../docs/README.md)** - Documentation entry point
