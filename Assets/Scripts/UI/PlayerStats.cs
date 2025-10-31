using UnityEngine;
using TMPro;


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

    public bool isPoisoned = false;
    public int poisonTurnsRemaining = 0;
    [SerializeField] private GameObject poisonDebuffIcon; // link icon UI
    [SerializeField] private TMP_Text poisonCounterText;

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

    public void ApplyPoison(int turns)
    {
        isPoisoned = true;
        poisonTurnsRemaining = turns;
        if (poisonDebuffIcon != null)
            poisonDebuffIcon.SetActive(true);
        if (poisonCounterText != null)
        {
            poisonCounterText.gameObject.SetActive(true);
            poisonCounterText.text = poisonTurnsRemaining.ToString();
        }

    }

    public void ProcessPoison()
    {
        if (isPoisoned)
        {
            TakeDamage(1f); // giảm 1 HP mỗi turn
            poisonTurnsRemaining--;

            // update text UI
            if (poisonCounterText != null)
                poisonCounterText.text = poisonTurnsRemaining.ToString();

            // chỉ ẩn khi hết turn
            if (poisonTurnsRemaining <= 0)
                CurePoison();
        }
    }

    public void CurePoison()
    {
        isPoisoned = false;
        poisonTurnsRemaining = 0;
        if (poisonDebuffIcon != null)
            poisonDebuffIcon.SetActive(false);
        if (poisonCounterText != null)
            poisonCounterText.gameObject.SetActive(false);
    }
}
