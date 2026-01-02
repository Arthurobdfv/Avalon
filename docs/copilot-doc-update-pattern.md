Copilot Documentation Update Pattern

Purpose

This document describes the standard pattern `GitHub Copilot` will follow
when asked to create or update repository documentation. Apply these
steps unless the user explicitly requests a different workflow.

Pattern

1. Discover related docs: search `README.md`, `FEATURES.md` (and
   `FEATURES/`), `CHANGELOG.md`, `CONTRIBUTING.md`, and the `docs/`
   folder.
2. Choose target:
   - If an existing document covers the change, edit that document.
   - Otherwise create `docs/feature-<short-name>.md`.
   - Never create or rely on `docs/index.md` as the canonical index.
3. Update canonical trackers:
   - Update `FEATURES.md` (or the appropriate file under `FEATURES/`) to
     add or update the feature entry and status.
   - Update `README.md` only to link canonical feature docs or
     `FEATURES.md` when appropriate.
4. Changelog: if `CHANGELOG.md` exists, propose adding an `Unreleased`
   entry and ask the user before adding it.
5. Feature doc contents: include the following sections � Overview, Key
   changes, Motivation, How to use, Notes/TODO, Files changed.
6. Commit message: produce a message that follows the repository
   `CONTRIBUTING.md` template (Conventional Commits style, `type(scope):`
   subject, <=72 characters, wrapped body at 72 characters).
7. Approval: ask the user for approval to stage and commit; perform git
   actions only after explicit user consent.

Commit message generation (policy)

When asked to generate a commit message (for unstaged, staged, or all
changes), `GitHub Copilot` will follow this policy:

- Gather changes:
  - If able to access the repository, inspect `git status --porcelain`,
    `git diff --name-only`, and `git diff` to determine changed files
    and a summary of edits.
  - If not able to run git, request the outputs of those commands or a
    pasted diff from the user.
- Select scope and type:
  - Choose a Conventional Commit `type` (e.g., `feat`, `fix`, `docs`,
    `chore`, `refactor`) based on the nature of changes.
  - Derive `scope` from the most relevant area (lowercase short name,
    e.g., `player`, `combat`, `input`, `docs`). If changes touch many
    areas, pick the most relevant scope or suggest splitting into
    multiple commits.
- Subject and body:
  - Create a short imperative `subject` (<=72 characters) with no
    trailing period.
  - Provide an optional body explaining motivation and important
    implementation notes; wrap at 72 characters per line.
  - Include a concise list of files changed and notable details.
- Docs handling:
  - By default include documentation changes in the files list and
    message body.
  - If the user requests to ignore documentation changes for the
    commit message, omit docs-only files from the described changes and
    focus the summary on code/artifact changes.
- Footers and references:
  - Add `Refs: #123` or `BREAKING CHANGE:` when relevant.
  - Include `Co-authored-by:` lines if requested.
- Finalization:
  - Present the generated commit message to the user and ask for
    approval before staging or committing.
  - If approved, run git commands only after explicit permission.

Notes

- Keep edits minimal and focused on the requested docs change.
- Do not duplicate existing documentation; prefer editing the canonical
  file that best fits the change.
- This pattern is only applied to documentation updates unless the user
  instructs otherwise.

Recorded by: `GitHub Copilot`
