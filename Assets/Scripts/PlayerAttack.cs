using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAttack : MonoBehaviour
{
    public float attackTime;
    public float attackCoolDown;
    public float attackDistance;
    
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;

    private int attackCount = 0;
    private float attackTimeTimer = 0;
    private float attackCoolDownTimer = 0;

    private Dictionary<int, string> attackTriggers = new Dictionary<int, string>
    {
        { 1, "t_attackA" },
        { 2, "t_attackB" }
    };

    private Animator anim;
    private PlayerMovement playerMovement;

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void Update()
    {
        // decrement attack timers
        if (attackCoolDownTimer > 0)
        {
            attackCoolDownTimer -= Time.deltaTime;
        }

        if (attackTimeTimer > 0)
        {
            attackTimeTimer -= Time.deltaTime;
            playerMovement.stopMovement = true;
        }

        // if player presses fire and attack count is less than anim triggers
        if (Input.GetButtonDown("Fire1") && attackCount < attackTriggers.Count)
        {
            // increment attack count
            attackCount++;

            // set increment attack cool down and reset attackTimeTimer for hiding layer
            attackCoolDownTimer += attackCoolDown;
            attackTimeTimer = attackTime;

            // make attack layer visible
            anim.SetLayerWeight(2, 1);

            // set trigger for animation based on attack count
            anim.SetTrigger(attackTriggers[attackCount]);

            var enemyColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackDistance, enemyLayer);

            foreach(var collider in enemyColliders)
            {
                var damageController = collider.GetComponent<DamageController>();
                damageController.maxHealth--;
            }
        }

        // hide attack layer when animation is over
        if (attackTimeTimer <= 0)
        {
            anim.SetLayerWeight(2, 0);
            playerMovement.stopMovement = false;
        }

        // reset attack count and triggers when cool down is over
        if (attackCoolDownTimer <= 0)
        {
            attackCount = 0;

            foreach(var key in attackTriggers.Keys)
            {
                anim.ResetTrigger(attackTriggers[key]);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackDistance);
    }
}
