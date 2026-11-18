using UnityEngine;

/// <summary>
/// Player input state packet sent from client to server each tick.
/// Contains the current input state: movement direction, look direction, and action states.
/// </summary>
[AvalonAuthorized]
public class PlayerInputState : AvalonPacket
{
    public Vector2 LookDirection = Vector2.zero;
    public Vector2 MoveDirection = Vector2.zero;
    public bool AttackAction = false;
    public bool SprintAction = false;
    public bool InteractAction = false;
}
