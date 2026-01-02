# How Copilot should document changes and create commit messages

Purpose
- This document explains how to request documentation updates and commit message creation from the Copilot assistant. Follow these steps when asking Copilot to produce docs and git commit messages for unstaged changes.

Process Copilot will follow
1. Inspect repository status
   - Run `git status --porcelain --untracked-files=all` to collect all modified/untracked files.
   - Use the output to determine which files changed and which are new.

2. Read the changed files
   - For each path reported as modified or untracked, open the file contents.
   - Extract runtime-relevant changes (classes, methods, public API, ScriptableObjects, scene/prefab references).

3. Summarize code/runtime changes
   - Produce a short summary describing: added/modified files, main behavior changes, new events/delegates, ScriptableObjects, scene/prefab changes, and any TODOs.
   - Include a list of files changed and short bullets about what changed in each file.

4. Update or create documentation pages
   - If there is a `docs/` page related to the feature, update it and add a "Unstaged / Recent runtime changes" section listing concrete changed files and highlights.
   - If none exists, create a new `docs/<Feature>.md` and link it from `docs/feature-*.md`, `FEATURES.md`, and `README.md` where relevant.

5. Create a commit message
   - Compose a clear commit message following conventional commit style (type(scope): short summary) with a bullet list in the commit body describing the key runtime and docs changes.
   - If many files are changed across unrelated features, suggest splitting into multiple commits and explain why.

6. Offer PR description (optional)
   - Provide a short PR description summarizing the intent, runtime impact, and testing notes.

Request format for users
- When asking Copilot to document and commit, provide: (a) a short title for the change, and (b) whether you want a single commit or multiple commits. Example:
  - "Please document and create a single commit for the unstaged changes: feature/BasicEntityManagerAndCombat"

Example question the user could ask Copilot
- "Copilot, document and commit the unstaged changes for combat. Create a single commit and give me the commit message."

Notes and caveats
- Copilot will not automatically stage or commit files unless explicitly requested. The assistant can propose and write the commit message for you to use.
- Copilot attempts to keep documentation minimal and focused; if you prefer verbose docs, ask for expanded details or examples.

If you want, I can now stage the current changes and create the commit using the commit message I produce.