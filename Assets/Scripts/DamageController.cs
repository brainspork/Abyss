using UnityEngine;

public class DamageController : MonoBehaviour
{
    public int maxHealth;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
        if (collision.CompareTag("Weapon"))
        {
            maxHealth--;
        }
    }
}
