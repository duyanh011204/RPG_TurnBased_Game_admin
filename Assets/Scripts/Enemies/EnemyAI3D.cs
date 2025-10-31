using UnityEngine;
public class EnemyAI3D : MonoBehaviour
{
    [Header("Stats")]
    public float maxHP = 100f;
    public float currentHP;
    public float maxMP = 50f;
    public float currentMP;

    [Header("Combat Settings")]
    public float attackDamage = 10f;
    public float skillCost = 10f;

    public Animator animator;
    private bool isDead = false;

    void Start()
    {
        currentHP = maxHP;
        currentMP = maxMP;
        animator = GetComponent<Animator>();
    }

    public void PerformAttack(PlayerStats playerStats)
    {
        if (isDead || playerStats == null) return;
        if (currentMP >= skillCost) // giả sử tấn công bằng skill tiêu tốn mana
        {
            UseMana(skillCost);
            animator.SetTrigger("Attack");
            playerStats.TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHP -= damage;
        if (currentHP <= 0f) Die();
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP;
    }

    public bool UseMana(float amount)
    {
        if (currentMP >= amount)
        {
            currentMP -= amount;
            return true;
        }
        return false;
    }

    public void RecoverMana(float amount)
    {
        currentMP += amount;
        if (currentMP > maxMP) currentMP = maxMP;
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        Destroy(gameObject, 1f);
    }
}
