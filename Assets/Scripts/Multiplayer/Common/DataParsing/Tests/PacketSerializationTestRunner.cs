using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Static test runner for packet serialization validation.
/// Can be called from Editor menu, code, or CLI via Unity batch mode.
/// 
/// Usage from code:
///   bool passed = PacketSerializationTestRunner.RunTests();
/// 
/// Usage from CLI (batch mode):
///   Unity.exe -batchmode -projectPath "path/to/project" -executeMethod PacketSerializationTestRunner.RunTestsAndExit -logFile -
/// </summary>
public static class PacketSerializationTestRunner
{
    /// <summary>
    /// Entry point for CLI batch mode execution.
    /// Exits with code 0 on success, 1 on failure.
    /// </summary>
    public static void RunTestsAndExit()
    {
        bool passed = RunTests();
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.Exit(passed ? 0 : 1);
#endif
    }

    /// <summary>
    /// Runs all serialization tests.
    /// Returns true if all tests pass.
    /// </summary>
    public static bool RunTests()
    {
        Debug.Log("=== Starting Packet Serialization Tests ===\n");
        
        PacketDescribers.RegisterAll();
        
        int passed = 0;
        int failed = 0;

        // CRITICAL TEST: All AvalonPacket types must have describers
        if (TestAllPacketTypesHaveDescribers()) passed++; else failed++;

        // Test: All registered packets can serialize/deserialize
        if (TestAllRegisteredPacketsRoundtrip()) passed++; else failed++;

        // Specific roundtrip tests with data
        if (TestConnectPlayerPacket()) passed++; else failed++;
        if (TestConnectObserverPacket()) passed++; else failed++;
        if (TestObserveMapPacket()) passed++; else failed++;
        if (TestPlayerEquipmentPacket()) passed++; else failed++;
        if (TestPlayerInputState()) passed++; else failed++;
        if (TestEntitySpawnPacket()) passed++; else failed++;
        if (TestEntitySpawnPacketPolymorphic()) passed++; else failed++;
        if (TestMapDataSpawnPacket()) passed++; else failed++;
        
        // Edge cases
        if (TestNullStrings()) passed++; else failed++;
        if (TestEmptyCollections()) passed++; else failed++;
        if (TestNullCollections()) passed++; else failed++;
        
        // ClientId serialization
        if (TestClientIdSerialization()) passed++; else failed++;
        if (TestNullClientIdSerialization()) passed++; else failed++;

        Debug.Log($"\n=== Test Results: {passed} passed, {failed} failed ===");
        
        if (failed == 0)
        {
            Debug.Log("<color=green>All tests passed!</color>");
        }
        else
        {
            Debug.LogError($"{failed} test(s) failed!");
        }

        return failed == 0;
    }

    /// <summary>
    /// Discovers all AvalonPacket subclasses and verifies each has a registered describer.
    /// This is the critical test that catches missing packet registrations.
    /// </summary>
    private static bool TestAllPacketTypesHaveDescribers()
    {
        const string testName = "All Packet Types Have Describers";
        try
        {
            var allPacketTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a =>
                {
                    try { return a.GetTypes(); }
                    catch { return new Type[0]; }
                })
                .Where(t => t.IsClass && !t.IsAbstract && typeof(AvalonPacket).IsAssignableFrom(t))
                .ToList();

            var missingDescribers = new List<string>();

            foreach (var packetType in allPacketTypes)
            {
                try
                {
                    var instance = Activator.CreateInstance(packetType) as AvalonPacket;
                    if (instance != null)
                    {
                        PacketDescriberRegistry.GetPacketTypeId(instance);
                    }
                }
                catch (InvalidOperationException)
                {
                    missingDescribers.Add(packetType.Name);
                }
            }

            if (missingDescribers.Count > 0)
            {
                return Fail(testName, 
                    $"Missing describers for {missingDescribers.Count} packet type(s):\n" +
                    $"  - {string.Join("\n  - ", missingDescribers)}\n" +
                    $"Register them in PacketDescribers.RegisterAll()");
            }

            return Pass(testName, $"All {allPacketTypes.Count} packet types have describers");
        }
        catch (Exception ex)
        {
            return Fail(testName, ex.Message);
        }
    }

    /// <summary>
    /// Tests that all registered packets can serialize and deserialize correctly.
    /// </summary>
    private static bool TestAllRegisteredPacketsRoundtrip()
    {
        const string testName = "All Registered Packets Roundtrip";
        try
        {
            var allPacketTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a =>
                {
                    try { return a.GetTypes(); }
                    catch { return new Type[0]; }
                })
                .Where(t => t.IsClass && !t.IsAbstract && typeof(AvalonPacket).IsAssignableFrom(t))
                .ToList();

            var errors = new List<string>();
            int testedCount = 0;

            foreach (var packetType in allPacketTypes)
            {
                try
                {
                    var instance = Activator.CreateInstance(packetType) as AvalonPacket;
                    if (instance == null) continue;

                    // Check if registered
                    try { PacketDescriberRegistry.GetPacketTypeId(instance); }
                    catch (InvalidOperationException) { continue; } // Skip unregistered (caught by other test)

                    // Test serialize
                    byte[] bytes = PacketDescriberRegistry.Serialize(instance);
                    if (bytes == null || bytes.Length < 4)
                    {
                        errors.Add($"{packetType.Name}: Serialization returned invalid bytes");
                        continue;
                    }

                    // Test deserialize
                    var deserialized = PacketDescriberRegistry.Deserialize(bytes);
                    if (deserialized == null)
                    {
                        errors.Add($"{packetType.Name}: Deserialization returned null");
                        continue;
                    }

                    if (deserialized.GetType() != packetType)
                    {
                        errors.Add($"{packetType.Name}: Deserialized to wrong type {deserialized.GetType().Name}");
                        continue;
                    }

                    testedCount++;
                }
                catch (Exception ex)
                {
                    errors.Add($"{packetType.Name}: {ex.Message}");
                }
            }

            if (errors.Count > 0)
            {
                return Fail(testName, $"Errors in {errors.Count} packet(s):\n  - {string.Join("\n  - ", errors)}");
            }

            return Pass(testName, $"All {testedCount} registered packets serialize/deserialize correctly");
        }
        catch (Exception ex)
        {
            return Fail(testName, ex.Message);
        }
    }

    private static bool TestConnectPlayerPacket()
    {
        const string testName = "ConnectPlayerPacket";
        try
        {
            var original = new ConnectPlayerPacket
            {
                PlayerId = "player-123",
                PlayerName = "TestPlayer",
                AuthToken = "token-abc-xyz"
            };

            byte[] bytes = PacketDescriberRegistry.Serialize(original);
            var deserialized = PacketDescriberRegistry.Deserialize(bytes) as ConnectPlayerPacket;

            if (deserialized == null)
                return Fail(testName, "Deserialized to null");
            if (original.PlayerId != deserialized.PlayerId)
                return Fail(testName, $"PlayerId mismatch: '{original.PlayerId}' vs '{deserialized.PlayerId}'");
            if (original.PlayerName != deserialized.PlayerName)
                return Fail(testName, $"PlayerName mismatch: '{original.PlayerName}' vs '{deserialized.PlayerName}'");
            if (original.AuthToken != deserialized.AuthToken)
                return Fail(testName, $"AuthToken mismatch: '{original.AuthToken}' vs '{deserialized.AuthToken}'");

            return Pass(testName, $"Serialized to {bytes.Length} bytes");
        }
        catch (Exception ex)
        {
            return Fail(testName, ex.Message);
        }
    }

    private static bool TestConnectObserverPacket()
    {
        const string testName = "ConnectObserverPacket";
        try
        {
            var original = new ConnectObserverPacket
            {
                ObserverId = "observer-456",
                ObserverName = "TestObserver"
            };

            byte[] bytes = PacketDescriberRegistry.Serialize(original);
            var deserialized = PacketDescriberRegistry.Deserialize(bytes) as ConnectObserverPacket;

            if (deserialized == null)
                return Fail(testName, "Deserialized to null");
            if (original.ObserverId != deserialized.ObserverId)
                return Fail(testName, $"ObserverId mismatch");
            if (original.ObserverName != deserialized.ObserverName)
                return Fail(testName, $"ObserverName mismatch");

            return Pass(testName, $"Serialized to {bytes.Length} bytes");
        }
        catch (Exception ex)
        {
            return Fail(testName, ex.Message);
        }
    }

    private static bool TestObserveMapPacket()
    {
        const string testName = "ObserveMapPacket";
        try
        {
            var original = new ObserveMapPacket
            {
                MapId = "map-world-1"
            };

            byte[] bytes = PacketDescriberRegistry.Serialize(original);
            var deserialized = PacketDescriberRegistry.Deserialize(bytes) as ObserveMapPacket;

            if (deserialized == null)
                return Fail(testName, "Deserialized to null");
            if (original.MapId != deserialized.MapId)
                return Fail(testName, $"MapId mismatch");

            return Pass(testName, $"Serialized to {bytes.Length} bytes");
        }
        catch (Exception ex)
        {
            return Fail(testName, ex.Message);
        }
    }

    private static bool TestPlayerEquipmentPacket()
    {
        const string testName = "PlayerEquipmentPacket";
        try
        {
            var original = new PlayerEquipmentPacket
            {
                PlayerBaseAsset = PlayerBaseAssetEnum.PLAYER_ADVENTURER_BASE_01
            };

            byte[] bytes = PacketDescriberRegistry.Serialize(original);
            var deserialized = PacketDescriberRegistry.Deserialize(bytes) as PlayerEquipmentPacket;

            if (deserialized == null)
                return Fail(testName, "Deserialized to null");
            if (original.PlayerBaseAsset != deserialized.PlayerBaseAsset)
                return Fail(testName, $"PlayerBaseAsset mismatch: {original.PlayerBaseAsset} vs {deserialized.PlayerBaseAsset}");

            return Pass(testName, $"Serialized to {bytes.Length} bytes");
        }
        catch (Exception ex)
        {
            return Fail(testName, ex.Message);
        }
    }

    private static bool TestPlayerInputState()
    {
        const string testName = "PlayerInputState";
        try
        {
            var original = new PlayerInputState
            {
                LookDirection = new Vector2(1.0f, 0.5f),
                MoveDirection = new Vector2(-0.5f, 1.0f),
                AttackAction = true,
                SprintAction = false,
                InteractAction = true
            };

            byte[] bytes = PacketDescriberRegistry.Serialize(original);
            var deserialized = PacketDescriberRegistry.Deserialize(bytes) as PlayerInputState;

            if (deserialized == null)
                return Fail(testName, "Deserialized to null");
            if (original.LookDirection != deserialized.LookDirection)
                return Fail(testName, $"LookDirection mismatch: {original.LookDirection} vs {deserialized.LookDirection}");
            if (original.MoveDirection != deserialized.MoveDirection)
                return Fail(testName, $"MoveDirection mismatch: {original.MoveDirection} vs {deserialized.MoveDirection}");
            if (original.AttackAction != deserialized.AttackAction)
                return Fail(testName, $"AttackAction mismatch");
            if (original.SprintAction != deserialized.SprintAction)
                return Fail(testName, $"SprintAction mismatch");
            if (original.InteractAction != deserialized.InteractAction)
                return Fail(testName, $"InteractAction mismatch");

            return Pass(testName, $"Serialized to {bytes.Length} bytes");
        }
        catch (Exception ex)
        {
            return Fail(testName, ex.Message);
        }
    }

    private static bool TestEntitySpawnPacket()
    {
        const string testName = "EntitySpawnPacket (basic)";
        try
        {
            var original = new EntitySpawnPacket
            {
                MapId = "test-map",
                Entities = new Dictionary<string, EntityInfo>
                {
                    ["entity-1"] = new EntityInfo
                    {
                        EntityType = EntityTypeEnum.Player,
                        Position = new Vector3(10, 20, 0),
                        Rotation = DirectionEnum.RIGHT,
                        Movement = 1
                    }
                }
            };

            byte[] bytes = PacketDescriberRegistry.Serialize(original);
            var deserialized = PacketDescriberRegistry.Deserialize(bytes) as EntitySpawnPacket;

            if (deserialized == null)
                return Fail(testName, "Deserialized to null");
            if (original.MapId != deserialized.MapId)
                return Fail(testName, "MapId mismatch");
            if (deserialized.Entities == null)
                return Fail(testName, "Entities is null");
            if (deserialized.Entities.Count != 1)
                return Fail(testName, $"Entity count mismatch: expected 1, got {deserialized.Entities.Count}");
            if (!deserialized.Entities.ContainsKey("entity-1"))
                return Fail(testName, "Missing entity-1");

            var entity = deserialized.Entities["entity-1"];
            if (entity.EntityType != EntityTypeEnum.Player)
                return Fail(testName, "EntityType mismatch");
            if (entity.Position != new Vector3(10, 20, 0))
                return Fail(testName, $"Position mismatch: {entity.Position}");
            if (entity.Rotation != DirectionEnum.RIGHT)
                return Fail(testName, "Rotation mismatch");
            if (entity.Movement != 1)
                return Fail(testName, "Movement mismatch");

            return Pass(testName, $"Serialized to {bytes.Length} bytes");
        }
        catch (Exception ex)
        {
            return Fail(testName, ex.Message);
        }
    }

    private static bool TestEntitySpawnPacketPolymorphic()
    {
        const string testName = "EntitySpawnPacket (polymorphic)";
        try
        {
            var original = new EntitySpawnPacket
            {
                MapId = "test-map-poly",
                Entities = new Dictionary<string, EntityInfo>
                {
                    ["player-1"] = new PlayerEntityInfo
                    {
                        EntityType = EntityTypeEnum.Player,
                        Position = new Vector3(100, 200, 0),
                        Rotation = DirectionEnum.UP,
                        Movement = 1
                    },
                    ["enemy-1"] = new EnemyEntityInfo
                    {
                        EntityType = EntityTypeEnum.Enemy,
                        Position = new Vector3(50, 60, 0),
                        Rotation = DirectionEnum.DOWN,
                        Movement = 0,
                        EntityAssetId = "skeleton"
                    }
                }
            };

            byte[] bytes = PacketDescriberRegistry.Serialize(original);
            var deserialized = PacketDescriberRegistry.Deserialize(bytes) as EntitySpawnPacket;

            if (deserialized == null)
                return Fail(testName, "Deserialized to null");
            if (deserialized.Entities.Count != 2)
                return Fail(testName, $"Entity count mismatch: expected 2, got {deserialized.Entities.Count}");

            // Check PlayerEntityInfo
            if (!deserialized.Entities.ContainsKey("player-1"))
                return Fail(testName, "Missing player-1");
            var player = deserialized.Entities["player-1"] as PlayerEntityInfo;
            if (player == null)
                return Fail(testName, "player-1 is not PlayerEntityInfo");

            // Check EnemyEntityInfo
            if (!deserialized.Entities.ContainsKey("enemy-1"))
                return Fail(testName, "Missing enemy-1");
            var enemy = deserialized.Entities["enemy-1"] as EnemyEntityInfo;
            if (enemy == null)
                return Fail(testName, "enemy-1 is not EnemyEntityInfo");
            if (enemy.EntityAssetId != "skeleton")
                return Fail(testName, $"EntityAssetId mismatch: '{enemy.EntityAssetId}'");

            return Pass(testName, $"Serialized to {bytes.Length} bytes, polymorphism preserved");
        }
        catch (Exception ex)
        {
            return Fail(testName, ex.Message);
        }
    }

    private static bool TestMapDataSpawnPacket()
    {
        const string testName = "MapDataSpawnPacket";
        try
        {
            // MapData is a ScriptableObject, so we test with null
            var original = new MapDataSpawnPacket
            {
                MapData = null
            };

            byte[] bytes = PacketDescriberRegistry.Serialize(original);
            var deserialized = PacketDescriberRegistry.Deserialize(bytes) as MapDataSpawnPacket;

            if (deserialized == null)
                return Fail(testName, "Deserialized to null");
            if (deserialized.MapData != null)
                return Fail(testName, "Expected null MapData");

            return Pass(testName, $"Serialized null MapData to {bytes.Length} bytes");
        }
        catch (Exception ex)
        {
            return Fail(testName, ex.Message);
        }
    }

    private static bool TestNullStrings()
    {
        const string testName = "Null String Handling";
        try
        {
            var original = new ConnectPlayerPacket
            {
                PlayerId = null,
                PlayerName = null,
                AuthToken = null
            };

            byte[] bytes = PacketDescriberRegistry.Serialize(original);
            var deserialized = PacketDescriberRegistry.Deserialize(bytes) as ConnectPlayerPacket;

            if (deserialized == null)
                return Fail(testName, "Deserialized to null");
            if (deserialized.PlayerId != null)
                return Fail(testName, "PlayerId should be null");
            if (deserialized.PlayerName != null)
                return Fail(testName, "PlayerName should be null");
            if (deserialized.AuthToken != null)
                return Fail(testName, "AuthToken should be null");

            return Pass(testName, $"Serialized to {bytes.Length} bytes");
        }
        catch (Exception ex)
        {
            return Fail(testName, ex.Message);
        }
    }

    private static bool TestEmptyCollections()
    {
        const string testName = "Empty Collections";
        try
        {
            var original = new EntitySpawnPacket
            {
                MapId = "empty-map",
                Entities = new Dictionary<string, EntityInfo>()
            };

            byte[] bytes = PacketDescriberRegistry.Serialize(original);
            var deserialized = PacketDescriberRegistry.Deserialize(bytes) as EntitySpawnPacket;

            if (deserialized == null)
                return Fail(testName, "Deserialized to null");
            if (deserialized.Entities == null)
                return Fail(testName, "Entities should not be null");
            if (deserialized.Entities.Count != 0)
                return Fail(testName, $"Entities should be empty, got {deserialized.Entities.Count}");

            return Pass(testName, $"Serialized to {bytes.Length} bytes");
        }
        catch (Exception ex)
        {
            return Fail(testName, ex.Message);
        }
    }

    private static bool TestNullCollections()
    {
        const string testName = "Null Collections";
        try
        {
            var original = new EntitySpawnPacket
            {
                MapId = "null-entities-map",
                Entities = null
            };

            byte[] bytes = PacketDescriberRegistry.Serialize(original);
            var deserialized = PacketDescriberRegistry.Deserialize(bytes) as EntitySpawnPacket;

            if (deserialized == null)
                return Fail(testName, "Deserialized to null");
            if (deserialized.Entities != null)
                return Fail(testName, "Entities should be null");

            return Pass(testName, $"Serialized to {bytes.Length} bytes");
        }
        catch (Exception ex)
        {
            return Fail(testName, ex.Message);
        }
    }

    private static bool TestClientIdSerialization()
    {
        const string testName = "ClientId Serialization";
        try
        {
            var original = new ConnectPlayerPacket
            {
                PlayerId = "player-123",
                PlayerName = "TestPlayer",
                AuthToken = "token"
            };
            original.SetClientId("client-abc-123");

            byte[] bytes = PacketDescriberRegistry.Serialize(original);
            var deserialized = PacketDescriberRegistry.Deserialize(bytes) as ConnectPlayerPacket;

            if (deserialized == null)
                return Fail(testName, "Deserialized to null");
            if (deserialized.ClientId != "client-abc-123")
                return Fail(testName, $"ClientId mismatch: expected 'client-abc-123', got '{deserialized.ClientId}'");
            if (deserialized.PlayerId != original.PlayerId)
                return Fail(testName, "PlayerId mismatch");

            return Pass(testName, $"ClientId preserved across serialization ({bytes.Length} bytes)");
        }
        catch (Exception ex)
        {
            return Fail(testName, ex.Message);
        }
    }

    private static bool TestNullClientIdSerialization()
    {
        const string testName = "Null ClientId Serialization";
        try
        {
            var original = new ConnectPlayerPacket
            {
                PlayerId = "player-123",
                PlayerName = "TestPlayer",
                AuthToken = "token"
            };
            // ClientId is null by default

            byte[] bytes = PacketDescriberRegistry.Serialize(original);
            var deserialized = PacketDescriberRegistry.Deserialize(bytes) as ConnectPlayerPacket;

            if (deserialized == null)
                return Fail(testName, "Deserialized to null");
            if (deserialized.ClientId != null)
                return Fail(testName, $"ClientId should be null, got '{deserialized.ClientId}'");

            return Pass(testName, $"Null ClientId handled correctly ({bytes.Length} bytes)");
        }
        catch (Exception ex)
        {
            return Fail(testName, ex.Message);
        }
    }

    private static bool Pass(string testName, string details = null)
    {
        string message = $"<color=green>[PASS]</color> {testName}";
        if (!string.IsNullOrEmpty(details))
            message += $" - {details}";
        Debug.Log(message);
        return true;
    }

    private static bool Fail(string testName, string reason)
    {
        Debug.LogError($"[FAIL] {testName} - {reason}");
        return false;
    }
}
