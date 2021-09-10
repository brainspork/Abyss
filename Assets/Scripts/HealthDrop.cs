using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class HealthDrop : Drop
{
    [SerializeField] private int HealthAmount;
    [SerializeField] private float spawnXMagnitude;
    [SerializeField] private float spawnForceMult;

    private Rigidbody2D rb2d;
    private Animator anim;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        var forceVector = new Vector2(Random.Range(-spawnXMagnitude, spawnXMagnitude), 1);
        rb2d.AddForce(forceVector * spawnForceMult, ForceMode2D.Impulse);
    }

    public override void PickupDrop(Player player)
    {
        player.Heal(HealthAmount);
    }

    public override void DestroyDrop()
    {
        gameObject.layer = 9;
        anim.SetTrigger("t_pickup");

        Invoke("Destroy", 1);
    }

    private void Destroy () => base.DestroyDrop();
}
