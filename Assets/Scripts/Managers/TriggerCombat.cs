using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerCombat : MonoBehaviour
{
    public string battleSceneName = "BattleScene";

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Lưu vị trí Player trước khi load BattleScene
            Vector3 playerPosition = other.transform.position;
            BattleStartData.LastPlayerPosition = playerPosition;

            // Load scene Battle
            SceneManager.LoadScene(battleSceneName);
        }
    }
}
