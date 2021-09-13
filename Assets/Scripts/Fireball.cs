using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Fireball : Projectile
{
    private Animator anim;

    public override void Start()
    {
        base.Start();

        anim = GetComponent<Animator>();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        direction = 0;
        base.OnCollisionEnter2D(collision);
        gameObject.layer = 9;
    }

    public override void HandleHitObject()
    {
        anim.SetTrigger("t_hitObject");

        Invoke("Destroy", 1);
    }

    public void Destroy()
    {
        base.HandleHitObject();
    }
}
