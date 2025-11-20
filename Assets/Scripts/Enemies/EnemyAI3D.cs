using UnityEngine;

public class EnemyAI3D : MonoBehaviour
{
    public string enemyID;

    [Header("Reward")]
    public int expReward = 50;

    [Header("Stats")]
    public float maxHP = 100f;
    public float currentHP;
    public float maxMP = 50f;
    public float currentMP;

    [Header("Combat Settings")]
    public float attackDamage = 10f;
    public float skillDamage = 20f;
    public float skillCost = 20f;
    public float skillChance = 1f;
    public int skillCooldownTurns = 2;

    public Animator animator;
    private bool isDead = false;
    private int currentSkillCooldown = 0;

    void Start()
    {
        currentHP = maxHP;
        currentMP = maxMP;
        if (animator == null) animator = GetComponent<Animator>();

    }

    public void PerformAttack(PlayerStats playerStats)
    {
        if (isDead || playerStats == null) return;

        if (currentSkillCooldown > 0)
            currentSkillCooldown--;

        bool canUseSkill = currentMP >= skillCost && currentSkillCooldown == 0 && Random.value < skillChance;

        if (animator != null)
        {
            if (canUseSkill) animator.SetTrigger("Shoot");
            else animator.SetTrigger("Attack");
        }

        if (canUseSkill)
        {
            UseMana(skillCost);
            playerStats.TakeDamage(skillDamage);
            playerStats.ApplyPoison(2);
            currentSkillCooldown = skillCooldownTurns;
        }
        else
        {
            playerStats.TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHP -= damage;
        if (currentHP < 0) currentHP = 0;

        BattleManager.Instance?.UpdateUI();

        if (currentHP <= 0)
        {
            isDead = true;

            EnemyBattleData.AddDefeated(enemyID);
            Debug.Log("Defeated Enemy ID: " + enemyID);


            if (animator != null)
                animator.SetTrigger("Die");

            BattleManager.Instance?.OnEnemyDefeated(this);
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
        Destroy(gameObject);
    }

    public void ActivateEnemy()
    {
        gameObject.SetActive(true);

        foreach (var s in GetComponents<MonoBehaviour>())
        {
            if (s != this) s.enabled = true;
        }

        if (animator != null)
            animator.enabled = true;
    }

    public void DeactivateEnemy()
    {
        foreach (var s in GetComponents<MonoBehaviour>())
        {
            if (s != this) s.enabled = false;
        }

        if (animator != null)
            animator.enabled = false;

        gameObject.SetActive(false);
    }

}

