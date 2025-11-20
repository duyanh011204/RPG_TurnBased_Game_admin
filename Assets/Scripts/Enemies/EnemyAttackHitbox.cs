using UnityEngine;

public class EnemyAttackHitbox : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string targetTag = "Player";
    [SerializeField] private string battleSceneName = "CombatScene";
    [SerializeField] private bool attackerGoesFirst = false;

    private bool triggered = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (triggered) return;

        if (collision.CompareTag(targetTag))
        {
            triggered = true;

            if (BattleStartData.TryStartBattle(attackerGoesFirst))
            {
                BattleStartData.LastPlayerPosition = collision.transform.position;
                SceneTransitions.LoadScene(battleSceneName);
            }
        }
    }
}