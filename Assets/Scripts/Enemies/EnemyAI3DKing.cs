using UnityEngine;

public class EnemyAI3DKing : MonoBehaviour
{
    public string enemyID;

    [Header("Reward")]
    public int expReward = 200;

    [Header("Stats")]
    public float maxHP = 500f;
    public float currentHP;
    public float maxMP = 50f;
    public float currentMP;

    [Header("Combat Settings")]
    public float attackDamage = 20f;

    public float skillDamage = 20f;
    public float skillCost = 20f;
    public float skillChance = 1f;
    public int skillCooldownTurns = 2;
    private int currentSkillCooldown = 0;

    public float extraSkill1Damage = 15f;
    public float extraSkill1Cost = 10f;
    public float extraSkill1Chance = 0.3f;
    public int extraSkill1CooldownTurns = 3;
    private int extraSkill1CurrentCooldown = 0;

    public float extraSkill2Damage = 25f;
    public float extraSkill2Cost = 15f;
    public float extraSkill2Chance = 0.2f;
    public int extraSkill2CooldownTurns = 4;
    private int extraSkill2CurrentCooldown = 0;

    public Animator animator;
    private bool isDead = false;

    void Start()
    {
        currentHP = maxHP;
        currentMP = maxMP;
        if (animator == null) animator = GetComponent<Animator>();
    }

    public void PerformAttack(PlayerStats playerStats)
    {
        if (isDead || playerStats == null) return;

        if (currentSkillCooldown > 0) currentSkillCooldown--;
        if (extraSkill1CurrentCooldown > 0) extraSkill1CurrentCooldown--;
        if (extraSkill2CurrentCooldown > 0) extraSkill2CurrentCooldown--;

        bool useSkill = currentMP >= skillCost && currentSkillCooldown == 0 && Random.value < skillChance;
        bool useExtra1 = currentMP >= extraSkill1Cost && extraSkill1CurrentCooldown == 0 && Random.value < extraSkill1Chance;
        bool useExtra2 = currentMP >= extraSkill2Cost && extraSkill2CurrentCooldown == 0 && Random.value < extraSkill2Chance;

        if (animator != null)
        {
            if (useExtra2) animator.SetTrigger("Ulti");
            else if (useExtra1) animator.SetTrigger("Roll");
            else if (useSkill) animator.SetTrigger("Shoot");
            else animator.SetTrigger("Attack");
        }

        if (useExtra2)
        {
            UseMana(extraSkill2Cost);
            playerStats.TakeDamage(extraSkill2Damage);
            extraSkill2CurrentCooldown = extraSkill2CooldownTurns;
        }
        else if (useExtra1)
        {
            UseMana(extraSkill1Cost);
            playerStats.TakeDamage(extraSkill1Damage);
            extraSkill1CurrentCooldown = extraSkill1CooldownTurns;
        }
        else if (useSkill)
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
