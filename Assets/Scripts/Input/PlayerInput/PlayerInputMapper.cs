using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputMapper : MonoBehaviour
{
    InputAction _moveAction;
    InputAction _lookAction;
    InputAction _attackAction;
    // Start is called before the first frame update
    void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _lookAction = InputSystem.actions.FindAction("Look");
        _attackAction = InputSystem.actions.FindAction("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveDirection = _moveAction.ReadValue<Vector2>();
        Vector2 lookDirection = _lookAction.ReadValue<Vector2>();
        bool attackAction = _attackAction.ReadValue<float>() > 0;
        OnSendMoveInput?.Invoke(lookDirection, moveDirection, attackAction);
    }

    public delegate void SendMoveInput(Vector2 lookDirection, Vector2 moveDirection, bool attackAction);
    public static event SendMoveInput OnSendMoveInput;
}
