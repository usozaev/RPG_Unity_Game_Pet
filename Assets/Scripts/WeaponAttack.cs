using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{

    public Animator animator;
    public int totalAttacks = 2; // Number of different swing animations
    private int attackIndex = 0;
    public float attackCooldown = 0.5f;
    private bool canAttack = true;
    private Rigidbody rb;
    public float damageAmount = 10;


    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && canAttack)
        {
            TriggerAttackAnimation();
        }
    }

    void TriggerAttackAnimation()
    {
        animator.SetInteger("AttackIndex", attackIndex);
        animator.SetTrigger("isAttacking");

        attackIndex = (attackIndex +1) % totalAttacks;
        canAttack = false;
        Invoke(nameof(ResetAttack), attackCooldown);
    }

private void ResetAttack()
{
    canAttack = true; // Allow next attack
    animator.SetBool("isIdle", true);
}
void ReturnToIdle()
{
    animator.SetInteger("AttackIndex", -1); // Reset attack index
    animator.ResetTrigger("isAttacking");  // Ensure attack trigger resets
}

    private void OnTriggerEnter(Collider other)
{
    Debug.Log("Hit detected on: " + other.gameObject.name); // Debug log

    UnitHealth targetHealth = other.gameObject.GetComponentInParent<UnitHealth>();

    // Get current animation state
    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    int currentStateHash = stateInfo.shortNameHash;

    // Expected animation states
    int expectedStateHash1 = Animator.StringToHash("swordSwing" + attackIndex);
    int expectedStateHash2 = Animator.StringToHash("swordSwing2"); // Alternative animation

    // Debugging
    bool animationMatch = (currentStateHash == expectedStateHash1 || currentStateHash == expectedStateHash2);
    Debug.Log("Current Animator State Hash: " + currentStateHash);
    Debug.Log("Expected State Hash 1: " + expectedStateHash1);
    Debug.Log("Expected State Hash 2: " + expectedStateHash2);
    Debug.Log("Animation Match Status: " + (animationMatch ? "MATCH" : "NO MATCH"));

    if (targetHealth != null && animationMatch)
    {
        Debug.Log("Applying damage!"); // Debug log
        targetHealth.takeDamage(damageAmount);
    }
}

}
