using UnityEngine;

public abstract class Drop : MonoBehaviour
{
    public float dropChance;

    public abstract void PickupDrop(Player player);

    public virtual void DestroyDrop()
    {
        Destroy(gameObject);
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PickupDrop(collision.gameObject.GetComponent<Player>());
            DestroyDrop();
        }
    }
}
