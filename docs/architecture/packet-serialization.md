# Packet Serialization System

This document describes the binary packet serialization system used for network communication in Avalon.

## Overview

The serialization system uses a **describer pattern** where each packet type has a `PacketDescriber<T>` that defines which properties to serialize and in what order. Serializers are auto-inferred from property types for common types, with explicit serializers available for complex types.

## Architecture

```
PacketDescriber<T>          Defines property order and serializers for a packet
    |
    +-- PropertyDescriptor  Describes a single property (getter, setter, serializer)
            |
            +-- ITypeSerializer<T>  Handles actual byte conversion
```

## Registered Packets


| Packet | Type ID | Properties | Serializers | Empty Size |
|--------|---------|------------|-------------|------------|
| `ConnectPlayerPacket` | 1 | `PlayerId`, `PlayerName`, `AuthToken` | Auto (String x3) | 16 bytes |
| `ConnectObserverPacket` | 2 | `ObserverId`, `ObserverName` | Auto (String x2) | 12 bytes |
| `ObserveMapPacket` | 3 | `MapId` | Auto (String) | 8 bytes |
| `EntitySpawnPacket` | 100 | `MapId`, `Entities` | String + EntityDictionary | 8 bytes |
| `PlayerEquipmentPacket` | 101 | `PlayerBaseAsset` | Auto (Enum) | 8 bytes |
| `PlayerInputState` | 200 | `LookDirection`, `MoveDirection`, `AttackAction`, `SprintAction`, `InteractAction` | Auto (Vector2 x2 + Bool x3) | 23 bytes |
| `MapDataSpawnPacket` | 300 | `MapData` | MapSerializers.MapData | 5 bytes |

### Packet Type ID Ranges

- **1-99**: Connection/Observer packets
- **100-199**: Entity packets
- **200-299**: Player action packets
- **300-399**: Map packets

## Type Serializers

### Built-in Serializers (Auto-inferred)

| Type | Serializer | Size | Wire Format |
|------|------------|------|-------------|
| `int` | Int32Serializer | 4 bytes | Big-endian |
| `float` | FloatSerializer | 4 bytes | Big-endian IEEE 754 |
| `bool` | BoolSerializer | 1 byte | 0 or 1 |
| `string` | StringSerializer | 4 + length | Length-prefixed UTF-8 |
| `Vector2` | Vector2Serializer | 8 bytes | 2 floats |
| `Vector3` | Vector3Serializer | 12 bytes | 3 floats |
| `Enum` | EnumSerializer<T> | 4 bytes | As int, big-endian |


### Collection Serializers


| Type | Serializer | Format |
|------|------------|--------|
| `List<T>` | ListSerializer<T> | 4-byte count + items |
| `Dictionary<string, T>` | DictionarySerializer<T> | 4-byte count + key-value pairs |

### Complex Type Serializers

| Type | Serializer | Notes |
|------|------------|-------|
| `EntityInfo` | EntityInfoSerializer | Polymorphic (handles PlayerEntityInfo, EnemyEntityInfo) |
| `MapData` | MapDataSerializer | Includes SpawnPoints and WarpPoints |
| `SpawnPointData` | SpawnPointDataSerializer | Nested in MapData |
| `WarpPoint` | WarpPointSerializer | Nested in MapData |

## Wire Format

### Packet Structure

All packets include a common header with `PacketTypeId` and `ClientId`:

```
[4 bytes: Packet Type ID][N bytes: ClientId (length-prefixed string)][M bytes: Payload]
```

- **Packet Type ID**: 4 bytes, big-endian integer identifying the packet type
- **ClientId**: Variable-length string (from base `AvalonPacket` class), automatically serialized
- **Payload**: Type-specific properties as defined by the `PacketDescriber`

### String Encoding

```
[4 bytes: Length (or -1 for null)][N bytes: UTF-8 data]
```

### Collection Encoding

```
[4 bytes: Count (or -1 for null)][Items...]
```

### Polymorphic Types (EntityInfo)

```
[1 byte: Type discriminator][Base fields...][Derived fields...]
```

Type discriminators:
- `0x00`: EntityInfo (base)
- `0x01`: PlayerEntityInfo
- `0x02`: EnemyEntityInfo
- `0xFF`: null

## Usage

### Defining a New Packet

1. Create the packet class:

```csharp
public class MyNewPacket : AvalonPacket
{
    public string SomeString { get; set; }
    public int SomeInt { get; set; }
    public MyEnum SomeEnum { get; set; }
}
```

2. Register in `PacketDescribers.cs`:

```csharp
public const int MyNewPacketId = 200; // Use appropriate range

// In RegisterAll() or a new method:
var myDescriber = new PacketDescriber<MyNewPacket>()
    .Property(p => p.SomeString, (p, v) => p.SomeString = v)
    .Property(p => p.SomeInt, (p, v) => p.SomeInt = v)
    .Property(p => p.SomeEnum, (p, v) => p.SomeEnum = v);
PacketDescriberRegistry.Register(MyNewPacketId, myDescriber);
```

3. For complex types, provide an explicit serializer:

```csharp
.Property(p => p.ComplexData, (p, v) => p.ComplexData = v, MySerializers.ComplexData)
```

### Serializing/Deserializing

```csharp
// Serialize any registered packet
byte[] bytes = PacketDescriberRegistry.Serialize(packet);

// Deserialize (auto-detects type from header)
AvalonPacket packet = PacketDescriberRegistry.Deserialize(bytes);

// Deserialize to specific type
var describer = PacketDescriberRegistry.Get<ConnectPlayerPacket>();
var packet = describer.Deserialize(bytes);
```

## Testing

### Running Tests in Unity Editor

Use the menu: **Tools > Avalon > Run Full Serialization Test Suite**

Or call from code:
```csharp
bool passed = PacketSerializationTestRunner.RunTests();
```

### Running Tests from CLI (Build Pipeline)

```bash
# Windows
Unity.exe -batchmode -quit -projectPath "C:\path\to\project" -executeMethod PacketSerializationTestRunner.RunTestsAndExit -logFile -

# macOS/Linux
./Unity -batchmode -quit -projectPath "/path/to/project" -executeMethod PacketSerializationTestRunner.RunTestsAndExit -logFile -
```

Exit codes:
- `0`: All tests passed
- `1`: One or more tests failed

### CI/CD Pipeline Example (GitHub Actions)

```yaml
jobs:
  test-serialization:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      
      - uses: game-ci/unity-test-runner@v4
        with:
          projectPath: .
          customParameters: -executeMethod PacketSerializationTestRunner.RunTestsAndExit
```

## Validation

### Validate All Packets

Menu: **Tools > Avalon > Validate Packet Serialization**

This checks:
- All packet types have registered describers
- All packets can serialize without errors
- All packets can deserialize without errors
- Roundtrip data integrity

## Files

| File | Purpose |
|------|---------|
| `DataParsing/Serializers/ITypeSerializer.cs` | Core serializer interface |
| `DataParsing/Serializers/TypeSerializers.cs` | Built-in type serializers |
| `DataParsing/Serializers/TypeSerializerResolver.cs` | Auto-resolves serializers by type |
| `DataParsing/Serializers/CollectionSerializers.cs` | List and Dictionary serializers |
| `DataParsing/Serializers/EntityInfoSerializer.cs` | Polymorphic EntityInfo serializer |
| `DataParsing/Serializers/MapDataSerializer.cs` | MapData and related serializers |
| `DataParsing/Describers/PropertyDescriptor.cs` | Property descriptor base class |
| `DataParsing/Describers/PacketDescriber.cs` | Packet describer with fluent API |
| `DataParsing/Describers/PacketDescriberRegistry.cs` | Central registry for all describers |
| `DataParsing/Describers/PacketDescribers.cs` | All packet registrations |
| `DataParsing/Tests/PacketSerializationTestRunner.cs` | Test runner (static) |
| `Editor/Validation/PacketSerializationValidator.cs` | Editor validation utility |

## Changelog

- **2024-01**: Initial implementation with auto-inference and polymorphic support
