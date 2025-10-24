using UnityEngine;

public class EnemyAI3D : MonoBehaviour
{
    [Header("Stats")]
    public float maxHP = 100f;
    public float currentHP;

    [Header("References")]
    public Animator animator;
    public Rigidbody rb;

    [Header("Combat Settings")]
    public float attackDamage = 10f;

    private bool isDead = false;

    void Start()
    {
        currentHP = maxHP;
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (animator == null) animator = GetComponent<Animator>();
    }

    public void PerformAttack(PlayerStats playerStats)
    {
        if (isDead || playerStats == null) return;

        if (animator != null) animator.SetTrigger("Attack");
        playerStats.TakeDamage(attackDamage);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHP -= damage;
        if (currentHP <= 0f) Die();
    }

    void Die()
    {
        isDead = true;
        if (animator != null) animator.SetTrigger("Die");
        if (rb != null) rb.velocity = Vector3.zero;
        Destroy(gameObject, 1f);
    }
}
