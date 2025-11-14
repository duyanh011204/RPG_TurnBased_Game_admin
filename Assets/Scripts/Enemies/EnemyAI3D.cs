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
    public float skillDamage = 20f;
    public float skillCost = 20f;
    public float skillChance = 100f; // 30%
    public int skillCooldownTurns = 2;

    public Animator animator;
    private bool isDead = false;
    private int currentSkillCooldown = 0;

    void Start()
    {
        currentHP = maxHP;
        currentMP = maxMP;
        animator = GetComponent<Animator>();
    }

    public void PerformAttack(PlayerStats playerStats)
    {
        if (isDead || playerStats == null) return;

        if (currentSkillCooldown > 0)
            currentSkillCooldown--;

        Debug.Log("Trying to use skill. MP: " + currentMP + ", Cooldown: " + currentSkillCooldown + ", Chance: " + Random.value);


        bool canUseSkill = currentMP >= skillCost && currentSkillCooldown == 0 && Random.value < skillChance;

        if (canUseSkill)
        {
            UseMana(skillCost);
            Debug.Log("Enemy used skill, remaining MP: " + currentMP);

            if (BattleManager.Instance != null)
                BattleManager.Instance.UpdateUI();

            animator.SetTrigger("Shoot");
            playerStats.TakeDamage(skillDamage);

            // Gây độc 2 turn
            Debug.Log("🔥 Enemy used Poison skill!");
            playerStats.ApplyPoison(2);

            currentSkillCooldown = skillCooldownTurns;
        }
        else
        {
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
        if (isDead) return;

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
