using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    public float damage = 10f;
    public float lifetime = 5f;

    void Start()
    {
        Destroy(gameObject, lifetime); // Destroy after time
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if it hits the player
        {
            UnitHealth playerHealth = other.GetComponent<UnitHealth>();
            if (playerHealth != null)
            {
                playerHealth.takeDamage(damage);
                Destroy(gameObject); // Destroy magic after hit
            }
        }
    }
}
