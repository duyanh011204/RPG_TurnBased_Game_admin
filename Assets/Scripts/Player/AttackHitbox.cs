using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private string targetTag = "Enemy"; 
    [SerializeField] private string battleSceneName = "CombatScene"; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {

            if (BattleStartData.TryStartBattle(true))
            {
                SceneTransitions.LoadScene(battleSceneName);
            }

        }
    }
}
