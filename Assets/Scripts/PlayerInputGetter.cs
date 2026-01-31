using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputGetter : MonoBehaviour
{

    PlayerHub _hub;

    private void Awake()
    {
        _hub = GetComponent<PlayerHub>();
    }

    void OnMove(InputValue value)
    {
        _hub.InputMovement = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            _hub.JumpEvent?.Invoke();
        }
    }

    void OnDash(InputValue value)
    {
        if (value.isPressed)
        {
            _hub.JumpEvent?.Invoke();
        }
    }

    void OnRoll(InputValue value)
    {
        if (value.isPressed)
        {
            _hub.JumpEvent?.Invoke();
        }
    }

    void OnApplyMask1(InputValue value)
    {
        if (value.isPressed)
        {
            _hub.BlueMaskEvent?.Invoke();
        }
    }

    void OnApplyMask2(InputValue value)
    {
        if (value.isPressed)
        {
            _hub.RedMaskEvent?.Invoke();
        }
    }

    void OnApplyMask3(InputValue value)
    {
        if (value.isPressed)
        {
            _hub.YellowMaskEvent?.Invoke();
        }
    }

    void OnRemoveMask(InputValue value)
    {
        if (value.isPressed)
        {
            _hub.MaskOffEvent?.Invoke();
        }
    }

}
