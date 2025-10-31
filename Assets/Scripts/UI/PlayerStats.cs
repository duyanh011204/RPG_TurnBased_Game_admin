using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    public float maxHP = 100f;
    public float maxMP = 50f;
    public float currentHP;
    public float currentMP;

    [Header("Combat")]
    public bool isDead = false;
    public bool isGuarding = false;
    private float guardReduction = 0.5f; // giảm 50% damage khi Guard

    void Awake()
    {
        currentHP = maxHP;
        currentMP = maxMP;
        isDead = false;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        if (isGuarding)
        {
            damage *= guardReduction; // giảm 50%
            Debug.Log($"🛡️ Guard active! Damage reduced to {damage}");
        }

        currentHP -= damage;
        if (currentHP <= 0f)
        {
            currentHP = 0f;
            isDead = true;
            Debug.Log("💀 Player is dead!");
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
        if (isDead) return false;
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

    // Kích hoạt Guard
    public void ActivateGuard()
    {
        if (isDead) return;
        isGuarding = true;
        Debug.Log("🛡️ Guard activated! Damage will be reduced this turn.");
    }

    // Hủy Guard (sau lượt quái)
    public void ResetGuard()
    {
        if (isGuarding)
        {
            isGuarding = false;
            Debug.Log("🛡️ Guard ended. Damage back to normal.");
        }
    }
}
