using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public Transform playerPosition;
    public float followDistance = 5f;
    public float speed = 3f;
    public float attackRange = 7f;
    public GameObject magicProjectilePrefab;
    public Transform magicSpawnAI;
    public float attackCooldown = 2f;
    public float rotationSpeed = 5f;

    private float lastAttackTime;
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
       agent = GetComponent<NavMeshAgent>();
       agent.speed = speed; 
    }

    // Update is called once per frame
    void Update()
    {
        if (playerPosition == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, playerPosition.position);

        if (distance > followDistance)
        {
            agent.SetDestination(playerPosition.position);
        }
        else
        {
            agent.SetDestination(transform.position);
        }

        RotateTowardsPlayer();

        if (distance <= attackRange && Time.time > lastAttackTime + attackCooldown)
        {
            AttackPlayer();
            lastAttackTime = Time.time;
        }
    }

        private void RotateTowardsPlayer()
    {
        Vector3 directionToPlayer = playerPosition.position - magicSpawnAI.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        magicSpawnAI.rotation = Quaternion.Slerp(magicSpawnAI.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }


private void AttackPlayer()
{
    // Instantiate the magic projectile at the spawn point, using magicSpawnAI's rotation
    GameObject magic = Instantiate(magicProjectilePrefab, magicSpawnAI.position, magicSpawnAI.rotation);

    // Get the MagicAttack component from the magic projectile
    MagicAttack magicAttack = magic.GetComponent<MagicAttack>();
    if (magicAttack != null)
    {
        // Calculate the direction from the magic spawn to the player
        Vector3 direction = (playerPosition.position - magicSpawnAI.position).normalized;

        // Initialize the projectile with the direction towards the player
        magicAttack.Initialize(direction);
    }

    // Destroy the projectile after 3 seconds to clean up
    Destroy(magic, 3f);
}



}
