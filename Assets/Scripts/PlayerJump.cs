using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerJump : MonoBehaviour
{
    public float jumpForce;

    [Header("Grounded Settings")]
    [SerializeField] private bool grounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRaduis;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb2d;
    private Animator anim;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRaduis, groundLayer);

        if (grounded)
        {
            anim.SetLayerWeight(1, 0);
            ResetJumpAnimation();
            SetFallingAnimation(false);
        }
        else
        {
            anim.SetLayerWeight(1, 1);
        }

        if (Input.GetButtonDown("Jump") && grounded)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            TriggerJumpAnimation();
        }

        if (rb2d.velocity.y < 0)
        {
            SetFallingAnimation(true);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, groundCheckRaduis);
    }

    private void SetFallingAnimation(bool isFalling)
    {
        anim.SetBool("b_isFalling", isFalling);
    }

    private void TriggerJumpAnimation()
    {
        anim.SetTrigger("t_jump");
    }

    private void ResetJumpAnimation()
    {
        anim.ResetTrigger("t_jump");
    }
}
