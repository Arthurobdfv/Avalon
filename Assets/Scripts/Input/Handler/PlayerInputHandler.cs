using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    List<(DirectionEnum direction, float upperBand)> DirectionWithUpperBandMapping = new ();


    // Start is called before the first frame update
    void OnEnable()
    {
        PlayerInputMapper.OnSendMoveInput += HandleMoveInput;
    }


    private void OnDisable()
    {
        PlayerInputMapper.OnSendMoveInput -= HandleMoveInput;
    }
    private void HandleMoveInput(Vector2 lookDirection, Vector2 moveDirection, bool attackAction)
    {

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
