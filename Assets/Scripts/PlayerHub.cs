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
    internal bool onGround = false;
    internal bool isDashing = false;
    internal bool canDash = true;
    internal bool morphForm = true;
    // internal bool canMorph = true;
    internal bool morphing = false;
    internal bool morphInProgress = false;

    internal bool hasJumpAbility = false;
    internal bool hasDashAbility = false;

    [SerializeField] Rigidbody2D _rb;
    public LayerMask layerMask;
    [SerializeField] SpriteRenderer renderer;
    private void Awake()
    {
        Mask1Event += TryApplyMask1;
        Mask2Event += TryApplyMask2;
        Mask3Event += TryApplyMask3;
        MaskOffEvent += TryMaskOff;
    }

    private void OnDestroy()
    {
        Mask1Event -= TryApplyMask1;
        Mask2Event -= TryApplyMask2;
        Mask3Event -= TryApplyMask3;
        MaskOffEvent -= TryMaskOff;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       // CheckRoof();
        FlipSprite();
        CheckGround();
        ReloadDashing();
        // CheckIsMoving();
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
    /*void CheckIsMoving()
    {
        if (_rb.linearVelocityX != 0)
        {
            canMorph = false;
        }
        else
        {
            canMorph = true;
        }
    }*/


    private void TryApplyMask1()
    {
        if (!onGround) return;
        if(GlobalMaskManager.TryAddMask(PrimaryColorMask.MASK_1))
        {
            hasDashAbility = true;
        }
    }
    private void TryApplyMask2()
    {
        if (!onGround) return;
        if(GlobalMaskManager.TryAddMask(PrimaryColorMask.MASK_2))
        {
            hasJumpAbility = true;
        }

    }
    private void TryApplyMask3()
    {
        if (!onGround || morphInProgress) return;
        bool success = GlobalMaskManager.TryAddMask(PrimaryColorMask.MASK_3);
        if (success)
        {
            morphForm = true;
            RollEvent?.Invoke();
        }
    }

    private void TryMaskOff()
    {
        if (!onGround) return;
        if(GlobalMaskManager.CurrentMaskStack.TryPeek(out var mask))
        {
            switch(mask)
            {
                case PrimaryColorMask.MASK_1:
                    if (GlobalMaskManager.TryRemoveMask()) hasDashAbility = false;
                    break;
                case PrimaryColorMask.MASK_2:
                    if (GlobalMaskManager.TryRemoveMask()) hasJumpAbility = false;
                    break;
                case PrimaryColorMask.MASK_3:
                    if (!morphInProgress && GlobalMaskManager.TryRemoveMask())
                    {
                        morphForm = false;
                        RollEvent?.Invoke();
                    }
                    break;
            }
        }
        else
        {
            GlobalMaskManager.TryRemoveMask();
        }
    }

}
