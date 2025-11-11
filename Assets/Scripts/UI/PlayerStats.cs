using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    public int attackBonus = 0;
    public float maxHP = 100f;
    public float speed = 10f;
    public float maxMP = 50f;
    public float currentHP;
    public float currentMP;

    [Header("Combat")]
    public bool isDead = false;
    public bool isGuarding = false;
    private float guardReduction = 0.5f;

    public bool isPoisoned = false;
    public int poisonTurnsRemaining = 0;
    [SerializeField] private GameObject poisonDebuffIcon;
    [SerializeField] private TMP_Text poisonCounterText;

    void Awake()
    {
        PlayerData data = SaveSystem.LoadData();
        if (data != null)
        {
            attackBonus = data.attack;
            maxHP = data.maxHP;
            speed = data.speed;
            maxMP = data.maxMP;
        }
        else
        {
            maxHP = 100f;
            speed = 10f;
            maxMP = 50f;
        }

        currentHP = maxHP;
        currentMP = maxMP;
        isDead = false;
    }


    public void ApplyPlayerData()
    {
        if (GameManager.Instance != null)
        {
            PlayerData data = GameManager.Instance.playerData;
            maxHP = data.maxHP;
            maxMP = data.maxMP;
            speed = data.speed;
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        if (isGuarding) damage *= guardReduction;
        currentHP -= damage;

        if (currentHP <= 0f)
        {
            currentHP = 0f;
            isDead = true;
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;
        currentHP = Mathf.Min(currentHP + amount, maxHP);
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
        currentMP = Mathf.Min(currentMP + amount, maxMP);
    }

    public void ActivateGuard()
    {
        if (!isDead) isGuarding = true;
    }

    public void ResetGuard()
    {
        if (isGuarding) isGuarding = false;
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
            TakeDamage(1f);
            poisonTurnsRemaining--;
            if (poisonCounterText != null)
                poisonCounterText.text = poisonTurnsRemaining.ToString();
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
