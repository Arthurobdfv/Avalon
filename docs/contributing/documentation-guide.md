# Documentation Guide

This guide explains how to create, update, and maintain documentation for the Avalon project. Following these guidelines ensures consistent, discoverable, and useful documentation.

## Documentation Philosophy

- **Clear and Concise** - Write for your audience, avoid jargon
- **Actionable** - Include examples and usage instructions
- **Maintainable** - Keep docs close to code, update with changes
- **Discoverable** - Proper structure and cross-linking
- **Current** - Outdated docs are worse than no docs

## Documentation Structure

All documentation lives under the `docs/` folder:

```
docs/
├── README.md                    # Documentation hub (start here)
├── architecture/                # Technical system docs
│   ├── README.md               # Architecture overview
│   └── <system-name>.md        # Individual system docs
├── features/                    # User-facing feature docs
│   ├── README.md               # Feature index
│   └── <feature-name>.md       # Individual feature docs
├── contributing/                # Contribution guides
│   ├── README.md               # How to contribute
│   ├── documentation-guide.md  # This file
│   └── code-style.md           # Coding conventions
└── changelog/                   # Points to root CHANGELOG.md
```

## Types of Documentation

### Architecture Documentation (`docs/architecture/`)

**Purpose:** Explain **how** technical systems work

**Audience:** Developers who need to understand or modify systems

**When to create:**
- New technical system is implemented
- Existing system is significantly refactored
- Complex technical decisions need explanation

**Examples:**
- Combat System Architecture
- Entity Management Architecture
- Input System Architecture

### Feature Documentation (`docs/features/`)

**Purpose:** Describe **what** user-facing capabilities exist

**Audience:** Players, designers, and developers wanting overview

**When to create:**
- New user-facing feature is added
- Existing feature is significantly enhanced
- Feature needs user guidance or examples

**Examples:**
- Character Movement
- Combat Flow
- Build Pipeline

### Contributing Documentation (`docs/contributing/`)

**Purpose:** Guide contributors on how to participate

**Audience:** Anyone wanting to contribute code or docs

**When to create:**
- Process or workflow changes
- New coding standards adopted
- Common contributor questions arise

**Examples:**
- Contributing Guide
- Documentation Guide
- Code Style Guide

## File Naming Conventions

### Use kebab-case

All documentation files use lowercase with hyphens:

**Good:**
- `combat-system.md`
- `character-movement.md`
- `documentation-guide.md`

**Bad:**
- `CombatSystem.md` (PascalCase)
- `Combat_System.md` (snake_case)
- `combatsystem.md` (no separators)

### Be Descriptive

File names should clearly indicate content:

**Good:**
- `input-system.md` - Clear what it documents
- `sprite-pipeline.md` - Specific feature name

**Bad:**
- `system.md` - Too vague
- `stuff.md` - Not descriptive

## Document Templates

### Feature Document Template

```markdown
# Feature Name

- **Status:** Draft | In Progress | Implemented | Deprecated
- **Summary:** One-line description

## Overview
What this feature does and why it exists.

## User Experience
How users interact with this feature.

## Implementation
Current implementation details and key components.

## Architecture
Link to relevant architecture documentation.

## Usage
How to use or test this feature.

## Known Limitations
Current limitations or issues.

## Next Steps
What remains to be done.

## Related Documentation
Links to related docs.
```

### Architecture Document Template

```markdown
# System Name

## Overview
What this system does and why it exists.

## Purpose
Goals and responsibilities of this system.

## Core Components
Main classes/files with brief descriptions.

## How It Works
Technical explanation with diagrams/flow.

## Integration Points
How this system connects to other systems.

## Usage Guide
How to use this system (setup, configuration, examples).

## Design Decisions
Why this approach was chosen.

## Known Limitations
Current limitations and constraints.

## Future Enhancements
Planned improvements.

## File Reference
List of relevant source files.

## Related Documentation
Links to related docs.
```

## Writing Guidelines

### Start with Context

Begin with a brief overview that answers:
- What is this?
- Why does it exist?
- Who is this for?

### Use Clear Headers

Organize with descriptive headers:

**Good:**
```markdown
## How Combat Timing Works
## Setting Up Character Animations
## Known Limitations
```

**Bad:**
```markdown
## Details
## Stuff
## Other
```

### Include Code Examples

Show, don't just tell:

```csharp
// Good: Show actual code
public void Attack(CombatCharacter target) {
    if (IsInRange(target, AttackRange)) {
        target.TakeDamage(AttackDamage);
    }
}
```

### Link Liberally

Cross-reference related documentation:

```markdown
See [Combat System Architecture](../architecture/combat-system.md)
Related: [Entity Management](../architecture/entity-management.md)
```

### Use Lists and Tables

Make information scannable:

**Lists for steps:**
1. Create combat stats
2. Assign to character
3. Set target
4. Combat runs automatically

**Tables for comparisons:**
| Feature | Status | Priority |
|---------|--------|----------|
| Combat  | Done   | High     |
| AI      | WIP    | Medium   |

### Add Visual Aids

Include diagrams for complex flows:

```
Input System
    |
    v
Character Controller
    |
    v
Animation System
```

### Explain Decisions

Document the "why" not just the "what":

**Good:**
```markdown
We use event-driven architecture because it decouples
systems and makes multiplayer synchronization easier.
```

**Bad:**
```markdown
The system uses events.
```

## Maintenance Guidelines

### Update with Code Changes

When changing code, update related docs:

1. Find affected documentation
2. Update implementation details
3. Update examples if needed
4. Check cross-references
5. Update CHANGELOG.md

### Mark Outdated Content

If docs become outdated and you can't update immediately:

```markdown
> **Note:** This section may be outdated as of v0.3.
> The system was refactored in PR #123.
```

### Remove or Archive

Don't keep old docs indefinitely:
- Remove docs for deprecated features
- Archive historical docs if needed
- Update links to removed docs

### Regular Reviews

Periodically review docs for:
- Accuracy
- Broken links
- Outdated screenshots
- Missing new features

## Adding New Documentation

### Step-by-Step Process

1. **Determine type:**
   - Feature (user-facing) → `docs/features/`
   - Architecture (technical) → `docs/architecture/`
   - Contributing (process) → `docs/contributing/`

2. **Create file:**
   - Use kebab-case filename
   - Follow appropriate template
   - Fill in all sections

3. **Update index:**
   - Add to relevant README.md
   - Include one-line summary
   - Link to new document

4. **Update CHANGELOG:**
   - Add entry under `Unreleased`
   - Note the new documentation

5. **Cross-reference:**
   - Add links from related docs
   - Update navigation as needed

### Example: Adding Feature Doc

```bash
# 1. Create file
docs/features/quest-system.md

# 2. Add to docs/features/README.md
### Quest System
Implement player quests and objectives.
See [Quest System](quest-system.md)

# 3. Update CHANGELOG.md
- Added documentation for Quest System feature

# 4. Link from related docs
# In docs/features/character-progression.md:
Related: [Quest System](quest-system.md)
```

## Documentation Workflow (Copilot)

This section consolidates documentation patterns for automated tools.

### When to Document Code Changes

Update documentation when you:
- Add a new feature or system
- Significantly modify existing functionality
- Change public APIs or interfaces
- Fix bugs that affect documented behavior
- Refactor in ways that affect architecture

### Copilot Documentation Pattern

When Copilot or automated tools document changes:

1. **Inspect repository status:**
```bash
git status --porcelain --untracked-files=all
```

2. **Read changed files:**
   - Identify modified and new files
   - Extract runtime-relevant changes

3. **Determine target docs:**
   - Find existing related documentation
   - Or create new doc in appropriate folder

4. **Update or create docs:**
   - Add "Recent Changes" section if relevant
   - Update implementation details
   - Refresh examples if needed

5. **Update CHANGELOG.md:**
   - Add entry under `Unreleased`
   - Summarize the change

6. **Generate commit message:**
   - Use Conventional Commits format
   - Reference affected docs and files

### Commit Message Format

For documentation changes:

```
docs(scope): short description

Longer description if needed.

- Updated architecture/combat-system.md
- Added examples for attack timing
- Refreshed diagrams

Related to #issue-number
```

### Automation Request Format

When requesting automated documentation:

```
"Document the changes in [files/feature]"
"Create documentation for the new [system/feature]"
"Update docs for [component] with recent changes"
```

## Style Guide

### Language

- **Active voice:** "The system processes input"
- **Present tense:** "The manager handles events"
- **Imperative for instructions:** "Create a new file"
- **Second person for guides:** "You can configure..."

### Formatting

- **Bold** for emphasis: `**important**`
- *Italic* for terminology: `*term*`
- `Code` for technical terms: backticks
- > Blockquotes for notes/warnings

### Code Blocks

Always specify language:

```markdown
```csharp
public class Example {
    // C# code
}
\```

```bash
git commit -m "message"
\```

```json
{
  "key": "value"
}
\```
```

### Links

Use relative links for internal docs:

```markdown
[Architecture Overview](../architecture/README.md)
[Features Index](../features/README.md)
```

Use absolute links for external resources:

```markdown
[Unity Documentation](https://docs.unity3d.com/)
```

## Common Mistakes

### Avoid These

❌ **Mixing concepts in one doc**
- Keep features and architecture separate
- One system or feature per doc

❌ **Skipping examples**
- Always include usage examples
- Show actual code, not pseudocode

❌ **Forgetting to update CHANGELOG**
- Always update CHANGELOG.md
- Note documentation additions

❌ **Broken links**
- Test all links before committing
- Use relative paths for internal links

❌ **No cross-references**
- Link to related documentation
- Build a documentation web, not silos

❌ **Overly technical features docs**
- Keep feature docs user-focused
- Move technical details to architecture docs

## Questions?

If you have questions about documentation:

1. Check existing docs as examples
2. Ask in GitHub issues/discussions
3. Reference this guide
4. When in doubt, ask before writing

## Related Documentation

- [Contributing Guide](README.md) - Overall contribution guidelines
- [Code Style Guide](code-style.md) - Coding conventions
- [Features Index](../features/README.md) - Example feature docs
- [Architecture Overview](../architecture/README.md) - Example architecture docs
