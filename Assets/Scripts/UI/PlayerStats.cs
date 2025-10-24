using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    public float maxHP = 100f;
    public float maxMP = 50f;
    public float currentHP;
    public float currentMP;

    void Awake()
    {
        currentHP = maxHP;
        currentMP = maxMP;
    }

    // Hàm nhận sát thương
    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP < 0f) currentHP = 0f;
    }

    // Hàm hồi máu
    public void Heal(float amount)
    {
        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP;
    }

    // Hàm tiêu thụ mana
    public bool UseMana(float amount)
    {
        if (currentMP >= amount)
        {
            currentMP -= amount;
            return true;
        }
        return false;
    }

    // Hàm hồi mana
    public void RecoverMana(float amount)
    {
        currentMP += amount;
        if (currentMP > maxMP) currentMP = maxMP;
    }
}
