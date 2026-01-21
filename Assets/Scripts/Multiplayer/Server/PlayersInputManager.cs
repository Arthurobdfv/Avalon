using System;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInputHandler;

public class PlayersInputManager : MonoBehaviour
{
    Dictionary<string, PlayerInputState> _pendingPlayerInputs = new Dictionary<string, PlayerInputState>();

    [SerializeField] private PlayerEntitiesManager _playerManager;
    [SerializeField] private ServerCommunicationLayerManager _serverCommunicationLayerManager;

    private ServerPacketHandler _serverPacketHandler;

    private void OnEnable()
    {
        if (_playerManager == null)
        {
            _playerManager = FindFirstObjectByType<PlayerEntitiesManager>();
        }

        if (_serverCommunicationLayerManager == null)
        {
            _serverCommunicationLayerManager = FindFirstObjectByType<ServerCommunicationLayerManager>();
        }

        if (_serverPacketHandler == null)
        {
            _serverPacketHandler = FindFirstObjectByType<ServerPacketHandler>();
        }

        _serverPacketHandler?.RegisterServerHandler<PlayerInputState>(HandlePlayerInputState);
    }

    private void OnDisable()
    {
        _serverPacketHandler?.UnregisterServerHandler<PlayerInputState>();
    }

    private void HandlePlayerInputState(PlayerInputState state)
    {
        // Store the latest input state per client - overwrites previous if multiple arrive before processing
        _pendingPlayerInputs[state.ClientId] = state;
    }

    private void FixedUpdate()
    {
        ProcessAllPendingInputs();
    }

    private void ProcessAllPendingInputs()
    {
        foreach (var kvp in _pendingPlayerInputs)
        {
            var state = kvp.Value;
            ProcessPlayerInputState(state);
            // Entity state is broadcast to clients via EntitySpawnPacket from GlobalEntitiesManager
        }

        _pendingPlayerInputs.Clear();
    }

    private void ProcessPlayerInputState(PlayerInputState state)
    {
        GlobalEntitiesManager.allEntities.TryGetValue(state.ClientId, out var entity);
        var playerCharacter = entity as PlayerCharacter;

        if (playerCharacter == null)
        {
            return;
        }

        HandlePlayerMovement(state, playerCharacter);
        HandlePlayerInteraction(state, playerCharacter);
    }

    private void HandlePlayerMovement(PlayerInputState state, PlayerCharacter playerCharacter)
    {
        var hasMovementInput = state.MoveDirection != Vector2.zero;
        var isSprinting = state.SprintAction && hasMovementInput;
        playerCharacter.SetMovement(hasMovementInput ? isSprinting ? 2 : 1 : 0);

        var characterFacingVector = hasMovementInput
            ? state.MoveDirection
            : playerCharacter.Target != null
                ? LookDirectionFromTargetPosition(playerCharacter.Target.transform.position, playerCharacter.transform.position)
                : LookDirectionFromMousePosition(state.LookDirection, playerCharacter.transform.position);

        playerCharacter.SetDirection(DirectionEnumHelper.Vector2DirectionEnum(characterFacingVector));

        if (hasMovementInput)
        {
            // TODO: Replace hardcoded speed with character speed attribute
            playerCharacter.transform.position += (Vector3)state.MoveDirection.normalized * (3 + (isSprinting ? 1 : 0) * 3) * Time.fixedDeltaTime;
        }
    }

    private void HandlePlayerInteraction(PlayerInputState state, PlayerCharacter playerCharacter)
    {
        if (!state.InteractAction)
        {
            return;
        }
        _playerManager.HandlePlayerInteract(playerCharacter);
    }

    private Vector2 LookDirectionFromTargetPosition(Vector3 targetPosition, Vector3 playerPosition)
    {
        var playerPos = Camera.main.WorldToScreenPoint(playerPosition);
        var targetPos = Camera.main.WorldToScreenPoint(targetPosition);
        var lookDirection = (targetPos - playerPos).normalized;
        return lookDirection;
    }

    private Vector2 LookDirectionFromMousePosition(Vector2 mousePosition, Vector3 playerPosition)
    {
        var playerPos = Camera.main.WorldToScreenPoint(playerPosition);
        var lookDirection = (mousePosition - (Vector2)playerPos).normalized;
        return lookDirection;
    }
}
