using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerHub _hub;
    Rigidbody2D _rb;

    [SerializeField] float MoveSpeed;
    [SerializeField] float JumpVelocity;
    [SerializeField] float DefaultGravityScale;
    [SerializeField] float FallGravityScale;
    [SerializeField] float FallVelocityThreshold;

    void Awake()
    {
        _hub = GetComponent<PlayerHub>();
        _rb = GetComponent<Rigidbody2D>();

        _hub.JumpEvent += OnJump;
    }

    private void FixedUpdate()
    {
        GravityCheck();

        if (!_hub.canMove) return;

        Vector2 movement = Vector2.zero;

        Vector2 horizontalMovement = _hub.InputMovement.x * Vector2.right;
        //if (_hub.InputMovement.magnitude > .1f && _hub.canMove && !LockStrafe)
        if (_hub.InputMovement.magnitude >= .05f)
        {
            movement = horizontalMovement.normalized * MoveSpeed;
        }

        _rb.linearVelocityX = movement.x;
    }


    private void GravityCheck()
    {
        _rb.gravityScale = _rb.linearVelocityY > FallVelocityThreshold ? DefaultGravityScale : FallGravityScale;
    }

    private void OnJump()
    {
        if (!_hub.canJump) return;
        _rb.linearVelocityY = JumpVelocity;

    }
}
