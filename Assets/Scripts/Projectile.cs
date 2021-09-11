using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float maxRange;
    [SerializeField] protected int damage;
    [SerializeField] protected float stunTime;

    internal float direction;
    internal LayerMask enemyLayer;

    protected Vector2 startPositon;

    protected Rigidbody2D rb2d;

    public virtual void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        startPositon = transform.position;
    }

    public virtual void Update()
    {
        if (direction < 0)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y);
        }

        if (Mathf.Abs(Vector2.Distance(startPositon, transform.position)) >= maxRange)
        {
            HandleOutOfRange();
        }

        HandleMovement();
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (enemyLayer == (enemyLayer | (1 << collision.gameObject.layer)))
        {
            var enemy = collision.gameObject.GetComponent<Character>();

            if (enemy)
            {
                enemy.TakeDamage(damage, stunTime);
            }

            HandleHitObject();
        }
    }

    protected void Move()
    {
        rb2d.velocity = new Vector2(direction * speed, rb2d.velocity.y);
    }

    protected virtual void HandleMovement()
    {
        Move();
    }

    public virtual void HandleHitObject()
    {
        Destroy(gameObject);
    }

    public virtual void HandleOutOfRange()
    {
        Destroy(gameObject);
    }
}
