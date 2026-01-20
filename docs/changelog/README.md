# Changelog

The project changelog is maintained at the repository root for visibility:

📄 **[CHANGELOG.md](../../CHANGELOG.md)**

## Why at Root?

The changelog is kept at the repository root level because:
- It's a standard location that contributors expect
- GitHub and many tools look for it there
- High visibility for anyone browsing the repository
- Easy to find without navigating documentation structure

## What's in the Changelog?

The changelog tracks:
- New features and capabilities
- Bug fixes and corrections
- Breaking changes
- Deprecations
- Documentation updates
- Performance improvements

## How to Update

When making changes to the project:

1. **Add entry under `Unreleased` section:**
   ```markdown
   ## Unreleased
   
   ### Added
   - New combat system with event-driven architecture
   - Documentation for input system
   
   ### Changed
   - Refactored entity management for better performance
   
   ### Fixed
   - Fixed animation not updating on direction change
   ```

2. **Follow the format:**
   - Group by change type: Added, Changed, Deprecated, Removed, Fixed, Security
   - Use bullet points for each change
   - Include relevant links or references

3. **Be descriptive but concise:**
   - Good: "Added critical hit system with configurable chance and multiplier"
   - Bad: "Updated combat"

## Changelog Format

We follow [Keep a Changelog](https://keepachangelog.com/) format:

```markdown
# Changelog

## [Unreleased]
### Added
- New features

### Changed
- Changes to existing functionality

### Deprecated
- Soon-to-be removed features

### Removed
- Removed features

### Fixed
- Bug fixes

### Security
- Security patches

## [1.0.0] - 2024-01-15
### Added
- Initial release
```

## Related Documentation

- [Contributing Guide](README.md) - How to contribute
- [Documentation Guide](documentation-guide.md) - Documentation standards
- [CHANGELOG.md](../../CHANGELOG.md) - The actual changelog
