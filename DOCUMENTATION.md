# Documentation Structure

This file describes the current documentation layout and a recommended pattern for adding or updating docs in this repository.

Overview

- Root docs:
  - `README.md` — high-level project overview and links to feature index and changelog.
  - `CHANGELOG.md` — track notable changes under `Unreleased` and move entries to versioned headings on release.
  - `DOCUMENTATION.md` — this file (documentation structure guide).

- Feature docs:
  - Directory: `FEATURES/`
  - Index: `FEATURES/FEATURES.md` — lists available feature documents and how to add new ones.
  - Each feature doc should be a single markdown file named with a short kebab/Title style (e.g., `SpriteSheet-to-Animations.md`, `Enemy-Behavior.md`).

- Automation notes:
  - `.github/COPILOT_INSTRUCTIONS.md` — records automated documentation steps performed by Copilot tooling. Keep it updated when automation changes docs.

Guidelines / Pattern for new docs

1. Create a new file under `FEATURES/` for feature-level design and progress. Use the following sections:
   - Title and `Status` (Draft / In Progress / Implemented / Deprecated)
   - Summary and Motivation
   - Scope
   - Current implementation (code paths, files)
   - What is not implemented / TODOs
   - Integration notes
   - Related documentation (link `CHANGELOG.md`, `README.md`, and any other feature docs)

2. Update `FEATURES/FEATURES.md` to include the new file and a one-line description.
3. Add a short entry in `CHANGELOG.md` under `Unreleased` describing the change or new feature doc.
4. If the change affects usage, update `README.md` links.

Naming & style

- File names: `Pascal-Kebab.md` or `kebab-case` but keep consistent with existing files in `FEATURES/`.
- Use code formatting for file paths and code symbols.
- Keep docs concise; prefer links to code files rather than large pasted snippets.

PR / commit workflow

- Doc-only changes: include `docs:` prefix in the commit message.
- Code + doc changes: include docs updates in the same PR and reference the feature doc in the PR description.
- Record automated doc changes in `.github/COPILOT_INSTRUCTIONS.md` when Copilot or automation modifies documentation.

Contact

For documentation process changes, update this file and add a short note in `CHANGELOG.md` under `Unreleased`.