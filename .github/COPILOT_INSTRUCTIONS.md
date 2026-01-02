# Copilot Documentation Notes

Purpose: record steps performed by automated documentation edits so future Copilot prompts can reference the repository's documentation updates and layout.

Steps performed (date: recorded programmatically at commit time):

1. Created a root changelog: `CHANGELOG.md` with an "Unreleased" section summarizing recent feature work (sprite-sheet pipeline, character model change, enemy behaviour manager).
2. Updated the sprite-sheet feature document: `FEATURES/SpriteSheet-to-Animations.md` to reflect the current implementation, mark slicing implemented, and note animator-transition prototype.
3. Created a feature index: `FEATURES/FEATURES.md` listing available feature documents and guidance to add new ones.
4. Created `FEATURES/Enemy-Behavior.md` documenting the `EnemyBehaviourManager` implementation, limitations, and next steps.
5. Updated `README.md` to reference the features index and changelog for discoverability.
6. Linked the above files via a "Related documentation" section in the sprite-sheet feature doc.
7. Created `DOCUMENTATION.md` describing the repository's documentation structure and recommended pattern for adding/updating docs.

Usage guidance for future prompts:
- Prefer `FEATURES/FEATURES.md` as the feature-doc index when adding or discovering feature docs.
- Record any new documentation artifacts in `CHANGELOG.md` under `Unreleased` and update `FEATURES/FEATURES.md` to include the new document.
- When updating design or implementation docs, mention the exact file paths changed and a short rationale for the change.
- Use this file as the canonical place to record automated documentation steps performed by Copilot during future tasks.
- If Copilot automation creates or edits documentation, also update `DOCUMENTATION.md` to keep the documentation pattern in sync.

Notes:
- This file is maintained by automation; if you remove or rename any of the files recorded above, update this file accordingly.
