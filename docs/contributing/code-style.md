# Code Style Guide

This document outlines the coding conventions and best practices for the Avalon project. Consistent code style makes the codebase easier to read, maintain, and collaborate on.

## General Principles

- **Readability First** - Code is read more than written
- **Consistency** - Match existing code style
- **Simplicity** - Prefer simple, clear solutions
- **Documentation** - Comment the "why", not the "what"

## Language and Framework

- **Language:** C# (version dictated by Unity/.NET Framework 4.7.1)
- **Framework:** Unity Engine
- **Target:** .NET Framework 4.7.1

## Naming Conventions

### Classes and Structs

**PascalCase** for classes, structs, and interfaces:

```csharp
// Good
public class CombatManager { }
public struct HealthData { }
public interface ICombatHandler { }

// Bad
public class combatManager { }
public class combat_manager { }
```

### Methods and Properties

**PascalCase** for methods and properties:

```csharp
// Good
public void PerformAttack() { }
public int CurrentHealth { get; set; }

// Bad
public void performAttack() { }
public int current_health { get; set; }
```

### Fields

**Private fields:** Use `_camelCase` with underscore prefix:

```csharp
// Good
private int _currentHealth;
private CombatCharacter _target;

// Bad
private int currentHealth;
private int m_currentHealth;
```

**Public fields:** Use `PascalCase` (prefer properties):

```csharp
// Good (but prefer properties)
public int MaxHealth;

// Better
public int MaxHealth { get; private set; }
```

### Local Variables and Parameters

**camelCase** for local variables and parameters:

```csharp
// Good
void Attack(CombatCharacter target) {
    int damageAmount = CalculateDamage();
    float distance = Vector3.Distance(transform.position, target.transform.position);
}

// Bad
void Attack(CombatCharacter Target) {
    int DamageAmount = CalculateDamage();
}
```

### Constants

**PascalCase** for constants:

```csharp
// Good
public const int MaxPlayers = 100;
private const float AttackCooldown = 1.5f;

// Bad
public const int MAX_PLAYERS = 100;
```

### Events and Delegates

Suffix events with `Handler`:

```csharp
// Good
public delegate void OnCombatTick(float delta);
public static OnCombatTick CombatTickHandler;

public delegate void OnHealthChange(HealthChangeEventArgs args);
public OnHealthChange OnHealthChangeHandler;

// Bad
public OnCombatTick CombatTick;
public OnHealthChange HealthChanged;
```

### ScriptableObjects

Suffix with descriptive type:

```csharp
// Good
public class CombatBaseStats : ScriptableObject { }
public class CharacterData : ScriptableObject { }

// Bad
public class Stats : ScriptableObject { }
```

## File Organization

### File Structure

One class per file, file name matches class name:

```
CombatManager.cs -> public class CombatManager
PlayerCharacter.cs -> public class PlayerCharacter
```

### Folder Structure

Organize by feature/system:

```
Assets/Scripts/
├── Character/
│   ├── Player/
│   │   └── PlayerCharacter.cs
│   ├── Enemy/
│   │   └── EnemyCharacter.cs
│   └── Combat/
│       └── CombatManager.cs
├── Input/
│   └── PlayerInputHandler.cs
└── UI/
    └── HealthBar.cs
```

### Using Statements

Group and order using statements:

```csharp
// 1. System namespaces
using System;
using System.Collections.Generic;

// 2. Unity namespaces
using UnityEngine;
using UnityEngine.UI;

// 3. Third-party namespaces
using Newtonsoft.Json;

// 4. Project namespaces
using Avalon.Combat;
using Avalon.Input;
```

## Code Structure

### Class Member Order

Order class members logically:

```csharp
public class Example : MonoBehaviour {
    // 1. Constants
    private const int MaxValue = 100;
    
    // 2. Static fields
    public static OnEvent EventHandler;
    
    // 3. Serialized fields (Unity Inspector)
    [SerializeField] private int _health;
    
    // 4. Private fields
    private float _timer;
    private CombatCharacter _target;
    
    // 5. Properties
    public int CurrentHealth { get; private set; }
    
    // 6. Unity messages (in lifecycle order)
    void Awake() { }
    void Start() { }
    void Update() { }
    void OnEnable() { }
    void OnDisable() { }
    
    // 7. Public methods
    public void Attack() { }
    
    // 8. Private methods
    private void CalculateDamage() { }
}
```

### Methods

**Keep methods small and focused:**

```csharp
// Good - Single responsibility
private void ApplyDamage(int damage) {
    CurrentHealth -= damage;
    OnHealthChangeHandler?.Invoke(CreateHealthArgs());
}

private HealthChangeEventArgs CreateHealthArgs() {
    return new HealthChangeEventArgs {
        OldHealth = CurrentHealth + damage,
        NewHealth = CurrentHealth,
        MaxHealth = BaseStats.Health
    };
}

// Bad - Too much in one method
private void ApplyDamage(int damage) {
    CurrentHealth -= damage;
    var args = new HealthChangeEventArgs {
        OldHealth = CurrentHealth + damage,
        NewHealth = CurrentHealth,
        MaxHealth = BaseStats.Health
    };
    OnHealthChangeHandler?.Invoke(args);
    if (CurrentHealth <= 0) {
        Die();
    }
    UpdateHealthBar();
    PlayDamageSound();
}
```

### Properties vs Fields

**Prefer properties over public fields:**

```csharp
// Good
public int CurrentHealth { get; private set; }

// Bad
public int CurrentHealth;
```

**Use auto-properties when possible:**

```csharp
// Good
public CombatCharacter Target { get; private set; }

// Less good (only if you need custom logic)
private CombatCharacter _target;
public CombatCharacter Target {
    get { return _target; }
    private set { _target = value; }
}
```

## Unity-Specific Conventions

### SerializeField

Use `[SerializeField]` for private fields you want in Inspector:

```csharp
// Good - Private but visible in Inspector
[SerializeField] private int _maxHealth = 100;
[SerializeField] private CombatBaseStats _baseStats;

// Bad - Public just for Inspector
public int maxHealth = 100;
```

### MonoBehaviour Messages

Use Unity's message names exactly:

```csharp
// Good
void Awake() { }
void Start() { }
void Update() { }
void FixedUpdate() { }
void OnEnable() { }
void OnDisable() { }

// Bad
void awake() { }
void onEnable() { }
```

### Coroutines

Name coroutine methods with descriptive names:

```csharp
// Good
IEnumerator FadeOutAndDestroy() {
    yield return new WaitForSeconds(1f);
    Destroy(gameObject);
}

// Bad
IEnumerator Routine() { }
```

### Editor-Only Code

Guard editor-only code properly:

```csharp
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(CombatManager))]
public class CombatManagerEditor : Editor {
    // Editor code
}
#endif
```

## Comments and Documentation

### XML Documentation

Use XML comments for public APIs:

```csharp
/// <summary>
/// Applies damage to the character and triggers health change events.
/// </summary>
/// <param name="damage">Amount of damage to apply</param>
/// <returns>True if character is still alive</returns>
public bool TakeDamage(int damage) {
    // Implementation
}
```

### Inline Comments

Comment the "why", not the "what":

```csharp
// Good - Explains reasoning
// Use squared magnitude to avoid expensive square root calculation
float sqrDistance = (target.position - transform.position).sqrMagnitude;

// Bad - States the obvious
// Get the distance
float distance = Vector3.Distance(target.position, transform.position);
```

### TODO Comments

Use TODO for future work:

```csharp
// TODO: Implement armor calculation
// TODO: Add critical hit support
// FIXME: This doesn't handle negative damage
// HACK: Temporary workaround until proper system is implemented
```

## Best Practices

### Null Checks

Always check for null before using references:

```csharp
// Good
if (_target != null) {
    _target.TakeDamage(damage);
}

// Or use null-conditional operator
_target?.TakeDamage(damage);

// For Unity objects
if (_animator) {
    _animator.SetTrigger("Attack");
}
```

### Event Invocation

Use null-conditional operator for events:

```csharp
// Good
OnHealthChangeHandler?.Invoke(args);

// Bad - Can throw NullReferenceException
OnHealthChangeHandler(args);
```

### String Comparison

Use proper string comparison methods:

```csharp
// Good
if (tagName.Equals("Player", StringComparison.OrdinalIgnoreCase)) { }
if (string.IsNullOrEmpty(characterName)) { }

// Bad
if (tagName == "Player") { } // Case sensitive
if (characterName == null || characterName == "") { }
```

### Magic Numbers

Avoid magic numbers, use named constants:

```csharp
// Good
private const float AttackRange = 2.5f;
private const int MaxHealthBonus = 50;

if (distance < AttackRange) {
    Attack();
}

// Bad
if (distance < 2.5f) {
    Attack();
}
```

### Performance

**Cache component references:**

```csharp
// Good - Cache in Awake/Start
private Animator _animator;

void Awake() {
    _animator = GetComponent<Animator>();
}

void Update() {
    _animator.SetFloat("Speed", speed);
}

// Bad - GetComponent every frame
void Update() {
    GetComponent<Animator>().SetFloat("Speed", speed);
}
```

**Use appropriate update methods:**

```csharp
// Physics updates
void FixedUpdate() {
    // Rigidbody movements
    rb.MovePosition(newPosition);
}

// Frame updates
void Update() {
    // Input handling
    // Non-physics updates
}

// Late update
void LateUpdate() {
    // Camera following
}
```

## Testing and Debugging

### Debug Statements

Use clear debug messages:

```csharp
// Good
Debug.Log($"[CombatManager] Attack performed by {source.name} on {target.name}");
Debug.LogWarning($"[PlayerCharacter] Low health: {CurrentHealth}/{MaxHealth}");
Debug.LogError($"[EnemyCharacter] No valid target found!");

// Bad
Debug.Log("attack");
Debug.LogWarning("low health");
```

### Gizmos

Use Gizmos for visual debugging:

```csharp
void OnDrawGizmos() {
    // Draw attack range
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, AttackRange);
    
    // Draw vision range
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, VisionRange);
}
```

## Common Patterns

### Singleton (Use Sparingly)

For managers that need global access:

```csharp
public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    
    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
```

### Factory Pattern

For complex object creation:

```csharp
public class CharacterFactory {
    public static PlayerCharacter CreatePlayer(CharacterData data) {
        var prefab = Resources.Load<PlayerCharacter>("Prefabs/Player");
        var player = Instantiate(prefab);
        player.Initialize(data);
        return player;
    }
}
```

### Observer Pattern (Events)

For decoupled communication:

```csharp
// Publisher
public class CombatCharacter {
    public delegate void OnHealthChange(int oldHealth, int newHealth);
    public OnHealthChange OnHealthChangeHandler;
    
    protected void NotifyHealthChange(int old, int newValue) {
        OnHealthChangeHandler?.Invoke(old, newValue);
    }
}

// Subscriber
healthBar.character.OnHealthChangeHandler += UpdateHealthDisplay;
```

## Anti-Patterns to Avoid

❌ **God Classes** - Classes that do everything
❌ **Spaghetti Code** - Tangled dependencies
❌ **Premature Optimization** - Optimize after profiling
❌ **Copy-Paste Code** - Extract shared logic
❌ **Deep Nesting** - Refactor if more than 3 levels
❌ **Long Methods** - Break into smaller methods
❌ **Magic Strings** - Use constants or enums

## Tools and Automation

### Recommended Tools

- **ReSharper** - Code analysis and refactoring
- **Rider** - Unity-focused IDE
- **Visual Studio** - With Unity extensions
- **.editorconfig** - Enforce formatting rules

### Code Formatting

Run auto-formatting before commits:
- Visual Studio: Ctrl+K, Ctrl+D
- Rider: Ctrl+Alt+L

## Questions?

If you're unsure about a style choice:
1. Look at existing code for examples
2. Ask in pull request review
3. Prioritize readability and consistency

## Related Documentation

- [Contributing Guide](README.md) - General contribution guidelines
- [Documentation Guide](documentation-guide.md) - Documentation standards
- [Architecture Overview](../architecture/README.md) - System designs
