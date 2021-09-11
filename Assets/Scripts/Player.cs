using UnityEngine;

public class Player : Character
{
    public Projectile projectile;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        direction = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Fire1"))
        {
            HandleAttack();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            FireProjectile();
        }

        if (Input.GetButtonDown("Jump"))
        {
            HandleJump();
        }
    }

    private void FireProjectile()
    {
        if (projectile)
        {
            var proj = Instantiate(projectile, attackPoint.position, attackPoint.rotation);
            proj.direction = transform.localScale.x;
            proj.enemyLayer = enemyLayer;
        }
    }

    public void Heal(int amount)
    {
        var newHealth = currentHealth + amount;
        currentHealth = newHealth > maxHealth ? maxHealth : newHealth;
    }
}
