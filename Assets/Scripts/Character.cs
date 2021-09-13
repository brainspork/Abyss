using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour
{
    [Header("Movement Details")]
    [SerializeField] protected float speed;
    [SerializeField] protected float direction;

    protected int currentDirection = 1;
    protected bool isAttacking = false;

    [Header("Attack Details")]
    [SerializeField] protected float attackTime;
    [SerializeField] protected float attackCoolDown;
    [SerializeField] protected float attackDistance;
    [SerializeField] protected int attackDamage;
    [SerializeField] protected float attackDelay;
    [SerializeField] protected float attackStunTime;
    [SerializeField] protected int maxCombo;
    
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask enemyLayer;

    [SerializeField] protected string[] attackTriggers;

    protected int attackCount;
    protected float attackTimeTimer;
    protected float attackCoolDownTimer;
    protected float attackDelayTimer;

    [Header("Jump Details")]
    [SerializeField] protected float jumpForce;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckRaduis;
    [SerializeField] protected LayerMask groundLayer;

    protected bool grounded;
    
    [Header("Health Detials")]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected Slider healthBar;

    internal bool isDead;

    protected int currentHealth;
    protected float stunTimer;
    protected bool isStunned;

    protected GameManager gm;

    protected Rigidbody2D rb2d;
    protected Animator anim;

    public virtual void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        attackCount = 0;
        attackTimeTimer = 0;
        attackCoolDownTimer = 0;
        attackDelayTimer = 0;

        stunTimer = 0;
        currentHealth = maxHealth;
        isDead = false;

        if (healthBar)
        {
            healthBar.maxValue = maxHealth;
        }
    }

    public virtual void Update()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRaduis, groundLayer);

        if (grounded)
        {
            anim.SetLayerWeight(1, 0);
            anim.ResetTrigger("t_jump");
            anim.SetBool("b_isFalling", false);
        }
        else
        {
            anim.SetLayerWeight(1, 1);
        }

        if (rb2d.velocity.y < 0)
        {
            anim.SetBool("b_isFalling", true);
        }

        // decrement attack timers
        if (attackCoolDownTimer > 0)
        {
            attackCoolDownTimer -= Time.deltaTime;
        }

        if (attackTimeTimer > 0 || attackDelayTimer > 0)
        {
            isAttacking = true;
        }
        else
        {
            anim.SetLayerWeight(2, 0);
            isAttacking = false;
        }

        if (attackTimeTimer > 0)
        {
            attackTimeTimer -= Time.deltaTime;
        }

        if (stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
            isStunned = true;
        }

        if (stunTimer <= 0)
        {
            isStunned = false;
            anim.ResetTrigger("t_hit");
        }

        if (attackDelayTimer > 0)
        {
            attackDelayTimer -= Time.deltaTime;
        }

        // reset attack count and triggers when cool down is over
        if (attackCoolDownTimer <= 0)
        {
            attackCount = 0;

            foreach (var trigger in attackTriggers)
            {
                anim.ResetTrigger(trigger);
            }
        }

        if (healthBar)
        {
            healthBar.value = currentHealth;
        }
    }

    public virtual void FixedUpdate()
    {
        // destroy character when health drops below zero
        if (currentHealth <= 0)
        {
            isDead = true;
        }

        HandleMovement();
    }

    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 12)
        {
            currentHealth = 0;
        }
    }

    protected void Move()
    {
        rb2d.velocity = new Vector2(direction * speed, rb2d.velocity.y);
    }

    protected void Jump()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
    }

    protected void Attack()
    {
        // make attack layer visible
        anim.SetLayerWeight(2, 1);

        // set trigger for animation based on attack count
        anim.SetTrigger(attackTriggers[attackCount % attackTriggers.Length]);

        // increment attack count
        attackCount++;

        // set increment attack cool down and reset attackTimeTimer for hiding layer
        attackCoolDownTimer += attackCoolDown;
        attackTimeTimer = attackTime;

        var enemyColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackDistance, enemyLayer);

        foreach (var collider in enemyColliders)
        {
            var enemyCharacter = collider.GetComponent<Character>();
            enemyCharacter.TakeDamage(attackDamage, attackStunTime);
        }
    }

    public void TakeDamage(int damage, float stunTime)
    {
        if (!isStunned)
        {
            stunTimer = stunTime;
        }

        TakeDamage(damage);
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        anim.SetTrigger("t_hit");
    }

    protected virtual void HandleMovement()
    {
        if (!isAttacking && !isStunned)
        {
            Move();

            TurnAround();

            anim.SetFloat("f_speed", Mathf.Abs(direction));
        }
    }

    protected virtual void HandleAttack()
    {
        // we can attack if character isn't stunned and hasn't exausted combo
        if (attackCount < maxCombo && !isStunned)
        {
            attackDelayTimer = attackDelay;
            Attack();
        }
    }

    protected virtual void HandleJump()
    {
        if (grounded)
        {
            Jump();
            anim.SetTrigger("t_jump");
        }
    }

    protected void TurnAround()
    {
        var newDirection = direction > 0 ? 1 : direction < 0 ? -1 : 0;

        if (newDirection != 0 && newDirection != currentDirection)
        {
            currentDirection = newDirection;
            transform.localScale = new Vector3(currentDirection, transform.localScale.y);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, groundCheckRaduis);
        Gizmos.DrawWireSphere(attackPoint.position, attackDistance);
    }
}
