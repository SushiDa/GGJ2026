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
            _hub.DashEvent?.Invoke();
        }
    }

    void OnRoll(InputValue value)
    {
        if (value.isPressed)
        {
            _hub.RollEvent?.Invoke();
        }
    }

    void OnApplyMask1(InputValue value)
    {
        if (value.isPressed)
        {
            _hub.Mask1Event?.Invoke();
        }
    }

    void OnApplyMask2(InputValue value)
    {
        if (value.isPressed)
        {
            _hub.Mask2Event?.Invoke();
        }
    }

    void OnApplyMask3(InputValue value)
    {
        if (value.isPressed)
        {
            _hub.Mask3Event?.Invoke();
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
