# Build Pipeline

- **Status:** Planned
- **Summary:** Automated CI/CD using GitHub Actions to build the Unity project and deploy to GitHub Pages whenever changes are merged to the master branch.

## Overview

This planned feature will automate the build and deployment process for the Avalon prototype. Instead of manual builds, the system will automatically build the Unity project and deploy it to a playable web version whenever code is merged to the master branch.

This enables:
- Continuous integration and testing
- Always-available latest build for testing
- Automated deployment workflow
- Foundation for multi-environment deployment

## Goals

### Primary Goals

1. **Automated Builds** - Trigger builds on merge to master
2. **GitHub Pages Deployment** - Deploy WebGL builds to GitHub Pages
3. **Build Validation** - Ensure code compiles and builds successfully
4. **Version Tracking** - Tag builds with version numbers

### Secondary Goals

1. **Multi-Environment Support** - Different builds for dev/staging/production
2. **Build Notifications** - Notify on build success/failure
3. **Artifact Storage** - Store build artifacts for download
4. **Build Caching** - Speed up builds with Unity Library caching

## Planned Implementation

### GitHub Actions Workflow

**Location:** `.github/workflows/build-and-deploy.yml`

**Trigger:**
```yaml
on:
  push:
    branches:
      - master
  pull_request:
    branches:
      - master
```

**Jobs:**

1. **Build Unity Project:**
   - Checkout code
   - Setup Unity environment
   - Restore Unity Library cache
   - Run Unity build for WebGL
   - Store build artifacts

2. **Deploy to GitHub Pages:**
   - Checkout gh-pages branch
   - Copy build artifacts
   - Commit and push to gh-pages
   - Only on master branch (not PRs)

### Unity Build Settings

**Target Platform:** WebGL
**Compression:** Brotli (best compression for web)
**Code Optimization:** Master builds optimized, dev builds include debug symbols
**Data Caching:** Enabled for faster loading

### Workflow Structure

```
Code Push to Master
    |
    v
GitHub Actions Triggered
    |
    v
Unity Build Job
    |
    +-- Cache Library folder (for speed)
    +-- Run Unity CLI build
    +-- Validate build succeeded
    +-- Upload artifacts
    |
    v
Deploy Job (if build succeeded)
    |
    +-- Checkout gh-pages branch
    +-- Extract build artifacts
    +-- Copy to deployment folder
    +-- Commit and push
    |
    v
GitHub Pages Updated
    |
    v
Build Available at: https://<username>.github.io/Avalon/
```

## Acceptance Criteria

### Must Have

- [x] GitHub Actions workflow file created
- [ ] Unity build runs successfully in CI
- [ ] WebGL build deploys to GitHub Pages
- [ ] Build triggered on merge to master
- [ ] Build status visible in GitHub

### Should Have

- [ ] Build caching for faster builds
- [ ] Build notifications (email/Slack)
- [ ] Version tagging (semantic versioning)
- [ ] Build artifact download links
- [ ] Build time optimization (< 10 min)

### Nice to Have

- [ ] Multiple environment support (dev/staging/prod)
- [ ] Automated testing before build
- [ ] Build size reporting
- [ ] Performance benchmarking
- [ ] Changelog generation

## Technical Considerations

### Unity in CI/CD

**Challenges:**
- Unity requires activation/license in CI
- Large Unity Library folder (slow without caching)
- Long build times for complex projects
- Platform-specific build requirements

**Solutions:**
- Use Unity's CI activation workflow
- Cache Library folder between builds
- Incremental builds when possible
- Use Unity Cloud Build as alternative

### GitHub Pages Limitations

- Static file hosting only (suitable for WebGL)
- 1 GB repository size limit
- Bandwidth limits for high traffic
- No server-side code execution

### WebGL Build Considerations

- Build size affects loading time
- Compression required (Gzip or Brotli)
- Browser compatibility (modern browsers only)
- Mobile device support may be limited

## Alternative Approaches

### Unity Cloud Build

**Pros:**
- Official Unity solution
- Simpler Unity license handling
- Built-in build caching
- Platform-specific optimizations

**Cons:**
- Requires Unity subscription
- Less flexible than GitHub Actions
- Vendor lock-in

### Self-Hosted Runner

**Pros:**
- More control over build environment
- Faster builds (no cold start)
- Can use persistent Unity license

**Cons:**
- Requires maintaining build server
- Security concerns
- Additional infrastructure cost

## Estimated Effort

**Initial Setup:** 1-2 days
- Configure GitHub Actions workflow
- Setup Unity activation in CI
- Test build and deployment
- Document the process

**Testing & Iteration:** 1-2 days
- Fix build issues
- Optimize build time
- Validate deployment
- Test on different browsers

**Total:** 2-4 days for basic implementation

## Future Enhancements

### Multi-Environment Deployment

Deploy to different environments:
- **Dev:** On push to develop branch
- **Staging:** On merge to staging branch  
- **Production:** On merge to master or tag creation

### Automated Testing

Add testing steps before build:
- Unit tests for game logic
- Integration tests for systems
- Performance tests for critical paths
- Build only if tests pass

### Build Variants

Support multiple build targets:
- WebGL for GitHub Pages
- Windows standalone for itch.io
- Linux standalone
- Mobile platforms (iOS/Android)

### Notifications

Notify team of build status:
- Email on build failure
- Slack message with build results
- Discord webhook for status updates
- GitHub status checks on PRs

## Getting Started

When implementing this feature:

1. **Research Unity CI/CD:**
   - Study Unity's official CI/CD documentation
   - Review GameCI GitHub Actions
   - Check Unity license activation process

2. **Create workflow file:**
   - Start with basic build workflow
   - Add deployment step
   - Test with simple scene first

3. **Setup GitHub Pages:**
   - Enable GitHub Pages in repository settings
   - Configure gh-pages branch
   - Test manual deployment first

4. **Iterate and optimize:**
   - Add caching
   - Reduce build time
   - Improve error handling

## Related Documentation

- [Contributing Guide](../contributing/README.md) - Development workflow
- [Features Index](README.md) - All features
- [Architecture Overview](../architecture/README.md) - Technical overview

## External Resources

- [Unity CI/CD Documentation](https://unity.com/solutions/ci-cd)
- [GameCI GitHub Actions](https://game.ci/)
- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [GitHub Pages Documentation](https://docs.github.com/en/pages)
