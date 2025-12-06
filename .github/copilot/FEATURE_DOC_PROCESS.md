# Feature Documentation Workflow (Copilot helper)

Purpose
- Document the exact steps used to collect recent changes, summarize implementation work in release-note style, and publish/update feature docs so future feature work can be documented consistently without re-explaining the process.

When to run
- Run this anytime you want to update `FEATURES.md` or a per-feature doc after merging commits or PRs that change behaviour, add assets, or introduce new subsystems.

Quick checklist (summary)
- Fetch recent commits and file changes
- Scan code for inline TODOs/FIXMEs
- Create or update feature doc under `FEATURES/` (use the template below)
- Update `FEATURES.md` to link the feature doc and set the TODO status
- Update `README.md` (Project Progress quick list) if desired
- Commit, push, and open PR

Commands used (PowerShell) — copy/paste

1) Inspect recent commits with filenames and concise metadata (used to find what changed):

```powershell
cd C:\Users\arthu\Documents\Github\Avalon
git --no-pager log --name-status --pretty=format:"==%ncommit:%H%nshort:%h%nauthor:%an%ndate:%ad%nsubject:%s" --date=short -n 80
```

2) Search the workspace for inline notes (TODO/FIXME etc.):

```powershell
Get-ChildItem -Recurse -Include *.cs,*.md,*.txt,*.unity | Select-String -Pattern 'TODO|FIXME' | Format-Table Path, LineNumber, Line -AutoSize
```

3) Read or open files of interest (examples):

```powershell
# open README.md or other files in your editor
notepad .\README.md
```

4) Create or edit feature docs and README changes using the repository editor (we store feature docs in `FEATURES/`).

Feature doc template
- Create `FEATURES/Feature-Name.md` and fill with this structure:

````markdown
# Feature Name

- **Status:** (Planned / In progress / Implemented)
- **Summary:** One-sentence description of the feature.
- **Relevant paths:** list of key files/asset folders

## Release notes

- Bullet list of what was implemented (release-note style, not raw commit messages).

Developer notes

- Inline tips, TODOs found in code, or suggested refactors.

## Goals / Next Steps

- Short list of next work items.

## Tasks / TODO

- [ ] Task A
- [ ] Task B

````

How I convert commits -> release notes (recommended approach)
- Read recent commit subjects and filenames; group related changes (e.g., multiple animation clips added) into a single bullet.
- Avoid pasting raw commit hashes or full messages in the release notes; instead write a plain-English summary describing the effect (what changed and why it matters).
- For developer notes, include any inline TODOs or code comments found while scanning.

Updating the root `FEATURES.md` and `README.md`
- Add or update a single-line link in `FEATURES.md` pointing to the new `FEATURES/Feature-Name.md` file. Use a checkbox to reflect status:

  - `[x] [Feature Name](FEATURES/Feature-Name.md) — short summary`

- In `README.md`, keep a `## Project Progress` quick list linking to the most important feature docs so contributors see recent changes without opening `FEATURES.md`.

Commit & PR guidance
- Branch naming: `feature/<short-name>` or `docs/feature-docs/<feature-name>` for documentation-only updates.
- Commit message examples:
  - `docs(features): add Character Animation feature doc and update README progress`
  - `feat(animation): add eight-directional walk clips (docs updated)`
- Push and open a PR describing the feature change and link to the issue or the commit set if relevant.

Example: workflow for the Character Animation + Movement update
- Run the `git log` command above and inspect files added/modified under `Assets/Animations/` and `Assets/Scripts/Character/`.
- Use `Select-String` to collect TODOs. Note them in the Developer notes section of the new feature doc.
- Write release notes describing: "Added eight-directional walk animation clips, introduced CharacterAnimationHandler to coordinate animation states, and updated the sample scene to use new clips." — do not paste the commit message literally.
- Add TODO checkbox for the sprite-sheet parser under the feature doc and link it from `FEATURES.md` and `README.md`.

Automation notes (optional)
- You can automate parts of this with a small script:
  - Use `git log` parsing to list added/modified files since last tag/branch.
  - Use `Select-String` or `rg` (ripgrep) to find TODOs.
  - Use a template engine to render a feature doc and write it to `FEATURES/`.

Where to put new feature docs
- `FEATURES/Feature-Name.md` (use hyphenated names, e.g., `Character-Animation.md`).

Maintainer notes
- Keep release notes short and focused — 2–5 bullets is usually ideal.
- Keep the Developer notes actionable (code locations, TODOs, suggested refactors).
- Update `README.md`'s quick list only for high-level or active features.

If you want, I can also commit this file and mark the documentation TODO as completed. Do you want me to commit it now to the current branch (`feature/Movement`) or create a docs branch and open a PR?
