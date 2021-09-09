using UnityEngine;

public class Enemy : Character
{
    [Header("Enemy AI")]
    [SerializeField] private bool debug;
    [SerializeField] private float lookDistance;
    [SerializeField] private GameObject playerGameObject;

    private bool hasSeenPlayer = false;

    public override void Start()
    {
        base.Start();

        if (playerGameObject == null)
        {
            playerGameObject = GameObject.Find("Player");
        }
    }

    public override void Update()
    {
        base.Update();

        if (playerGameObject != null)
        {
            var playerPosition = playerGameObject.transform.position;
            var distanceFromPlayer = Vector2.Distance(playerPosition, attackPoint.position);

            if (distanceFromPlayer < lookDistance)
            {
                hasSeenPlayer = true;
            }

            if (Mathf.Abs(distanceFromPlayer) < attackDistance || (debug && Input.GetButtonDown("Fire1")))
            {
                if (attackDelayTimer <= 0)
                {
                    isAttacking = true;

                    attackDelayTimer = attackDelay;
                    anim.SetLayerWeight(2, 1);
                    anim.SetTrigger("t_startAttackA");

                    Invoke("HandleAttack", attackDelay);
                }
                else
                {
                    anim.ResetTrigger("t_startAttackA");
                }
            }

            if (hasSeenPlayer)
            {
                direction = transform.position.x > playerPosition.x ? -1 : 1;
            }
        }
        else
        {
            direction = 0;
        }
    }
}