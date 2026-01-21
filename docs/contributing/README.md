# Contributing to Avalon

Thank you for your interest in contributing to the Avalon MMO prototype! This guide will help you get started.

## Project Overview

Avalon is a Unity-based MMO prototype created to:
- Validate game design ideas and MMO mechanics
- Practice Unity development and game server implementation
- Explore client-server architecture patterns

**Current Status:** Prototype / Work in Progress

## Ways to Contribute

### Code Contributions
- Implement new features
- Fix bugs
- Improve performance
- Add tests

### Documentation
- Improve existing documentation
- Add missing documentation
- Fix typos and clarify confusing sections
- Add code examples

### Testing
- Manual testing of features
- Report bugs and issues
- Suggest improvements

## Getting Started

### Prerequisites
- Unity 2021+ (check `ProjectSettings/ProjectVersion.txt` for exact version)
- Git for version control
- Basic understanding of C# and Unity

### Setting Up Development Environment

1. **Clone the repository:**
   ```bash
   git clone https://github.com/Arthurobdfv/Avalon.git
   cd Avalon
   ```

2. **Open in Unity:**
   - Launch Unity Hub
   - Add the project folder
   - Open the project

3. **Explore the codebase:**
   - Read [Documentation Hub](../README.md)
   - Review [Architecture Documentation](../architecture/README.md)
   - Check [Features Documentation](../features/README.md)

### Development Workflow

1. **Create a branch:**
   ```bash
   git checkout -b feature/your-feature-name
   ```

2. **Make your changes:**
   - Write code following [Code Style Guide](code-style.md)
   - Test your changes locally
   - Update documentation

3. **Commit your changes:**
   ```bash
   git add .
   git commit -m "feat(scope): description"
   ```
   
   See [Commit Message Guidelines](#commit-message-guidelines) below.

4. **Push to GitHub:**
   ```bash
   git push origin feature/your-feature-name
   ```

5. **Create a Pull Request:**
   - Go to GitHub repository
   - Click "New Pull Request"
   - Fill in PR template (see [Pull Request Guidelines](#pull-request-guidelines))
   - Request review

## Contribution Guidelines

### Code Quality

- **Follow the code style:** See [Code Style Guide](code-style.md)
- **Keep changes focused:** One feature/fix per PR
- **Write clean code:** Self-documenting with minimal comments
- **Avoid breaking changes:** Unless discussed and agreed upon

### Testing

- **Test locally:** Ensure your changes work in Unity Editor
- **Test multiplayer:** If applicable, test with local loopback
- **Manual testing:** Provide test steps in PR description
- **Automated tests:** Add if test infrastructure exists

### Documentation

- **Update affected docs:** When changing behavior
- **Follow documentation guide:** See [Documentation Guide](documentation-guide.md)
- **Use proper formatting:** Markdown, kebab-case filenames
- **Add examples:** Code snippets and usage examples

### Performance

- **Profile changes:** If affecting performance-critical paths
- **Avoid allocations:** In Update/FixedUpdate loops
- **Cache references:** Don't use GetComponent in Update
- **Consider multiplayer:** Network bandwidth and server load

## Commit Message Guidelines

We follow [Conventional Commits](https://www.conventionalcommits.org/) format:

```
<type>(<scope>): <subject>

<body>

<footer>
```

### Types

- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `refactor`: Code refactoring (no behavior change)
- `perf`: Performance improvements
- `test`: Adding or updating tests
- `chore`: Maintenance tasks

### Scopes

Common scopes in this project:
- `combat`: Combat system
- `multiplayer`: Networking and multiplayer
- `input`: Input handling
- `entity`: Entity management
- `animation`: Animation system
- `docs`: Documentation

### Subject

- Use imperative mood: "add feature" not "added feature"
- Keep under 72 characters
- No period at the end

### Body (optional)

- Explain what and why, not how
- Wrap at 72 characters
- Separate from subject with blank line

### Examples

```
feat(combat): add tick-based attack system

Implemented fixed-interval combat ticks with configurable
tick rate. Characters now attack based on AttackSpeed stat.

Files changed:
- Assets/Scripts/Character/Combat/CombatManager.cs
- Assets/Scripts/Character/CombatCharacter.cs
```

```
docs(architecture): add multiplayer architecture documentation

Created comprehensive documentation for the multiplayer packet
flow, observer system, and map-scoped delivery.

Files added:
- docs/architecture/multiplayer-architecture.md
```

```
fix(input): prevent duplicate movement updates per tick

Fixed race condition where movement could be applied multiple
times per server tick, causing jittery player movement.
```

## Pull Request Guidelines

### PR Title

Use the same format as commit messages:
```
feat(combat): add tick-based attack system
```

### PR Description

Include these sections:

**Summary:**
- What does this PR do?
- Why is it needed?

**Changes:**
- List of major changes
- Files added/modified/deleted

**Testing:**
- How to test the changes
- Expected behavior
- Edge cases tested

**Documentation:**
- Documentation files updated
- New docs added

**Related Issues:**
- Links to related issues/discussions
- Closes #123 (if applicable)

**Screenshots/Videos:**
- For visual changes (UI, gameplay)
- Before/after comparisons if applicable

### Example PR Description

```markdown
## Summary
Implements tick-based combat system with configurable interval.

## Changes
- Added CombatManager with tick system
- Modified CombatCharacter to use tick-based attacks
- Updated combat stats to include AttackSpeed
- Added health change events for UI integration

Files changed:
- `Assets/Scripts/Character/Combat/CombatManager.cs` (new)
- `Assets/Scripts/Character/CombatCharacter.cs` (modified)
- `Assets/Scripts/Character/Combat/CombatBaseStats.cs` (modified)

## Testing
1. Open SampleScene
2. Enter play mode
3. Set enemy as player target
4. Observe attack occurs every AttackSpeed seconds
5. Verify health decreases on target

## Documentation
- Updated `docs/architecture/combat-system.md`
- Updated `CHANGELOG.md`

## Related Issues
Implements #42
```

## Code Review Process

### For Contributors

1. **Respond to feedback:** Address reviewer comments
2. **Make requested changes:** Push additional commits
3. **Explain decisions:** If you disagree with feedback
4. **Be patient:** Reviews may take time

### For Reviewers

1. **Be constructive:** Suggest improvements, don't just criticize
2. **Check for:**
   - Code style adherence
   - Potential bugs
   - Performance issues
   - Security concerns
   - Documentation updates
3. **Approve when ready:** Or request changes with clear guidance

## Branching Strategy

### Branch Naming

- `feature/feature-name` - New features
- `fix/bug-description` - Bug fixes
- `docs/what-documenting` - Documentation
- `refactor/what-refactoring` - Refactoring

### Main Branches

- `main` - Stable release branch
- `dev` - Development branch for integration
- Feature branches - Individual features/fixes

### Workflow

1. Branch from `dev`
2. Make changes
3. PR to `dev`
4. After review and approval, merge to `dev`
5. Periodically merge `dev` to `main` for releases

## Issue Reporting

### Bug Reports

Include:
- **Description:** What's wrong?
- **Steps to reproduce:** How to trigger the bug
- **Expected behavior:** What should happen
- **Actual behavior:** What actually happens
- **Unity version:** From `ProjectSettings/ProjectVersion.txt`
- **Screenshots/logs:** If applicable

### Feature Requests

Include:
- **Description:** What feature do you want?
- **Use case:** Why is it needed?
- **Proposed solution:** How might it work?
- **Alternatives:** Other approaches considered

## Communication

### Where to Ask Questions

- **GitHub Issues:** Bug reports, feature requests
- **GitHub Discussions:** General questions, ideas
- **Pull Request comments:** Code-specific questions

### Response Time

This is a learning project, so response times may vary. Please be patient!

## AI Usage Disclaimer

Some documentation in this project was generated or assisted by AI tools to enhance clarity and organization. Code is written by humans, not AI.

## License

This is a prototype project for educational and validation purposes.

## Additional Resources

- [Code Style Guide](code-style.md) - Coding conventions
- [Documentation Guide](documentation-guide.md) - Documentation conventions
- [Architecture Documentation](../architecture/README.md) - Technical design
- [Features Documentation](../features/README.md) - Feature guides

## Questions?

If you have questions not covered in this guide:
1. Check existing documentation
2. Search GitHub Issues/Discussions
3. Create a new Discussion or Issue

## Thank You!

We appreciate your contributions to Avalon. Every contribution, no matter how small, helps improve the project!
