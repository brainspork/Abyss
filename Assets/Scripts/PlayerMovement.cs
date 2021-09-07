using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    public float speed;

    private float horizontalMovement;
    internal int currentDirection = 1;
    internal bool stopMovement = false;

    private Rigidbody2D rb2d;
    private Animator anim;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        horizontalMovement = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        if (!stopMovement)
        {
            rb2d.velocity = new Vector2(horizontalMovement * speed, rb2d.velocity.y);

            SwapDirection();

            anim.SetFloat("f_speed", Mathf.Abs(horizontalMovement));
        }
    }

    private void SwapDirection()
    {
        var newDirection = horizontalMovement > 0 ? 1 : horizontalMovement < 0 ? -1 : 0;

        if (newDirection != 0 && newDirection != currentDirection)
        {
            currentDirection = newDirection;
            transform.localScale = new Vector3(currentDirection, transform.localScale.y);
        }
    }
}
