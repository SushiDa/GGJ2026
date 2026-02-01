using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerHub _hub;
    Rigidbody2D _rb;
    BoxCollider2D _collider;

    [SerializeField] float MoveSpeed;
    [SerializeField] float JumpVelocity;
    [SerializeField] float DefaultGravityScale;
    [SerializeField] float FallGravityScale;
    [SerializeField] float DashVelocity;
    [SerializeField] float FallVelocityThreshold;
    [SerializeField] float DashTimer; 
    LayerMask layerMask;

    void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _hub = GetComponent<PlayerHub>();
        _rb = GetComponent<Rigidbody2D>();

        _hub.JumpEvent += OnJump;
        _hub.RollEvent += OnCrawl;
        _hub.DashEvent += Dashing;
    }

    private void FixedUpdate()
    {
        if (!_hub.isDashing){

            if (!_hub.canMove) return;

            GravityCheck();

            Vector2 movement = Vector2.zero;

            Vector2 horizontalMovement = _hub.InputMovement.x * Vector2.right;
            //if (_hub.InputMovement.magnitude > .1f && _hub.canMove && !LockStrafe)
            if (_hub.InputMovement.magnitude >= .05f)
            {
                movement = horizontalMovement.normalized * MoveSpeed;
            }
            if (movement.x < 0)
            {
                _hub.lookRight = false;
            }
            else if (movement.x >0)
            {
                _hub.lookRight = true;
            }
                _rb.linearVelocityX = movement.x;
        }
        else
        {
            Dashing();
        }
    }


    private void GravityCheck()
    {
        _rb.gravityScale = _rb.linearVelocityY > FallVelocityThreshold ? DefaultGravityScale : FallGravityScale;
    }

    private void OnJump()
    {
        if (!_hub.hasJumpAbility) return;
        if (_hub.onGround == true && _hub.isDashing == false ) 
        {
            SFXPlayer.Play(_hub.morphForm? "Jump" : "SlimeJump");
            _rb.linearVelocityY = JumpVelocity;
        }
    }

    private void Dashing()
    {
        if (_hub.isDashing) return;
        if (_hub.canDash == false || !_hub.hasDashAbility) return;

        SFXPlayer.Play("Dash");
        StartCoroutine(Dash());
    }
    IEnumerator Dash()
    {
        _hub.canDash = false;
        _hub.isDashing = true;
        _rb.linearVelocityY = 0;
        _rb.gravityScale = 0;
        if (_hub.lookRight == false)
        {
            _rb.linearVelocityX = DashVelocity * -1;
        }
        else { 
            _rb.linearVelocityX = DashVelocity;
        }
        yield return new WaitForSeconds(DashTimer);
        {
            _hub.isDashing = false;
        }
    }
    void OnCrawl()
    {
        
        _hub.morphInProgress = true;
        _rb.linearVelocityX = 0;
        _rb.gravityScale = 0;
        _hub.canMove = false;
        DOTween.Sequence().AppendInterval(.5f).AppendCallback(() => { 
            _hub.morphInProgress = false;
            _hub.canMove = true;
        });
        SFXPlayer.Play("Morph");
        if (_hub.morphForm == true) {
            _hub.morphing = false;
            _collider.size = new Vector2(1, 0.5f);
            _collider.offset = new Vector2(0, 0.25f);
            _hub.morphForm = false;
        }
        else if (_hub.morphForm == false)
        {
            _hub.morphing = true;
            _collider.size = new Vector2(1, 1);
            _collider.offset = new Vector2(0, 0.5f);
            _hub.morphForm = true;
        }
    }
}
