using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    List<(DirectionEnum direction, float upperBand)> DirectionWithUpperBandMapping = new();
    private PlayerInputState _currentInputState = null;
    private PlayerInputState _serverReceivedInputState = null;

    [SerializeField] private PlayerEntitiesManager _playerManager;

    [SerializeField] ClientCommunicationLayerManager _clientCommunicationLayerManager;
    private ClientPacketHandler _clientPacketHandler;

    // TODO: Refactor into a different file
    [AvalonAuthorized]
    public class PlayerInputState : AvalonPacket
    {
        public Vector2 LookDirection = Vector2.zero;
        public Vector2 MoveDirection = Vector2.zero;
        public bool AttackAction = false;
        public bool SprintAction = false;
        public bool InteractAction = false;
    }


    // Start is called before the first frame update
    void OnEnable()
    {
        if (_playerManager == null)
        {
            _playerManager = FindFirstObjectByType<PlayerEntitiesManager>();
        }

        if(_clientCommunicationLayerManager == null)
        {
            _clientCommunicationLayerManager = FindFirstObjectByType<ClientCommunicationLayerManager>();
        }

        UpdateInputHandler(GlobalConfigsProvider.MultiplayerEnabled);
        GlobalConfigsProvider.MultiplayerEnabledChanged += UpdateInputHandler;
    }

    private void UpdateInputHandler(bool isEnabled)
    {
        Debug.Log($"Updating Player Input Handler to Multiplayer: {isEnabled}");
        PlayerInputMapper.OnSendMoveInput -= HandleMoveInput;
        PlayerInputMapper.OnSendMoveInput += HandleMoveInput;

        PlayerInputMapper.OnSentInteractInput -= HandleInteractInput;
        PlayerInputMapper.OnSentInteractInput += HandleInteractInput;
        
        if(_clientPacketHandler == null)
        {
            _clientPacketHandler = FindAnyObjectByType<ClientPacketHandler>(); 
        }
        if(isEnabled)
        {
            _clientPacketHandler.RegisterClientHandler<PlayerInputState>(HandleServerInputState);
        }
        else
        {
            _clientPacketHandler.UnregisterClientHandler<PlayerInputState>();
        }

    }

    private void UnsubAll(bool isEnabledMultiplayer)
    {
        PlayerInputMapper.OnSendMoveInput -= HandleMoveInput;
        PlayerInputMapper.OnSentInteractInput -= HandleInteractInput;
    }

    private void OnDisable()
    {
        GlobalConfigsProvider.MultiplayerEnabledChanged -= UpdateInputHandler;
        UnsubAll(GlobalConfigsProvider.MultiplayerEnabled);
    }


    private void FixedUpdate()
    {
        if (_currentInputState != null)
        {
            if(GlobalConfigsProvider.MultiplayerEnabled)
            {
                _clientCommunicationLayerManager.Send(_currentInputState);
                _currentInputState = null;
            }
        }
        if (GlobalConfigsProvider.MultiplayerEnabled)
        {
            if(_serverReceivedInputState != null)
            {
                HandlePlayerInputState(_serverReceivedInputState, () => { _serverReceivedInputState = null; });
            }
        }
        else
        {
            if(_currentInputState != null)
            {
                HandlePlayerInputState(_currentInputState, () => { _currentInputState = null; });
            }
        }
    }

    private void HandleServerInputState(PlayerInputState state)
    {
        _serverReceivedInputState = state;
    }

    private void HandlePlayerInputState(PlayerInputState state, Action handleCallback = null)
    {
        GlobalEntitiesManager.allEntities.TryGetValue(state.ClientId, out var entity);
        var playerCharacter = entity as PlayerCharacter;
        HandlePlayerMovement(state, playerCharacter);
        HandlePlayerInteraction(state, playerCharacter);
        handleCallback?.Invoke();
    }

    private void HandlePlayerInteraction(PlayerInputState state, PlayerCharacter playerCharacter)
    {
        if (!state.InteractAction)
        {
            return;
        }
        // Look for the closest enemy and start combat
        _playerManager.HandlePlayerInteract(playerCharacter);
    }

    private void HandlePlayerMovement(PlayerInputState state, PlayerCharacter playerCharacter)
    {
        var hasMovementInput = state.MoveDirection != Vector2.zero;
        var isSprinting = state.SprintAction && hasMovementInput;
        var movement = hasMovementInput ? isSprinting ? 2 : 1 : 0;
        playerCharacter?.SetMovement(hasMovementInput ? isSprinting ? 2 : 1 : 0);
        var characterFacingVector = hasMovementInput
            ? state.MoveDirection
            : playerCharacter.Target != null ?
                LookDirectionFromTargetPosition(playerCharacter.Target.transform.position, playerCharacter.transform.position) :
                LookDirectionFromMousePosition(state.LookDirection, playerCharacter.transform.position);
        playerCharacter?.SetDirection(Vector2DirectionEnum(characterFacingVector));

        if (hasMovementInput)
        {
            // TODO: Replace hardcoded speed with character speed attribute
            playerCharacter.transform.position += (Vector3)state.MoveDirection.normalized * (3 + (isSprinting ? 1 : 0) * 3) * Time.fixedDeltaTime;
        }
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

    // Creating a Stateful Input Handler for Player Character already thinking on having multiplayer in future
    // Sets up the last input received for the player character to be able to handle on the next server tick
    private void HandleMoveInput(Vector2 lookDirection, Vector2 moveDirection, bool sprintAction)
    {
        if (lookDirection != Vector2.zero)
        {
            GetCurrentInputState().LookDirection = lookDirection;
        }

        if (moveDirection != Vector2.zero)
        {
            GetCurrentInputState().MoveDirection = moveDirection;
        }
        GetCurrentInputState().SprintAction = sprintAction;
        // GetCurrentInputState().AttackAction = attackAction;
    }

    private void HandleInteractInput()
    {
        GetCurrentInputState().InteractAction = true;
    }

    private PlayerInputState GetCurrentInputState()
    {
        if (_currentInputState == null)
        {
            _currentInputState = new PlayerInputState();
        }
        return _currentInputState;
    }

    protected DirectionEnum Vector2DirectionEnum(Vector2 direction)
    {
        if (direction == Vector2.zero)
            return DirectionEnum.DIRECTION_NONE;



        var defaultDirection = Vector2.right;
        var angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        if (angle < 0)
        {
            angle = (360f + angle) % 360;
        }

        if (DirectionWithUpperBandMapping.Count == 0)
        {
            SetupDirectionWithUppderBandMapping();
        }

        foreach (var (directionEnum, upperBand) in DirectionWithUpperBandMapping)
        {
            if (angle <= upperBand)
            {
                return directionEnum;
            }
        }
        return DirectionEnum.UPPER_RIGHT;
    }

    private void SetupDirectionWithUppderBandMapping()
    {
        DirectionWithUpperBandMapping.Clear();
        DirectionWithUpperBandMapping.Add((DirectionEnum.UP, UpperBandFromIndex(0)));
        DirectionWithUpperBandMapping.Add((DirectionEnum.UPPER_RIGHT, UpperBandFromIndex(1)));
        DirectionWithUpperBandMapping.Add((DirectionEnum.RIGHT, UpperBandFromIndex(2)));
        DirectionWithUpperBandMapping.Add((DirectionEnum.LOWER_RIGHT, UpperBandFromIndex(3)));
        DirectionWithUpperBandMapping.Add((DirectionEnum.DOWN, UpperBandFromIndex(4)));
        DirectionWithUpperBandMapping.Add((DirectionEnum.LOWER_LEFT, UpperBandFromIndex(5)));
        DirectionWithUpperBandMapping.Add((DirectionEnum.LEFT, UpperBandFromIndex(6)));
        DirectionWithUpperBandMapping.Add((DirectionEnum.UPPER_LEFT, UpperBandFromIndex(7)));
    }

    private float UpperBandFromIndex(int index)
    {
        var degreePerBand = 360f / 8f;
        var initialOffset = degreePerBand / 2f;
        return (index + 1) * degreePerBand - initialOffset;
    }
}
