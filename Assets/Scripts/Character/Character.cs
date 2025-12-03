using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public DirectionEnum CurrentDiretion => currentDirection;
    protected DirectionEnum currentDirection = DirectionEnum.DIRECTION_NONE;

    public void SetDirection(DirectionEnum direction)
    {
        if(direction == currentDirection) return;
        currentDirection = direction;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
