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
        currentHP -= damage;
        if (currentHP < 0) currentHP = 0;

        BattleManager.Instance.UpdateUI();

        if (currentHP <= 0 && !isDead)
        {
            isDead = true;
            if (animator != null) animator.SetTrigger("Die");
            if (BattleManager.Instance != null)
                BattleManager.Instance.OnEnemyDefeated(this);
        }
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

    public void OnDieAnimationEnd()
    {
        if (BattleManager.Instance != null)
            BattleManager.Instance.OnEnemyDefeated(this);

        Destroy(gameObject);
    }
}
