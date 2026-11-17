using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public DirectionEnum CurrentDiretion => currentDirection;
    public int CurrentMovement => currentMovement;
    protected DirectionEnum currentDirection = DirectionEnum.DIRECTION_NONE;
    protected int currentMovement = 0;


    public void SetDirection(DirectionEnum direction)
    {
        if(direction == currentDirection) return;
        currentDirection = direction;
    }

    public void SetMovement(int movement)
    {
        if(movement == currentMovement) return;
        currentMovement = movement;
    }
}
