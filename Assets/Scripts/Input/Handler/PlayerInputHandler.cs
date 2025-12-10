using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] PlayerCharacter _playerCharacter;
    List<(DirectionEnum direction, float upperBand)> DirectionWithUpperBandMapping = new ();
    private PlayerInputState _currentInputState = null;

    // TODO: Refactor into a different file
    class PlayerInputState
    {
        public Vector2 LookDirection = Vector2.zero;
        public Vector2 MoveDirection = Vector2.zero;
        public bool AttackAction = false;
    }


    // Start is called before the first frame update
    void OnEnable()
    {
        PlayerInputMapper.OnSendMoveInput += HandleMoveInput;
    }


    private void OnDisable()
    {
        PlayerInputMapper.OnSendMoveInput -= HandleMoveInput;
    }


    private void FixedUpdate()
    {
        if (_currentInputState != null)
        {
            HandlePlayerMovement();
            _currentInputState = null;
        }
    }

    private void HandlePlayerMovement()
    {
        var hasMovementInput = _currentInputState.MoveDirection != Vector2.zero;
        _playerCharacter.SetMovement(hasMovementInput ? 1 : 0);
        var characterFacingVector = hasMovementInput
            ? _currentInputState.MoveDirection 
            : LookDirectionFromMousePosition(_currentInputState.LookDirection);
        _playerCharacter.SetDirection(Vector2DirectionEnum(characterFacingVector));

        if (hasMovementInput)
        {
            // TODO: Replace hardcoded speed with character speed attribute
            _playerCharacter.transform.position += (Vector3)_currentInputState.MoveDirection * 3 * Time.fixedDeltaTime;
        }
    }

    private Vector2 LookDirectionFromMousePosition(Vector2 mousePosition)
    {
        var playerPos = Camera.main.WorldToScreenPoint(_playerCharacter.transform.position);
        var lookDirection = (mousePosition - (Vector2)playerPos).normalized;
        return lookDirection;
    }

    // Creating a Stateful Input Handler for Player Character already thinking on having multiplayer in future
    // Sets up the last input received for the player character to be able to handle on the next server tick
    private void HandleMoveInput(Vector2 lookDirection, Vector2 moveDirection, bool attackAction)
    {
        if (lookDirection != Vector2.zero)
        {
            GetCurrentInputState().LookDirection = lookDirection;
        }

        if(moveDirection != Vector2.zero)
        {
            GetCurrentInputState().MoveDirection = moveDirection;
        }

        GetCurrentInputState().AttackAction = attackAction;
    }

    private PlayerInputState GetCurrentInputState()
    {
        if( _currentInputState == null)
        {
            _currentInputState = new PlayerInputState();
        }
        return _currentInputState;
    }

    private DirectionEnum Vector2DirectionEnum(Vector2 direction)
    {
        if (direction == Vector2.zero)
            return DirectionEnum.DIRECTION_NONE;

        

        var defaultDirection = Vector2.right;
        var angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        if (angle < 0)
        {
            angle = (360f + angle) % 360;
        }
        Debug.Log($"Angle calculated: {angle}");    

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
