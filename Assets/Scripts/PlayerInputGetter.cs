using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputGetter : MonoBehaviour
{

    PlayerHub _hub;
    PlayerInput _playerInput;

    private void Awake()
    {
        _hub = GetComponent<PlayerHub>();
        _playerInput = GetComponent<PlayerInput>();
    }

    void OnMove(InputValue value)
    {
        _hub.InputMovement = value.Get<Vector2>();
    }

    void OnRightStick(InputValue value)
    {
        // _hub.InputRightStick = value.Get<Vector2>();
    }

    void OnBtn1(InputValue value)
    {
        _hub.InputBtn1 = value.isPressed;
    }

    void OnBtn2(InputValue value)
    {
        _hub.InputBtn2 = value.isPressed;
    }

}
