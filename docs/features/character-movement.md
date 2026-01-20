# Character Movement

- **Status:** Implemented (movement + input integration)
- **Summary:** Player movement and input-handling logic have been implemented and synchronized with server ticks to avoid duplicate updates.


## Release notes

- Implemented player movement driven by mapped input, with initial player input handling and movement logic in place.
- Added `PlayerCharacter` abstraction to encapsulate movement application and state.
- Movement updates are synchronized to the server tick to avoid multiple updates per tick and reduce redundant network messages.
- The sample scene was updated to showcase movement and animation integration.

Developer notes / TODOs found in code

- `PlayerInputHandler` contains TODOs indicating planned refactors:
  - Extract input helpers into separate modules for clarity.
  - Replace hardcoded movement speed with a configurable character attribute.
- Consider documenting the server tick synchronization strategy so other contributors can follow the same pattern.

Suggested next steps

- Extract input mapping and add support for multiple input devices (keyboard/gamepad).
- Expose movement parameters (speed, acceleration) on character data/components.
- Add editor or automated tests to validate server tick sync behavior.

