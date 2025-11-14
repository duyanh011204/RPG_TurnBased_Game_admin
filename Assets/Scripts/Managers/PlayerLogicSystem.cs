using UnityEngine;

public static class PlayerLogicSystem
{
    public static PlayerData LoadOrCreate()
    {
        PlayerData data = SaveSystem.LoadData();
        if (data == null)
        {
            data = new PlayerData();
            SaveSystem.SaveData(data);
        }
        return data;
    }

    public static void AddExp(int amount)
    {
        PlayerData data = LoadOrCreate();

        if (data.level >= data.maxLevel) return;

        data.exp += amount;

        while (data.exp >= data.expToNextLevel && data.level < data.maxLevel)
        {
            data.exp -= data.expToNextLevel;
            data.level++;
            data.points += data.pointsPerLevel;
            data.expToNextLevel = Mathf.RoundToInt(data.expToNextLevel * 1.2f);
        }

        SaveSystem.SaveData(data);
    }

    public static void TakeDamage(int damage)
    {
        PlayerData data = LoadOrCreate();
        if (data.isDead) return;

        float actualDamage = damage;
        if (data.isGuarding) actualDamage *= 0.5f;

        data.currentHP -= (int)actualDamage;
        if (data.currentHP <= 0)
        {
            data.currentHP = 0;
            data.isDead = true;
        }

        SaveSystem.SaveData(data);
    }

    public static void Heal(int amount)
    {
        PlayerData data = LoadOrCreate();
        if (data.isDead) return;

        data.currentHP += amount;
        if (data.currentHP > data.maxHP) data.currentHP = data.maxHP;

        SaveSystem.SaveData(data);
    }

    public static void UseMana(int amount)
    {
        PlayerData data = LoadOrCreate();
        if (data.currentMP >= amount)
        {
            data.currentMP -= amount;
            SaveSystem.SaveData(data);
        }
    }

    public static void RecoverMana(int amount)
    {
        PlayerData data = LoadOrCreate();
        data.currentMP += amount;
        if (data.currentMP > data.maxMP) data.currentMP = data.maxMP;

        SaveSystem.SaveData(data);
    }

    public static void ActivateGuard()
    {
        PlayerData data = LoadOrCreate();
        data.isGuarding = true;
        SaveSystem.SaveData(data);
    }

    public static void ResetGuard()
    {
        PlayerData data = LoadOrCreate();
        data.isGuarding = false;
        SaveSystem.SaveData(data);
    }

    public static void ApplyPoison(int turns)
    {
        PlayerData data = LoadOrCreate();
        data.isPoisoned = true;
        data.poisonTurnsRemaining = turns;
        SaveSystem.SaveData(data);
    }

    public static void ProcessPoison()
    {
        PlayerData data = LoadOrCreate();
        if (!data.isPoisoned) return;

        TakeDamage(1);
        data.poisonTurnsRemaining--;
        if (data.poisonTurnsRemaining <= 0)
        {
            data.isPoisoned = false;
            data.poisonTurnsRemaining = 0;
        }
        SaveSystem.SaveData(data);
    }
}
