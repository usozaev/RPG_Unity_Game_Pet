using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class MagicAttack : MonoBehaviour
{

    public float magicSpeed = 0.5f;
    public float magiclifetime = 5f;

    public float damageAmount = 50;
    private Rigidbody rb;

    private Vector3 shootDirection;
    // Start is called before the first frame update


    public void Initialize(Vector3 direction)
    {
        shootDirection = direction.normalized;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = shootDirection * magicSpeed;
        Destroy(gameObject, magiclifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        UnitHealth targetHealth = collision.gameObject.GetComponent<UnitHealth>();

        if (targetHealth != null)
        {
            targetHealth.takeDamage(damageAmount);
        }


    }

}

