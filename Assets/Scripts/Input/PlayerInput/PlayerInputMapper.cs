using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputMapper : MonoBehaviour
{
    InputAction _moveAction;
    InputAction _lookAction;
    InputAction _attackAction;
    InputAction _sprintAction;
    InputAction _interact;
    // Start is called before the first frame update
    void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _lookAction = InputSystem.actions.FindAction("Look");
        _attackAction = InputSystem.actions.FindAction("Attack");
        _sprintAction = InputSystem.actions.FindAction("Sprint");
        _interact = InputSystem.actions.FindAction("Interact");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveDirection = _moveAction.ReadValue<Vector2>();
        Vector2 lookDirection = _lookAction.ReadValue<Vector2>();
        bool sprinting = _sprintAction.IsPressed();
        bool attackAction = _attackAction.ReadValue<float>() > 0;
        OnSendMoveInput?.Invoke(lookDirection, moveDirection, sprinting);
        bool interacting = _interact.IsPressed();
        if (interacting)
            OnSentInteractInput?.Invoke();
    }

    public delegate void SendMoveInput(Vector2 lookDirection, Vector2 moveDirection, bool sprinting);
    public static event SendMoveInput OnSendMoveInput;
    public delegate void SendInteractInput();
    public static event SendInteractInput OnSentInteractInput;
}
