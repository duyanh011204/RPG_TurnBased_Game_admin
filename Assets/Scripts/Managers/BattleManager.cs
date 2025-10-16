using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;
    private GameObject playerInstance;
    private GameObject enemyInstance;

    private bool playerTurn;

    void Start()
    {
        playerTurn = BattleStartData.PlayerAdvantage;
        SpawnCombatants();
        StartBattle();
    }

    void SpawnCombatants()
    {
        playerInstance = Instantiate(playerPrefab, new Vector3(-2, 0, 0), Quaternion.identity);
        enemyInstance = Instantiate(enemyPrefab, new Vector3(2, 0, 0), Quaternion.identity);
    }

    void StartBattle()
    {
        if (playerTurn)
            StartCoroutine(PlayerFirstTurn());
        else
            StartCoroutine(EnemyFirstTurn());
    }

    System.Collections.IEnumerator PlayerFirstTurn()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Player goes first!");
    }

    System.Collections.IEnumerator EnemyFirstTurn()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Enemy goes first!");
    }
}
