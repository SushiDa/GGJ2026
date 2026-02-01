using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] PlayerHub _hub;
    [SerializeField] Rigidbody2D _rb;
    Animator animator;
    void Awake()
    {
       animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {    
        animator.SetBool("isJumping", !_hub.onGround);

        if (_rb.linearVelocityY < 0)
        {
            animator.SetBool("isFalling", true);
        }
        else
        {
            animator.SetBool("isFalling", false);
        }
        animator.SetFloat("horizontalSpeed",  Mathf.Abs(_rb.linearVelocityX));
        if (_hub.isDashing == true)
        {
            animator.SetBool("isDashing", true);
        }
        else
        {
            if (_hub.isDashing == false)
            {
                animator.SetBool("isDashing", false);
            }
        }
        animator.SetBool("isCrawling", !_hub.morphForm);
        animator.SetFloat("verticalSpeed", _rb.linearVelocityY);
        animator.SetBool("morphing", _hub.morphing);
    }
}
