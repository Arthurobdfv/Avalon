using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper class for DirectionEnum conversions.
/// </summary>
public static class DirectionEnumHelper
{
    private static List<(DirectionEnum direction, float upperBand)> _directionWithUpperBandMapping = new List<(DirectionEnum, float)>();

    public static DirectionEnum Vector2DirectionEnum(Vector2 direction)
    {
        if (direction == Vector2.zero)
            return DirectionEnum.DIRECTION_NONE;

        var angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        if (angle < 0)
        {
            angle = (360f + angle) % 360;
        }

        if (_directionWithUpperBandMapping.Count == 0)
        {
            SetupDirectionWithUpperBandMapping();
        }

        foreach (var (directionEnum, upperBand) in _directionWithUpperBandMapping)
        {
            if (angle <= upperBand)
            {
                return directionEnum;
            }
        }
        return DirectionEnum.UPPER_RIGHT;
    }

    private static void SetupDirectionWithUpperBandMapping()
    {
        _directionWithUpperBandMapping.Clear();
        _directionWithUpperBandMapping.Add((DirectionEnum.UP, UpperBandFromIndex(0)));
        _directionWithUpperBandMapping.Add((DirectionEnum.UPPER_RIGHT, UpperBandFromIndex(1)));
        _directionWithUpperBandMapping.Add((DirectionEnum.RIGHT, UpperBandFromIndex(2)));
        _directionWithUpperBandMapping.Add((DirectionEnum.LOWER_RIGHT, UpperBandFromIndex(3)));
        _directionWithUpperBandMapping.Add((DirectionEnum.DOWN, UpperBandFromIndex(4)));
        _directionWithUpperBandMapping.Add((DirectionEnum.LOWER_LEFT, UpperBandFromIndex(5)));
        _directionWithUpperBandMapping.Add((DirectionEnum.LEFT, UpperBandFromIndex(6)));
        _directionWithUpperBandMapping.Add((DirectionEnum.UPPER_LEFT, UpperBandFromIndex(7)));
    }

    private static float UpperBandFromIndex(int index)
    {
        var degreePerBand = 360f / 8f;
        var initialOffset = degreePerBand / 2f;
        return (index + 1) * degreePerBand - initialOffset;
    }
}
