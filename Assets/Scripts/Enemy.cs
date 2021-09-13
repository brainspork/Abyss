using UnityEngine;

public class Enemy : Character
{
    [Header("Drop Details")]
    [SerializeField] protected Drop[] droppableItems;

    protected bool hasDropped;
    protected Drop dropItem;

    public override void Start()
    {
        base.Start();

        hasDropped = false;
        dropItem = droppableItems[Random.Range(0, droppableItems.Length)];
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // if enemy has an item to drop, has died and hasn't dropped yet, drop item
        if (dropItem && isDead && !hasDropped)
        {
            hasDropped = true;

            if (Random.Range(0f, 1f) <= dropItem.dropChance)
            {
                // TODO: maybe drop position should be provided so bosses can drop on map somewhere?
                Instantiate(dropItem, transform.position, transform.rotation);
            }
        }
    }
}
