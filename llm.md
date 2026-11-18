# Avalon

> **What's in this file:** What Avalon is, what is built, companion repos, and where docs live.
> **Read if:** Starting any work on the Avalon game project.
> **Skip if:** You need auth → `AvalonAuthServer/`, networking → `AvalonRelayServer/`.

---

## What is this?

Unity MMO prototype. Validates MMO mechanics and practices Unity + game server architecture.

**Engine:** Unity 2021+ | **Language:** C# | **Status:** Prototype / WIP

---

## Companion repos

| Repo | Purpose |
|---|---|
| `AvalonAuthServer` | .NET 8 JWT + OAuth2 auth server for player login |
| `AvalonRelayServer` | TCP relay server for NAT traversal between clients |

---

## What's built

| Feature | Status |
|---|---|
| Character movement (WASD + directional animations) | Done |
| Combat system (attack/defend, event-driven tick-based) | Done |
| Enemy AI (targeting, aggro) | Done |
| Local multiplayer (client-server loopback) | Done |
| Sprite pipeline (automated animation) | In progress |

---

## Docs

Full docs live in `docs/`. Start at `docs/README.md`.

| Section | Path |
|---|---|
| Architecture | `docs/architecture/README.md` |
| Features | `docs/features/README.md` |
| Contributing / code style | `docs/contributing/README.md` |

---

## Key systems

- **Combat:** Event-driven tick-based. See `docs/architecture/combat-system.md`
- **Entity management:** Global registry with map-scoping
- **Input:** Input handling with multiplayer sync
- **Animation:** Character animation coordination
