using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Validation utility for packet serialization.
/// Use this to verify all packets are properly described and serializable.
/// 
/// In Unity Editor:
/// 1. Open the menu: Tools > Avalon > Validate Packet Serialization
/// 2. Check the Console for results
/// 
/// Or call PacketSerializationValidator.ValidateAll() from code.
/// </summary>
public static class PacketSerializationValidator
{
    /// <summary>
    /// Validates all packet types and returns a validation result.
    /// </summary>
    public static ValidationResult ValidateAll()
    {
        var result = new ValidationResult();

        // Ensure describers are registered
        PacketDescribers.RegisterAll();

        // Get all packet types
        var allPacketTypes = GetAllPacketTypes();
        result.TotalPacketTypes = allPacketTypes.Length;

        foreach (var packetType in allPacketTypes)
        {
            ValidatePacketType(packetType, result);
        }

        return result;
    }

    private static Type[] GetAllPacketTypes()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a =>
            {
                try { return a.GetTypes(); }
                catch { return new Type[0]; }
            })
            .Where(t => t.IsClass && !t.IsAbstract && typeof(AvalonPacket).IsAssignableFrom(t))
            .ToArray();
    }

    private static void ValidatePacketType(Type packetType, ValidationResult result)
    {
        // Check if describer is registered
        try
        {
            var instance = Activator.CreateInstance(packetType) as AvalonPacket;
            if (instance == null)
            {
                result.Errors.Add($"{packetType.Name}: Could not create instance");
                return;
            }

            try
            {
                PacketDescriberRegistry.GetPacketTypeId(instance);
            }
            catch (InvalidOperationException)
            {
                result.MissingDescribers.Add(packetType.Name);
                return;
            }

            // Try serialization
            try
            {
                byte[] bytes = PacketDescriberRegistry.Serialize(instance);
                if (bytes == null || bytes.Length < 4)
                {
                    result.Errors.Add($"{packetType.Name}: Serialization returned invalid bytes");
                    return;
                }
                result.SerializedSizes[packetType.Name] = bytes.Length;
            }
            catch (Exception ex)
            {
                result.SerializationErrors.Add($"{packetType.Name}: {ex.Message}");
                return;
            }

            // Try deserialization roundtrip
            try
            {
                byte[] bytes = PacketDescriberRegistry.Serialize(instance);
                var deserialized = PacketDescriberRegistry.Deserialize(bytes);

                if (deserialized == null)
                {
                    result.Errors.Add($"{packetType.Name}: Deserialization returned null");
                    return;
                }

                if (deserialized.GetType() != packetType)
                {
                    result.Errors.Add($"{packetType.Name}: Deserialized to wrong type {deserialized.GetType().Name}");
                    return;
                }

                result.ValidPackets.Add(packetType.Name);
            }
            catch (Exception ex)
            {
                result.DeserializationErrors.Add($"{packetType.Name}: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            result.Errors.Add($"{packetType.Name}: {ex.Message}");
        }
    }

    /// <summary>
    /// Logs validation results to the Unity console.
    /// </summary>
    public static void LogResults(ValidationResult result)
    {
        Debug.Log("=== Packet Serialization Validation ===");
        Debug.Log($"Total packet types found: {result.TotalPacketTypes}");
        Debug.Log($"Valid packets: {result.ValidPackets.Count}");

        if (result.MissingDescribers.Count > 0)
        {
            Debug.LogWarning($"Missing describers ({result.MissingDescribers.Count}):\n" +
                $"  - {string.Join("\n  - ", result.MissingDescribers)}\n" +
                $"Register them in PacketDescribers.RegisterAll()");
        }

        if (result.SerializationErrors.Count > 0)
        {
            Debug.LogError($"Serialization errors ({result.SerializationErrors.Count}):\n" +
                $"  - {string.Join("\n  - ", result.SerializationErrors)}");
        }

        if (result.DeserializationErrors.Count > 0)
        {
            Debug.LogError($"Deserialization errors ({result.DeserializationErrors.Count}):\n" +
                $"  - {string.Join("\n  - ", result.DeserializationErrors)}");
        }

        if (result.Errors.Count > 0)
        {
            Debug.LogError($"Other errors ({result.Errors.Count}):\n" +
                $"  - {string.Join("\n  - ", result.Errors)}");
        }

        if (result.IsValid)
        {
            Debug.Log("<color=green>All registered packets are valid!</color>");
        }
        else
        {
            Debug.LogError("Validation failed. See errors above.");
        }

        // Log packet sizes
        if (result.SerializedSizes.Count > 0)
        {
            var sizeInfo = string.Join("\n  ",
                result.SerializedSizes
                    .OrderBy(kvp => kvp.Key)
                    .Select(kvp => $"{kvp.Key}: {kvp.Value} bytes (empty)"));
            Debug.Log($"Serialized sizes (empty packets):\n  {sizeInfo}");
        }
    }

    public class ValidationResult
    {
        public int TotalPacketTypes { get; set; }
        public List<string> ValidPackets { get; } = new List<string>();
        public List<string> MissingDescribers { get; } = new List<string>();
        public List<string> SerializationErrors { get; } = new List<string>();
        public List<string> DeserializationErrors { get; } = new List<string>();
        public List<string> Errors { get; } = new List<string>();
        public Dictionary<string, int> SerializedSizes { get; } = new Dictionary<string, int>();

        public bool IsValid =>
            MissingDescribers.Count == 0 &&
            SerializationErrors.Count == 0 &&
            DeserializationErrors.Count == 0 &&
            Errors.Count == 0;

        public bool HasWarnings => MissingDescribers.Count > 0;
    }

#if UNITY_EDITOR
    [MenuItem("Tools/Avalon/Validate Packet Serialization")]
    public static void ValidateFromMenu()
    {
        var result = ValidateAll();
        LogResults(result);
    }

    [MenuItem("Tools/Avalon/Run Packet Roundtrip Tests")]
    public static void RunRoundtripTests()
    {
        PacketDescribers.RegisterAll();

        bool allPassed = true;

        // Test ConnectPlayerPacket
        allPassed &= TestRoundtrip("ConnectPlayerPacket", () =>
        {
            var original = new ConnectPlayerPacket
            {
                PlayerId = "player-123",
                PlayerName = "TestPlayer",
                AuthToken = "token-abc-xyz"
            };

            byte[] bytes = PacketDescriberRegistry.Serialize(original);
            var deserialized = PacketDescriberRegistry.Deserialize(bytes) as ConnectPlayerPacket;

            return deserialized != null &&
                   original.PlayerId == deserialized.PlayerId &&
                   original.PlayerName == deserialized.PlayerName &&
                   original.AuthToken == deserialized.AuthToken;
        });

        // Test ConnectObserverPacket
        allPassed &= TestRoundtrip("ConnectObserverPacket", () =>
        {
            var original = new ConnectObserverPacket
            {
                ObserverId = "observer-456",
                ObserverName = "TestObserver"
            };

            byte[] bytes = PacketDescriberRegistry.Serialize(original);
            var deserialized = PacketDescriberRegistry.Deserialize(bytes) as ConnectObserverPacket;

            return deserialized != null &&
                   original.ObserverId == deserialized.ObserverId &&
                   original.ObserverName == deserialized.ObserverName;
        });

        // Test ObserveMapPacket
        allPassed &= TestRoundtrip("ObserveMapPacket", () =>
        {
            var original = new ObserveMapPacket
            {
                MapId = "map-world-1"
            };

            byte[] bytes = PacketDescriberRegistry.Serialize(original);
            var deserialized = PacketDescriberRegistry.Deserialize(bytes) as ObserveMapPacket;

            return deserialized != null && original.MapId == deserialized.MapId;
        });

        // Test EntitySpawnPacket with data
        allPassed &= TestRoundtrip("EntitySpawnPacket", () =>
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
                    },
                    ["entity-2"] = new EnemyEntityInfo
                    {
                        EntityType = EntityTypeEnum.Enemy,
                        Position = new Vector3(50, 60, 0),
                        Rotation = DirectionEnum.LEFT,
                        Movement = 0,
                        EntityAssetId = "skeleton"
                    }
                }
            };

            byte[] bytes = PacketDescriberRegistry.Serialize(original);
            var deserialized = PacketDescriberRegistry.Deserialize(bytes) as EntitySpawnPacket;

            if (deserialized == null || deserialized.Entities == null) return false;
            if (deserialized.MapId != original.MapId) return false;
            if (deserialized.Entities.Count != 2) return false;

            var entity2 = deserialized.Entities["entity-2"] as EnemyEntityInfo;
            return entity2 != null && entity2.EntityAssetId == "skeleton";
        });

        // Test null handling
        allPassed &= TestRoundtrip("NullValues", () =>
        {
            var original = new ConnectPlayerPacket
            {
                PlayerId = null,
                PlayerName = null,
                AuthToken = null
            };

            byte[] bytes = PacketDescriberRegistry.Serialize(original);
            var deserialized = PacketDescriberRegistry.Deserialize(bytes) as ConnectPlayerPacket;

            return deserialized != null &&
                   deserialized.PlayerId == null &&
                   deserialized.PlayerName == null &&
                   deserialized.AuthToken == null;
        });

        // Test empty collections
        allPassed &= TestRoundtrip("EmptyEntities", () =>
        {
            var original = new EntitySpawnPacket
            {
                MapId = "empty-map",
                Entities = new Dictionary<string, EntityInfo>()
            };

            byte[] bytes = PacketDescriberRegistry.Serialize(original);
            var deserialized = PacketDescriberRegistry.Deserialize(bytes) as EntitySpawnPacket;

            return deserialized != null &&
                   deserialized.Entities != null &&
                   deserialized.Entities.Count == 0;
        });

        if (allPassed)
        {
            Debug.Log("<color=green>=== All roundtrip tests passed! ===</color>");
        }
        else
        {
            Debug.LogError("=== Some roundtrip tests failed! ===");
        }
    }

    private static bool TestRoundtrip(string testName, Func<bool> test)
    {
        try
        {
            bool passed = test();
            if (passed)
            {
                Debug.Log($"<color=green>[PASS]</color> {testName}");
            }
            else
            {
                Debug.LogError($"[FAIL] {testName}: Assertion failed");
            }
            return passed;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[FAIL] {testName}: {ex.Message}");
            return false;
        }
    }

    [MenuItem("Tools/Avalon/Run Full Serialization Test Suite")]
    public static void RunFullTestSuite()
    {
        PacketSerializationTestRunner.RunTests();
    }
#endif
}
