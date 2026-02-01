using System;
using UnityEngine;

public class PlayerHub : MonoBehaviour
{

    internal Vector2 InputMovement;
    internal Action JumpEvent;
    internal Action DashEvent;
    internal Action RollEvent;
    internal Action Mask1Event;
    internal Action Mask2Event;
    internal Action Mask3Event;
    internal Action MaskOffEvent;

    internal bool lookRight = true;
    internal bool canMove = true;
    internal bool canJump = true;
    internal bool onGround = false;
    internal bool isDashing = false;
    internal bool canDash = true;
    internal bool morphForm = true;
    internal bool canMorph = true;
    internal bool morphing = false;

    [SerializeField] Rigidbody2D _rb;
    public LayerMask layerMask;
    [SerializeField] SpriteRenderer renderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckRoof();
        FlipSprite();
        CheckGround();
        ReloadDashing();
        CheckIsMoving();
    }

    void CheckGround()
    {
        Collider2D getGround = Physics2D.OverlapBox(transform.position, new Vector2(0.25f, 0.1f), 0, layerMask);
        onGround = getGround != null;
    }

    public void FlipSprite()
    {
        renderer.flipX = lookRight;
    }

    void ReloadDashing()
    {
        if (isDashing == false && onGround == true)
        {
            canDash = true;
        }
    }
    void CheckIsMoving()
    {
        if (_rb.linearVelocityX != 0)
        {
            canMorph = false;
        }
        else
        {
            canMorph = true;
        }
    }
    void CheckRoof()
    {
        Collider2D getRoof = Physics2D.OverlapBox(transform.position, new Vector2(0.1f, 1f), 0, layerMask);
       canMorph = getRoof != null;
    }
}
