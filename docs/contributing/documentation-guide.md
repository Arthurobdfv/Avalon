# Documentation Guide

This guide explains how to create, update, and maintain documentation for the Avalon project.

## Documentation Structure

The documentation is organized into clear sections:

```
docs/
├── README.md                    # Documentation hub (start here)
├── architecture/                # Technical architecture docs
├── features/                    # User-facing feature docs
├── contributing/                # Contribution guidelines (this section)
└── changelog/                   # Points to root CHANGELOG.md
```

## Documentation Types

### Architecture Documentation (`docs/architecture/`)

**Purpose:** Technical design and implementation details for core systems.

**When to Create:**
- Implementing a new core system (combat, networking, etc.)
- Significant refactoring of existing architecture
- Need to explain complex technical decisions

**Contents:**
- System overview and responsibilities
- Component descriptions and interactions
- Implementation details and patterns
- Integration points with other systems
- Performance considerations
- Known limitations and future improvements

**Example:** `docs/architecture/combat-system.md`, `docs/architecture/multiplayer-architecture.md`

### Feature Documentation (`docs/features/`)

**Purpose:** User-facing feature descriptions and usage guides.

**When to Create:**
- Adding a new gameplay feature
- Implementing user-facing functionality
- Need to explain how to use a feature

**Contents:**
- Feature status and summary
- How the feature works
- Usage instructions and examples
- Integration with other features
- Current limitations
- Future enhancements

**Example:** `docs/features/combat-flow.md`, `docs/features/character-movement.md`

### Contributing Documentation (`docs/contributing/`)

**Purpose:** Guidelines for contributing to the project.

**Includes:**
- This documentation guide
- Code style conventions
- How to contribute guide

## Documentation Workflow

### 1. Creating New Documentation

**Step 1: Determine the type**
- Architecture doc for technical systems
- Feature doc for user-facing functionality

**Step 2: Choose location**
- Architecture: `docs/architecture/<system-name>.md`
- Feature: `docs/features/<feature-name>.md`

**Step 3: Use kebab-case naming**
- Good: `multiplayer-architecture.md`, `combat-flow.md`
- Bad: `MultiplayerArchitecture.md`, `Combat_Flow.md`

**Step 4: Follow the template**
See "Documentation Templates" section below.

**Step 5: Link from index**
- Add entry to `docs/architecture/README.md` or `docs/features/README.md`
- Link from `docs/README.md` if major addition
- Reference from related docs

**Step 6: Update changelog**
Add entry to root `CHANGELOG.md` under "Unreleased" section.

### 2. Updating Existing Documentation

**When to Update:**
- Code changes affect documented behavior
- New features added to existing systems
- Bugs fixed or limitations resolved
- Architecture refactored

**How to Update:**
1. Locate the relevant documentation file
2. Update affected sections (keep changes minimal)
3. Add "Recent Changes" or "Unstaged Changes" section if documenting work-in-progress
4. Update `CHANGELOG.md` with brief note
5. Verify cross-references are still accurate

### 3. Documenting Code Changes

When making code changes, update documentation:

**Inspect Changes:**
```bash
git status --porcelain --untracked-files=all
git diff
```

**Identify Affected Docs:**
- Which systems/features are affected?
- Which docs need updates?
- Are new docs needed?

**Update Documentation:**
1. Edit existing docs with new behavior
2. Create new docs if adding major systems
3. Add cross-references as needed

**Document Runtime Changes:**
For significant changes, add an "Unstaged / Recent runtime changes" section:

```markdown
## Unstaged / Recent runtime changes

Summary of recent edits:
- `Assets/Scripts/Character/Combat/CombatManager.cs`: Added tick-based combat loop
- `Assets/Scripts/Character/CombatCharacter.cs`: Implemented attack timing
- Behavior: Characters now attack on fixed intervals based on AttackSpeed stat
```

**Update Changelog:**
Add entry to `CHANGELOG.md` under "Unreleased":

```markdown
## Unreleased

### Features & Updates
- Combat: Implemented tick-based attack timing with configurable intervals
```

### 4. Commit Messages for Documentation

Use conventional commit format:

```
docs(<scope>): <imperative summary>

<optional body with details>
<list of files changed>
```

**Examples:**

```
docs(combat): add tick-based combat architecture

- Documented CombatManager tick system
- Added attack timing flow diagrams
- Updated combat-system.md with multiplayer notes

Files updated:
- docs/architecture/combat-system.md
- CHANGELOG.md
```

```
docs(features): add enemy behavior documentation

Created new feature doc for enemy AI targeting.

Files added:
- docs/features/enemy-behavior.md

Files updated:
- docs/features/README.md
- CHANGELOG.md
```

## Documentation Templates

### Architecture Document Template

```markdown
# [System Name] Architecture

## Overview
Brief description of the system and its purpose.

## Core Components

### [Component Name]
**Responsibilities:**
- List of what this component does

**Implementation Details:**
- How it works
- Key classes and methods

**Location:** `Assets/Scripts/...`

## [Additional sections as needed]

## Usage
How to use this system.

## Integration Points
How this system connects with others.

## Known Limitations
Current issues and constraints.

## Future Improvements
Planned enhancements.

## Related Documentation
Links to related docs.
```

### Feature Document Template

```markdown
# [Feature Name]

## Status
Implemented / In Progress / Planned

## Summary
One-line description of the feature.

## Overview
Detailed explanation of what the feature does.

## How It Works
Step-by-step explanation of feature behavior.

## Usage Example
Code or instruction examples.

## Configuration
Settings and parameters.

## Current Limitations
Known issues.

## Future Improvements
Planned enhancements.

## Files of Interest
- List of relevant source files

## Related Documentation
Links to architecture and other features.
```

## Special Documentation Sections

### Unstaged / Recent Runtime Changes

Use this section when documenting work-in-progress:

```markdown
## Unstaged / Recent runtime changes

Summary: [brief description of changes]

Changed files and highlights:
- `path/to/file.cs`: [what changed]
- `path/to/other.cs`: [what changed]

Behavioral notes:
- [explicit behavior changes]

Testing notes:
- [how to test the changes]
```

This section should be:
- Added when documenting incomplete work
- Removed when work is completed and committed
- Used to help reviewers understand recent changes

## Cross-Referencing Documentation

### Linking Between Docs

Use relative paths for internal links:

```markdown
See [Combat System](../architecture/combat-system.md) for details.
See [Enemy Behavior](enemy-behavior.md) for AI targeting.
```

### Linking to Code

Reference code files by full path:

```markdown
**Location:** `Assets/Scripts/Character/Combat/CombatManager.cs`
```

### Linking to External Resources

Use absolute URLs for external links:

```markdown
See [Unity Documentation](https://docs.unity3d.com/) for more info.
```

## Documentation Best Practices

### Writing Style

1. **Be concise and clear**
   - Use simple language
   - Avoid jargon where possible
   - Explain technical terms

2. **Use imperative mood**
   - Good: "Create a new file"
   - Bad: "You should create a new file"

3. **Include examples**
   - Code snippets
   - Usage examples
   - Diagrams (ASCII art is fine)

4. **Keep it current**
   - Update docs when code changes
   - Remove outdated information
   - Mark deprecated features

### Formatting

1. **Use headings effectively**
   - `#` for document title
   - `##` for major sections
   - `###` for subsections

2. **Use lists for clarity**
   - Bullet points for unordered items
   - Numbered lists for sequential steps

3. **Use code blocks**
   ```markdown
   ```csharp
   // Code example here
   ```
   ```

4. **Use tables for comparisons**
   ```markdown
   | Feature | Status | Notes |
   |---------|--------|-------|
   | Combat  | Done   | Basic |
   ```

### Character Encoding

**Important:** Use only ASCII characters in documentation.

**Avoid:**
- Smart quotes (" " ' ')
- Em-dashes (—)
- Non-breaking spaces
- Other Unicode symbols

**Use instead:**
- Straight quotes (" ')
- Hyphens (-)
- Regular spaces

This prevents encoding issues in static site builds and tools.

## Copilot Automation

### Requesting Documentation from Copilot

When asking Copilot to document changes:

**Format:**
```
Copilot, document and commit the unstaged changes for [feature/system].
Create a [single/multiple] commit and provide the commit message.
```

**Example:**
```
Copilot, document and commit the unstaged changes for combat system.
Create a single commit and provide the commit message.
```

### Copilot Documentation Process

When Copilot documents changes, it will:

1. Inspect repository status (`git status --porcelain`)
2. Read changed files
3. Summarize runtime-relevant changes
4. Update or create documentation pages
5. Add "Unstaged / Recent runtime changes" section if appropriate
6. Create conventional commit message
7. Optionally provide PR description

### Copilot Commit Message Format

Copilot generates commit messages following this pattern:

```
<type>(<scope>): <summary>

<body with details>

Files changed:
- path/to/file1
- path/to/file2

CHANGELOG entry: [suggested addition]
```

**Types:** `feat`, `fix`, `docs`, `chore`, `refactor`

### PR Message Requests

To get a PR message from Copilot:

```
Copilot, produce a PR message for these changes.
```

Copilot will provide:
- Diff summary (files added/modified/removed)
- Conventional PR title and body
- Testing steps
- CHANGELOG entry suggestion
- Markdown code block ready for copy/paste

## Documentation Maintenance

### Regular Reviews

Periodically review documentation for:
- Outdated information
- Broken links
- Missing sections
- Inconsistent formatting

### Version Updates

When releasing a version:
1. Move "Unreleased" section in `CHANGELOG.md` to a versioned section
2. Update status in feature docs (In Progress → Implemented)
3. Archive old documentation if major refactoring occurs

### Cleanup

Remove or archive:
- Documentation for removed features
- Outdated architecture docs after refactoring
- Superseded guides

## Tools and Automation

### Documentation Tools

- **Markdown editors:** VS Code, Typora, etc.
- **Link checkers:** markdown-link-check
- **Spell checkers:** Built into editors or hunspell

### Automated Checks

Consider adding:
- Markdown linting (markdownlint)
- Link validation in CI
- Spell checking in CI

## Getting Help

### Questions About Documentation

- Check existing documentation first
- Ask in project discussions/issues
- Review recent commits for examples

### Documentation Issues

If you find issues in documentation:
1. Open an issue describing the problem
2. Suggest improvements
3. Submit a PR with fixes

## Related Documentation

- [How to Contribute](README.md) - General contribution guidelines
- [Code Style](code-style.md) - Coding conventions
- [Architecture Documentation](../architecture/README.md) - Technical docs
- [Features Documentation](../features/README.md) - Feature docs
