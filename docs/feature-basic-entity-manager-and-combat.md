Feature: Basic Entity Manager and Combat Flow

Overview

This feature introduces a minimal entity management system, player input
handling, and a simple timed combat flow to bootstrap gameplay
interactions. It centralizes entity operations and provides a starting
point for player/enemy interactions.

Key changes

- `PlayerEntitiesManager`: centralizes entity registration and lookup.
- `EntityExtensions`: helper extensions for common entity operations.
- `PlayerInputHandler`: captures player input and forwards actions to the
  entity system.
- `PlayerCharacter`: initializes runtime health, advances an attack timer
  in `Update`, and triggers timed attacks in `HandleCombat` when a
  `Target` is present.
- `EnemyBehaviourManager` / `EnemyCharacter`: integrate enemies with the
  new behavioural and combat flow.
- `CombatCharacter` / `CombatBaseStats`: shared combat properties and
  base stats used by characters.

Motivation

Having a small, centralized entity manager and clear input and combat
flow makes it easier to iterate on gameplay and prepare for future
extensions (for example, separating combat into a dedicated `Combat
Manager` for multiplayer support).

How to use

1. Ensure `PlayerEntitiesManager` is initialized at startup (attach to a
   scene object or bootstrap from code).
2. Hook `PlayerInputHandler` up to the input pipeline so player actions
   reach the entity systems.
3. Use `PlayerCharacter` and `EnemyCharacter` prefabs to participate in
   the entity system. The player will perform a timed attack whenever a
   `Target` is set and the attack timer exceeds `BaseStats.AttackSpeed`.

Notes and TODOs

- Current attack action is a placeholder: it logs `Attack` to the
  console and resets the attack timer. Replace with actual attack
  effects (animations, damage application, projectiles, etc.).
- Consider moving combat responsibilities to a dedicated `Combat
  Manager` for better separation of concerns and multiplayer support.

Files changed

- `Assets/Scripts/Character/Extensions/EntityExtensions.cs`
- `Assets/Scripts/Character/Player/PlayerEntitiesManager.cs`
- `Assets/Scripts/Character/Player/PlayerCharacter.cs`
- `Assets/Scripts/Input/Handler/PlayerInputHandler.cs`
- `Assets/Scripts/Character/Enemy/EnemyBehaviourManager.cs`
- `Assets/Scripts/Character/Enemy/EnemyCharacter.cs`
- `Assets/Scripts/Character/CombatCharacter.cs`
- `Assets/Scripts/Character/Combat/CombatBaseStats.cs`

See also

- `docs/index.md` for a list of documentation pages for this repository.
