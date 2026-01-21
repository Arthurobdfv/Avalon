# Code Style Guide

This document outlines coding conventions and style guidelines for the Avalon project.

## General Principles

1. **Consistency:** Follow existing code style in the file you're editing
2. **Clarity:** Write code that's easy to read and understand
3. **Simplicity:** Keep changes minimal and focused
4. **Documentation:** Comment complex logic, avoid obvious comments

## Unity C# Conventions

### Naming Conventions

**Classes and Structs:** PascalCase
```csharp
public class CombatManager { }
public struct PlayerInputState { }
```

**Methods and Properties:** PascalCase
```csharp
public void PerformAttack() { }
public int CurrentHealth { get; set; }
```

**Private Fields:** _camelCase with underscore prefix
```csharp
private float _attackTimer;
private int _tickInterval;
```

**Public Fields:** PascalCase (avoid when possible, use properties)
```csharp
public CombatBaseStats BaseStats;
```

**Local Variables and Parameters:** camelCase
```csharp
void Attack(CombatCharacter target, float damage) {
    var currentTime = Time.time;
}
```

**Constants:** PascalCase or ALL_CAPS
```csharp
public const float MaxHealth = 100f;
public const string INITIAL_MAP = "Map01";
```

**Interfaces:** IPascalCase with 'I' prefix
```csharp
public interface IPacketSender { }
```

**ScriptableObjects:** PascalCase, often with suffix
```csharp
public class CombatBaseStats : ScriptableObject { }
```

### File Organization

**One class per file:**
- File name matches class name
- Exception: Small helper classes/structs can share file

**Namespace usage:**
```csharp
namespace Avalon.Character.Combat
{
    public class CombatManager : MonoBehaviour
    {
        // Implementation
    }
}
```

**Using statements:**
- Place at top of file
- Remove unused using statements
- Group: System namespaces first, then Unity, then project

```csharp
using System;
using System.Collections.Generic;
using UnityEngine;
using Avalon.Networking;
```

### Class Structure

Organize class members in this order:

1. **Constants and static fields**
2. **Static events and delegates**
3. **Serialized fields** (`[SerializeField]`)
4. **Public fields** (avoid if possible)
5. **Private fields**
6. **Properties**
7. **Unity lifecycle methods** (Awake, Start, Update, etc.)
8. **Public methods**
9. **Protected methods**
10. **Private methods**
11. **Event handlers**
12. **Nested types**

**Example:**
```csharp
public class CombatCharacter : Character
{
    // Static
    public static event OnPerformCombat OnPerformCombatHandler;
    
    // Serialized fields
    [SerializeField] private CombatBaseStats _baseStats;
    
    // Private fields
    private float _currentTime;
    private CombatCharacter _target;
    
    // Properties
    public CombatBaseStats BaseStats => _baseStats;
    public float CurrentHealth { get; set; }
    
    // Unity lifecycle
    private void Start() { }
    private void Update() { }
    
    // Public methods
    public void SetTarget(CombatCharacter target) { }
    
    // Private methods
    private void PerformAttack() { }
    
    // Event handlers
    private void OnHealthChanged(HealthChangeEventArgs args) { }
}
```

## Code Formatting

### Braces and Indentation

**Use Allman style (braces on new line):**
```csharp
if (condition)
{
    DoSomething();
}
```

**Exception: Empty blocks can be on one line:**
```csharp
public void DoNothing() { }
```

**Indentation:** 4 spaces (not tabs)

### Spacing

**Binary operators:** Space on both sides
```csharp
int sum = a + b;
bool isValid = x > 5 && y < 10;
```

**After commas:**
```csharp
DoSomething(a, b, c);
```

**After control flow keywords:**
```csharp
if (condition) { }
for (int i = 0; i < count; i++) { }
while (isRunning) { }
```

**No space before parentheses in method calls:**
```csharp
PerformAttack();  // Good
PerformAttack ();  // Bad
```

### Line Length

- **Preferred maximum:** 100-120 characters
- Break long lines at logical points
- Indent continuation lines

```csharp
// Good
var result = SomeVeryLongMethodName(
    firstParameter,
    secondParameter,
    thirdParameter);

// Good
if (condition1 && condition2 && 
    condition3 && condition4)
{
    // ...
}
```

## Unity-Specific Guidelines

### SerializeField

**Use `[SerializeField]` for private fields that need Inspector visibility:**
```csharp
[SerializeField] private float _attackSpeed = 1.0f;
```

**Avoid public fields:**
```csharp
// Bad
public float AttackSpeed = 1.0f;

// Good
[SerializeField] private float _attackSpeed = 1.0f;
public float AttackSpeed => _attackSpeed;
```

### Unity Lifecycle Methods

**Order of lifecycle methods:**
```csharp
private void Awake() { }
private void OnEnable() { }
private void Start() { }
private void FixedUpdate() { }
private void Update() { }
private void LateUpdate() { }
private void OnDisable() { }
private void OnDestroy() { }
```

**Make lifecycle methods private:**
```csharp
// Good
private void Update() { }

// Bad
void Update() { }
public void Update() { }
```

### ScriptableObjects

**Create menu attributes:**
```csharp
[CreateAssetMenu(fileName = "NewCombatStats", menuName = "Character/Combat Base Stats")]
public class CombatBaseStats : ScriptableObject
{
    // Fields
}
```

**Use for data, not behavior:**
- ScriptableObjects should primarily hold data
- Minimal logic, mostly getters and simple calculations

### Coroutines

**Prefer async/await when possible:**
```csharp
// Prefer this
private async Task LoadDataAsync()
{
    await SomeAsyncOperation();
}

// Over this (unless Unity-specific timing needed)
private IEnumerator LoadData()
{
    yield return SomeOperation();
}
```

## Comments and Documentation

### When to Comment

**Comment these:**
- Complex algorithms or logic
- Non-obvious design decisions
- Workarounds for bugs or limitations
- TODOs and FIXMEs

**Don't comment these:**
- Obvious code
- What the code does (code should be self-documenting)

**Good:**
```csharp
// Use squared magnitude for performance (avoids sqrt)
float distanceSq = (target.position - transform.position).sqrMagnitude;
if (distanceSq < range * range)
{
    // In range
}
```

**Bad:**
```csharp
// Set health to 100
health = 100;

// Loop through enemies
foreach (var enemy in enemies) { }
```

### XML Documentation Comments

**Use for public APIs:**
```csharp
/// <summary>
/// Sets the target for this character to attack.
/// </summary>
/// <param name="target">The character to target, or null to clear target.</param>
public void SetTarget(CombatCharacter target)
{
    _target = target;
}
```

**Not needed for:**
- Private methods (unless complex)
- Self-explanatory methods
- Overrides of Unity lifecycle methods

### TODOs and FIXMEs

**Format:**
```csharp
// TODO: Add death animation handling
// FIXME: Race condition when multiple enemies target same player
```

## Error Handling

### Validation

**Validate inputs:**
```csharp
public void SetTarget(CombatCharacter target)
{
    if (target == null)
    {
        Debug.LogWarning("Attempted to set null target");
        return;
    }
    
    _target = target;
}
```

### Debug Logging

**Use appropriate log levels:**
```csharp
Debug.Log("Normal information");
Debug.LogWarning("Something unexpected but not critical");
Debug.LogError("Error that needs attention");
```

**Remove or guard debug logs in production:**
```csharp
#if UNITY_EDITOR
    Debug.Log($"Attack timer: {_currentTime}");
#endif
```

## Performance Guidelines

### Avoid in Update/FixedUpdate

- `GetComponent<T>()` - cache in Awake/Start
- `GameObject.Find()` - cache references
- String concatenation - use StringBuilder
- Boxing/unboxing - avoid object types

**Good:**
```csharp
private Rigidbody _rigidbody;

private void Awake()
{
    _rigidbody = GetComponent<Rigidbody>();
}

private void Update()
{
    _rigidbody.AddForce(Vector3.up);  // Cached
}
```

**Bad:**
```csharp
private void Update()
{
    GetComponent<Rigidbody>().AddForce(Vector3.up);  // Every frame!
}
```

### Object Pooling

Consider object pooling for frequently spawned objects:
- Projectiles
- VFX particles
- Damage numbers

### String Operations

**Use string interpolation:**
```csharp
// Good
string message = $"Health: {health}/{maxHealth}";

// Okay
string message = string.Format("Health: {0}/{1}", health, maxHealth);

// Bad (in hot paths)
string message = "Health: " + health + "/" + maxHealth;
```

## Editor-Only Code

### Assembly Separation

**Editor code goes in `Assembly-CSharp-Editor`:**
- Custom inspectors
- Editor windows
- Asset processors
- Build pipeline extensions

**Guard editor-only calls:**
```csharp
#if UNITY_EDITOR
    UnityEditor.AssetDatabase.Refresh();
#endif
```

## Multiplayer Code

### Packet Handling

**Register handlers explicitly:**
```csharp
public void Initialize()
{
    RegisterHandler<ConnectPlayerPacket>(OnConnectPlayer);
    RegisterHandler<EntitySpawnPacket>(OnEntitySpawn);
}
```

**Handle missing data gracefully:**
```csharp
private void OnPacketReceived(EntitySpawnPacket packet)
{
    if (packet.Entities == null || packet.Entities.Length == 0)
    {
        Debug.LogWarning("Received empty entity spawn packet");
        return;
    }
    
    // Process packet
}
```

## Git and Version Control

### Commit Messages

Follow conventional commit format:
```
<type>(<scope>): <subject>

<body>

<footer>
```

**Types:**
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation
- `refactor`: Code refactoring
- `test`: Tests
- `chore`: Maintenance

**Examples:**
```
feat(combat): add tick-based attack system

Implemented fixed-interval combat ticks with configurable
tick rate. Characters now attack based on AttackSpeed stat.

Files changed:
- Assets/Scripts/Character/Combat/CombatManager.cs
- Assets/Scripts/Character/CombatCharacter.cs
```

### .gitignore

Don't commit:
- `Library/` - Unity cache
- `Temp/` - Temporary files
- `*.csproj` - Generated project files (unless custom)
- `*.sln` - Generated solution files (unless custom)
- Build outputs (unless WebGL demo build)

## Project-Specific Conventions

### Constants

**Use `Constants.cs` for shared values:**
```csharp
public static class Constants
{
    public const string InitialMap = "Map01";
    public const float DefaultAttackRange = 2.0f;
}
```

### Events and Delegates

**Define delegates before events:**
```csharp
public delegate void OnCombatTick(float previousTime, float currentTime, float delta);
public static event OnCombatTick CombatTickHandler;
```

**Invoke with null check:**
```csharp
CombatTickHandler?.Invoke(prevTime, curTime, delta);
```

### Map Awareness

**Systems should be map-aware:**
```csharp
// Good
var playersOnMap = PlayerEntitiesManager.PlayersOnMap(mapId);

// Bad (global queries in multiplayer game)
var allPlayers = FindObjectsOfType<PlayerCharacter>();
```

## Refactoring Guidelines

When refactoring:
1. **Make small changes** - One logical change per commit
2. **Preserve behavior** - Don't change functionality while refactoring
3. **Add tests** - If test infrastructure exists
4. **Update docs** - Keep documentation in sync

## Code Review Guidelines

When reviewing code:
- Check for adherence to these style guidelines
- Look for potential bugs or edge cases
- Verify performance considerations
- Ensure documentation is updated
- Check for security issues (input validation, etc.)

## Questions?

If you're unsure about a convention:
1. Look at existing code for examples
2. Ask in project discussions
3. Err on the side of consistency with existing code

## Related Documentation

- [How to Contribute](README.md) - General contribution guidelines
- [Documentation Guide](documentation-guide.md) - Documentation conventions
- [Architecture Documentation](../architecture/README.md) - Technical design
