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

    [Header("Level & EXP")]
    public int level = 1;
    public int maxLevel = 10;
    public int exp = 0;
    public int expToNextLevel = 100;
    public int points = 0;
    public int pointsPerLevel = 5;

    [Header("Combat")]
    public bool isDead = false;
    public bool isGuarding = false;
    private float guardReduction = 0.5f;

    public bool isPoisoned = false;
    public int poisonTurnsRemaining = 0;
    [SerializeField] private GameObject poisonDebuffIcon;
    [SerializeField] private TMP_Text poisonCounterText;

    [Header("UI")]
    public TMP_Text levelText;
    public TMP_Text expText;


    void Awake()
    {
        PlayerData data = null;

        if (GameManager.Instance != null && GameManager.Instance.playerData != null)
        {
            data = GameManager.Instance.playerData;
            Debug.Log("✅ Loaded player data from GameManager");
        }
       
        else
        {
            data = SaveSystem.LoadData();
            if (data != null)
                Debug.Log("✅ Loaded player data from JSON");
        }

      
        if (data != null)
        {
            attackBonus = data.attack;
            maxHP = data.maxHP;
            speed = data.speed;
            maxMP = data.maxMP;
            level = data.level;
            exp = data.exp;
            points = data.points;
        }
        else
        {
           
            maxHP = 100f;
            speed = 10f;
            maxMP = 50f;
            level = 1;
            exp = 0;
            points = 0;
        }

      
        currentHP = maxHP;
        currentMP = maxMP;
        isDead = false;

  
        UpdateLevelUI();
        UpdateExpUI();
       
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


    public void AddExp(int amount)
    {
        if (level >= maxLevel) return;
        exp += amount;
        UpdateExpUI();

        while (exp >= expToNextLevel && level < maxLevel)
        {
            exp -= expToNextLevel;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        points += pointsPerLevel;
        expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.2f);
        UpdateLevelUI();
        UpdateExpUI();
    }

    private void UpdateLevelUI()
    {
        if (levelText != null)
            levelText.text = "Player Level " + level;
    }

    private void UpdateExpUI()
    {
        if (expText != null)
            expText.text = exp + "/" + expToNextLevel;
    }

   
}
