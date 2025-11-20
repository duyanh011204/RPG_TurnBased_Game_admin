using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string targetTag = "Enemy";
    [SerializeField] private string battleSceneName = "CombatScene";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            if (BattleStartData.TryStartBattle(true))
            {
                BattleStartData.LastPlayerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
                SceneTransitions.LoadScene(battleSceneName);
            }
        }
    }
}