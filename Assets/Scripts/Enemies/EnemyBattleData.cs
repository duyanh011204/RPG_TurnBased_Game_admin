using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central runtime enemy state manager.
/// H? tr? c? API c? (AddDefeated / IsDefeated) và API m?i (SetDead / IsDead / GetDeathTime / MarkAlive).
/// L?u tr?ng thái trong runtime (không ghi file).
/// </summary>
public static class EnemyBattleData
{
    // l?u enemyID -> deathTime (Time.realtimeSinceStartup) khi b? ?ánh b?i
    private static Dictionary<string, float> defeatedEnemies = new Dictionary<string, float>();

    // ---------------------------
    // API "m?i" (th?i gian ch?t)
    // ---------------------------
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

    // ---------------------------
    // API "c?" (kh?p v?i code tr??c ?ó)
    // gi? ?? t??ng thích
    // ---------------------------
    public static void AddDefeated(string enemyID)
    {
        // alias -> ch? ?ánh d?u (không l?u th?i gian)
        // ?? t??ng thích v?i code g?i AddDefeated previously.
        if (string.IsNullOrEmpty(enemyID)) return;
        // l?u th?i gian ngay lúc g?i (behaviour gi?ng SetDead)
        defeatedEnemies[enemyID] = Time.realtimeSinceStartup;
    }

    public static bool IsDefeated(string enemyID)
    {
        // alias cho IsDead
        return IsDead(enemyID);
    }

    // ---------------------------
    // Reset / Clear
    // ---------------------------
    public static void Reset()
    {
        defeatedEnemies.Clear();
    }

    // ti?n ích: remove single enemy (alias)
    public static void ResetEnemy(string enemyID)
    {
        MarkAlive(enemyID);
    }
}
