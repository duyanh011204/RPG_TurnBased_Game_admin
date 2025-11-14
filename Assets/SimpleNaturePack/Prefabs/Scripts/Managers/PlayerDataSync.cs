using UnityEngine;

public class PlayerDataSync : MonoBehaviour
{
    void Start()
    {
        if (GameManager.Instance == null) return;

        PlayerData data = GameManager.Instance.playerData;
        PlayerStats stats = FindObjectOfType<PlayerStats>();

        if (stats != null)
        {
            stats.maxHP = data.maxHP;
            stats.currentHP = data.maxHP;
            stats.maxMP = data.maxMP;
            stats.currentMP = data.maxMP;
            stats.speed = data.speed;

            Debug.Log("✅ PlayerDataSync: Combat scene stats updated from PlayerData");
        }
    }
}
