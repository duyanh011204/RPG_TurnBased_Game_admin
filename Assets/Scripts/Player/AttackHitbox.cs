using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string targetTag = "Enemy";
    [SerializeField] private string battleSceneName = "CombatScene";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyAI2D enemy = collision.GetComponent<EnemyAI2D>();
        if (enemy != null)
        {
            BattleStartData.SelectedEnemyID = collision.GetComponent<EnemyAI2D>().enemyID.ToString();
            BattleStartData.LastPlayerPosition = transform.position;
            SceneTransitions.LoadScene("CombatScene");
        }
    }

}