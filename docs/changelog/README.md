# Changelog

The project changelog is maintained at the repository root level for visibility.

See: [`/CHANGELOG.md`](../../CHANGELOG.md)

## Why Root Level?

The changelog is kept at root level because:
1. **Visibility:** Easy to find for contributors and users
2. **Convention:** Standard practice in open source projects
3. **Git integration:** Tools like GitHub automatically highlight root CHANGELOG.md

## Changelog Format

We follow the [Keep a Changelog](https://keepachangelog.com/) format with an "Unreleased" section for ongoing work.

### Structure

```markdown
# Changelog

## Unreleased

### Features & Updates
- New features and significant updates

### Documentation
- Documentation changes

## [Version] - YYYY-MM-DD

### Features & Updates
- Released features

### Bug Fixes
- Fixed issues
```

## How to Update

When making changes:

1. **Add to Unreleased section** in root `CHANGELOG.md`
2. **Be concise:** One line per significant change
3. **Group by category:** Features, Bug Fixes, Documentation, etc.
4. **Link to docs:** Reference relevant documentation

### Example Entry

```markdown
## Unreleased

### Features & Updates
- Combat: Added tick-based attack system with configurable intervals
- Multiplayer: Implemented observer support with map filtering
- Entity Management: Added map-scoped entity queries

### Documentation
- Added comprehensive architecture documentation
- Documented multiplayer packet flow and observer system
- Created contributing guidelines
```

## Version Releases

When releasing a version:

1. **Create version section:** `## [1.0.0] - 2024-01-20`
2. **Move unreleased items** to version section
3. **Clear unreleased section:** Ready for next changes
4. **Tag in git:** `git tag v1.0.0`

## Related Documentation

- [Contributing Guide](../contributing/README.md) - How to contribute
- [Documentation Guide](../contributing/documentation-guide.md) - Documentation conventions
