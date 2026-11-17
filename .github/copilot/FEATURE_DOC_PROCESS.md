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

Creating a new feature doc (step-by-step)
The steps below reflect the exact process used for the "SpriteSheet to Animations Pipeline" feature.

1) Identify feature name and slug
- Use a short, hyphenated file name: `FEATURES/<Feature-Name>.md` (e.g., `SpriteSheet-to-Animations.md`).

2) Create the feature doc under `FEATURES/`
- Start from the templates below. For technical/editor tooling, prefer the "Extended technical feature template".

3) Update `FEATURES.md`
- In the Todo list, add a new entry linking to the feature doc (or replace any older placeholder pointing elsewhere).
- In the Details section, add or rename a subsection to match the new feature name and link to the doc. Example performed:
  - Replaced the "Custom Asset Importer" details with "SpriteSheet to Animations Pipeline" and added a Docs link.

4) Update related feature docs (if content moved)
- If details were previously nested under another feature, add a short note pointing to the new page. Example performed:
  - In `FEATURES/Character-Animation.md`, added a note: "Custom Asset Importer (moved → see: [SpriteSheet to Animations Pipeline](SpriteSheet-to-Animations.md))".

5) Update `README.md`
- Adjust the Project Progress quick list to link to the new feature doc.

6) Commit changes
- Use clear messages. Example messages used:
  - `docs: add SpriteSheet-to-Animations feature doc and link from FEATURES list; point Character-Animation doc to new page`
  - `docs: update README to link to SpriteSheet-to-Animations feature page`
  - `docs: index now links to SpriteSheet-to-Animations and details section updated`
- PowerShell tip: Use `;` or separate commands instead of `&&`:
  - `git add FEATURES/... ; git commit -m "docs: ..."`

7) Push and open PR
- Title should reflect the feature and that it updates docs. Link related issues/PRs.

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

Extended technical feature template (used for SpriteSheet-to-Animations)

````markdown
# <Feature Name>

- Status: Planned / In Progress / Implemented
- Summary: What the tooling/system does in one sentence.

## Motivation
Why this is needed (speed, reliability, consistency, etc.).

## Scope
- In-scope bullets
- Out-of-scope bullets

## Requirements / Acceptance Criteria
- List concrete requirements and acceptance bullets

## Proposed Implementation
- Where the code lives
- Storage/config options
- Typical workflow steps
- Naming/structural conventions

## Example Mapping/Config (if applicable)
Code block samples (JSON, YAML, etc.)

## Output Layout (suggested)
- Where generated assets/files go

## Integration Notes
- How runtime systems consume outputs

## Tasks / TODO
- [ ] Task list

## Risks / Open Questions
- Risks and mitigations
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

Example: SpriteSheet to Animations Pipeline update
- Create `FEATURES/SpriteSheet-to-Animations.md` using the extended template.
- Update `FEATURES.md`:
  - Add a Todo entry linking to the new page.
  - Rename the Details subsection to "SpriteSheet to Animations Pipeline" and add a Docs link.
- In `FEATURES/Character-Animation.md`, add a moved note pointing to the new page.
- Update `README.md` Project Progress to link to the new page.
- Commit with messages like:
  - `docs: add SpriteSheet-to-Animations feature doc and link from FEATURES list; point Character-Animation doc to new page`
  - `docs: update README to link to SpriteSheet-to-Animations feature page`

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
