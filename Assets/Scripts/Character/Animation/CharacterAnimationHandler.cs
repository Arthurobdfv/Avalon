using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationHandler : MonoBehaviour
{
    [SerializeField] Character _character;
    [SerializeField] Animator _animator;
    DirectionEnum _lastDirection = DirectionEnum.DIRECTION_NONE;
    int _currentMovement = -1;

    #region Animation Constants
    private const string DirectionXParam = "DirectionX";
    private const string DirectionYParam = "DirectionY";
    private const string MovementParam = "Movement";
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Change this to event based on character direction change
        UpdateAnimatorDirectionParams(_character.CurrentDiretion);
        UpdateAnimatorMovementParams(_character.CurrentMovement);
    }

    private void UpdateAnimatorMovementParams(int currentMovement)
    {
        if(_currentMovement == currentMovement) return;
        _currentMovement = currentMovement;
        _animator.SetInteger(MovementParam, currentMovement);
    }

    void UpdateAnimatorDirectionParams(DirectionEnum direction)
    {
        if(direction == _lastDirection) return;
        _lastDirection = direction;
        var vector = Vector2FromDirection(_lastDirection);
        _animator.SetFloat(DirectionXParam, vector.x);
        _animator.SetFloat(DirectionYParam, vector.y);
    }

    private Vector2 Vector2FromDirection(DirectionEnum direction)
    {
        var directionVector = Vector2.zero;
        switch(direction)
        {
            case DirectionEnum.UP:
                directionVector = new Vector2(0, 1);
                break;
            case DirectionEnum.UPPER_LEFT:
                directionVector = new Vector2(-1, 1);
                break;
            case DirectionEnum.UPPER_RIGHT:
                directionVector = new Vector2(1, 1);
                break;
            case DirectionEnum.RIGHT:
                directionVector = new Vector2(1, 0);
                break;
            case DirectionEnum.LOWER_RIGHT:
                directionVector = new Vector2(1, -1);
                break;
            case DirectionEnum.DOWN:
                directionVector = new Vector2(0, -1);
                break;
            case DirectionEnum.LOWER_LEFT:
                directionVector = new Vector2(-1, -1);
                break;
            case DirectionEnum.LEFT:
            default:
                directionVector = new Vector2(-1, 0);
                break;
            }
        return directionVector.normalized;
    }
}
