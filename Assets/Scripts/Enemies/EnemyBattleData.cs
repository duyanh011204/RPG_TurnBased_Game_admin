using System.Collections.Generic;
using UnityEngine;


public static class EnemyBattleData
{
  
    private static Dictionary<string, float> defeatedEnemies = new Dictionary<string, float>();

  
    public static void SetDead(string enemyID)
    {
        if (string.IsNullOrEmpty(enemyID)) return;
        defeatedEnemies[enemyID] = Time.realtimeSinceStartup;
    }

    public static bool IsDead(string enemyID)
    {
        if (string.IsNullOrEmpty(enemyID)) return false;
        return defeatedEnemies.ContainsKey(enemyID);
    }

    public static float GetDeathTime(string enemyID)
    {
        if (string.IsNullOrEmpty(enemyID)) return 0f;
        if (!defeatedEnemies.ContainsKey(enemyID)) return 0f;
        return defeatedEnemies[enemyID];
    }

    public static void MarkAlive(string enemyID)
    {
        if (string.IsNullOrEmpty(enemyID)) return;
        if (defeatedEnemies.ContainsKey(enemyID))
            defeatedEnemies.Remove(enemyID);
    }

    
    public static void AddDefeated(string enemyID)
    {
        if (string.IsNullOrEmpty(enemyID)) return;
       
        defeatedEnemies[enemyID] = Time.realtimeSinceStartup;
    }

    public static bool IsDefeated(string enemyID)
    {
     
        return IsDead(enemyID);
    }

 
    public static void Reset()
    {
        defeatedEnemies.Clear();
    }

 
    public static void ResetEnemy(string enemyID)
    {
        MarkAlive(enemyID);
    }
}
