using UnityEngine;

/// <summary>
/// Client-side input handler. Collects player input and sends it to the server.
/// Entity state updates are received via EntitySpawnPacket handled by EntitySpawner.
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInputState _currentInputState = null;

    [SerializeField] ClientCommunicationLayerManager _clientCommunicationLayerManager;
    ClientCommunicationLayerManager ClientCommunicationLayerManager
    {
        get
        {
            if (_clientCommunicationLayerManager == null)
            {
                _clientCommunicationLayerManager = FindFirstObjectByType<ClientCommunicationLayerManager>() ?? throw new System.NullReferenceException("No ClientCommunicationLayerManager found");
            }
            return _clientCommunicationLayerManager;
        }
    }


    void OnEnable()
    {
        PlayerInputMapper.OnSendMoveInput += HandleMoveInput;
        PlayerInputMapper.OnSentInteractInput += HandleInteractInput;
    }

    private void OnDisable()
    {
        PlayerInputMapper.OnSendMoveInput -= HandleMoveInput;
        PlayerInputMapper.OnSentInteractInput -= HandleInteractInput;
    }


    private void FixedUpdate()
    {
        // Send input to server (or local server in single-player mode)
        if (ClientCommunicationLayerManager != null && ClientCommunicationLayerManager != null)
        {
            ClientCommunicationLayerManager.Send(_currentInputState);
            _currentInputState = null;
        }
    }

    // Creates a stateful input for the player character to be sent on the next server tick
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
}
