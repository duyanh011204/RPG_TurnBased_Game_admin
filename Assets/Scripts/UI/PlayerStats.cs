using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    public float maxHP = 100f;
    public float maxMP = 50f;
    public float currentHP;
    public float currentMP;
    public float defenseMultiplier = 1f;
    public bool isDead = false;

    void Awake()
    {
        currentHP = maxHP;
        currentMP = maxMP;
    }

    public void TakeDamage(float damage)
    {
        damage /= defenseMultiplier;
        currentHP -= damage;
        if (currentHP < 0f) currentHP = 0f;
    }

    public void Heal(float amount)
    {
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

    public IEnumerator ApplyGuardBuff(int turns)
    {
        defenseMultiplier = 1.5f;
        Debug.Log("Guard activated! Defense +50%");
        yield return new WaitForSeconds(3f);
        defenseMultiplier = 1f;
        Debug.Log("Guard ended.");
    }
}
