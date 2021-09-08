using UnityEngine;

public class Player : Character
{
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

        if (Input.GetButtonDown("Jump"))
        {
            HandleJump();
        }
    }
}
