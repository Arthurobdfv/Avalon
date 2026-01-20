# Contributing to Avalon

Thank you for your interest in contributing to the Avalon MMO prototype! This guide will help you get started with contributing code, documentation, and features to the project.

## Quick Start

1. **Understand the project** - Read the [README](../../README.md) and [Architecture Overview](../architecture/README.md)
2. **Check existing issues** - Look for open issues or create a new one
3. **Make your changes** - Follow the guidelines in this document
4. **Submit a PR** - Create a pull request with a clear description
5. **Iterate** - Address feedback and update your changes

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [How Can I Contribute?](#how-can-i-contribute)
- [Development Workflow](#development-workflow)
- [Coding Standards](#coding-standards)
- [Documentation Standards](#documentation-standards)
- [Pull Request Process](#pull-request-process)
- [Getting Help](#getting-help)

## Code of Conduct

### Our Pledge

This is a learning project and everyone is welcome to contribute, regardless of experience level. Be respectful, helpful, and constructive in all interactions.

### Expected Behavior

- Be welcoming to newcomers
- Provide constructive feedback
- Focus on what is best for the project
- Show empathy towards other contributors

## How Can I Contribute?

### Reporting Bugs

**Before submitting a bug report:**
- Check existing issues to avoid duplicates
- Collect information about the bug (Unity version, platform, steps to reproduce)

**Submit a bug report:**
1. Use the bug report template
2. Provide a clear, descriptive title
3. Include steps to reproduce
4. Describe expected vs actual behavior
5. Add screenshots or logs if applicable

### Suggesting Features

**Before suggesting a feature:**
- Check the [Features Index](../features/README.md) for planned features
- Review existing feature requests in issues

**Submit a feature request:**
1. Use the feature request template
2. Explain the problem the feature solves
3. Describe your proposed solution
4. Consider alternative solutions
5. Add mockups or examples if applicable

### Contributing Code

**Good first issues:**
- Look for issues tagged `good first issue`
- Documentation improvements
- Small bug fixes
- Test additions

**Larger contributions:**
- Discuss in an issue before starting work
- Break large features into smaller PRs
- Follow the architecture patterns
- Add tests for new functionality

### Contributing Documentation

See the [Documentation Guide](documentation-guide.md) for detailed instructions on:
- Writing and updating documentation
- Documentation structure and templates
- File naming conventions
- Where to add different types of docs

## Development Workflow

### Setting Up Your Environment

1. **Fork and clone the repository:**
```bash
git clone https://github.com/YOUR_USERNAME/Avalon.git
cd Avalon
```

2. **Open in Unity:**
   - Use the Unity version specified in `ProjectSettings/ProjectVersion.txt`
   - Let Unity import all assets

3. **Create a feature branch:**
```bash
git checkout -b feature/your-feature-name
```

### Making Changes

1. **Make focused changes:**
   - Keep changes small and focused
   - One feature/fix per PR
   - Don't mix refactoring with new features

2. **Follow coding standards:**
   - See [Code Style Guide](code-style.md)
   - Match existing code style
   - Use meaningful names

3. **Test your changes:**
   - Run the game in Unity Editor
   - Test the specific feature you changed
   - Verify no regressions

4. **Update documentation:**
   - Update relevant docs in `docs/`
   - Add entry to `CHANGELOG.md` under `Unreleased`
   - Update feature status if applicable

### Committing Changes

**Commit message format:**

Use Conventional Commits style:
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
- `style` - Code style (formatting, no logic change)
- `refactor` - Code refactoring
- `test` - Adding tests
- `chore` - Maintenance tasks

**Examples:**
```
feat(combat): add critical hit system

Implements critical hits with 15% chance and 2x damage multiplier.
Adds CritChance and CritMultiplier to CombatBaseStats.

- Added critical hit calculation to CombatManager
- Updated CombatBaseStats ScriptableObject
- Added unit tests for crit calculation

Related to #42
```

```
docs(architecture): add combat system documentation

Created comprehensive combat system architecture doc.
Explains tick system, damage resolution, and event flow.
```

### Testing Changes

**Manual testing:**
1. Open relevant Unity scene
2. Enter Play mode
3. Exercise the changed code paths
4. Verify expected behavior
5. Check for errors in Console

**What to test:**
- Your new feature works as expected
- Existing features still work (no regressions)
- Edge cases and error conditions
- Performance (no major slowdowns)

## Coding Standards

For detailed coding conventions, see [Code Style Guide](code-style.md).

**Quick reference:**
- Use PascalCase for classes, methods, properties
- Use _camelCase for private fields
- Add XML comments for public APIs
- Keep methods small and focused
- Use meaningful variable names

## Documentation Standards

For detailed documentation guidelines, see [Documentation Guide](documentation-guide.md).

**Quick reference:**
- Use kebab-case for file names
- Feature docs in `docs/features/`
- Architecture docs in `docs/architecture/`
- Follow the documentation templates
- Update `CHANGELOG.md` for notable changes

## Pull Request Process

### Before Submitting

- [ ] Code follows the style guide
- [ ] Changes are tested and working
- [ ] Documentation is updated
- [ ] CHANGELOG.md is updated
- [ ] Commit messages follow convention
- [ ] PR is focused and not too large

### Submitting a PR

1. **Push your branch:**
```bash
git push origin feature/your-feature-name
```

2. **Create PR on GitHub:**
   - Use the PR template
   - Provide a clear description
   - Reference related issues
   - Add screenshots for visual changes

3. **Fill out PR description:**
   - What does this PR do?
   - Why is this change needed?
   - How was it tested?
   - Any breaking changes?
   - Related issues/PRs?

### PR Template

```markdown
## Description
Brief description of what this PR does.

## Motivation
Why is this change needed?

## Changes
- List of specific changes
- File paths affected
- Key components modified

## Testing
How was this tested?
- [ ] Manual testing in Unity Editor
- [ ] Tested in standalone build
- [ ] No console errors or warnings

## Screenshots
(If applicable)

## Checklist
- [ ] Code follows style guide
- [ ] Documentation updated
- [ ] CHANGELOG.md updated
- [ ] Tests passing
- [ ] No breaking changes (or documented)

## Related Issues
Closes #123
Related to #456
```

### Review Process

1. **Automated checks:**
   - Build succeeds (when CI is implemented)
   - No merge conflicts

2. **Code review:**
   - Maintainer reviews code
   - May request changes
   - Discussion in PR comments

3. **Address feedback:**
   - Make requested changes
   - Push updates to same branch
   - Re-request review when ready

4. **Merge:**
   - PR is merged to main branch
   - Branch is deleted
   - Issue is closed automatically

## Getting Help

### Resources

- **Documentation:** Start with [docs/README.md](../README.md)
- **Architecture:** See [docs/architecture/README.md](../architecture/README.md)
- **Features:** See [docs/features/README.md](../features/README.md)

### Asking Questions

- **GitHub Issues:** For bugs and feature requests
- **GitHub Discussions:** For questions and general discussion
- **PR Comments:** For questions about specific code

### Tips for New Contributors

1. **Start small:** Pick a `good first issue`
2. **Ask questions:** Don't hesitate to ask for help
3. **Read the docs:** Most answers are in the documentation
4. **Learn by example:** Look at existing code and PRs
5. **Be patient:** Reviews may take time

## Project Structure

Understanding the project layout helps you navigate:

```
Avalon/
├── .github/              # GitHub templates and workflows
├── Assets/               # Unity assets
│   ├── Animations/       # Animation clips and controllers
│   ├── Prefabs/          # Character and UI prefabs
│   ├── Scenes/           # Unity scenes
│   ├── Scripts/          # C# source code
│   │   ├── Character/    # Character systems
│   │   ├── Input/        # Input handling
│   │   ├── UI/           # UI components
│   │   └── Editor/       # Editor tools
│   └── Sprites/          # Textures and sprites
├── docs/                 # Documentation
│   ├── architecture/     # Technical docs
│   ├── features/         # Feature docs
│   └── contributing/     # Contribution guides
├── ProjectSettings/      # Unity project settings
├── README.md             # Project overview
└── CHANGELOG.md          # Project history
```

## Attribution

Contributors will be credited in:
- Git commit history
- CHANGELOG.md for significant contributions
- Special thanks section in README (for major features)

Thank you for contributing to Avalon! 🎮
