using UnityEngine;
using UnityEngine.SceneManagement;

public class EncounterManager : MonoBehaviour
{
    public static EncounterManager Instance { get; private set; }
    [SerializeField] private string battleSceneName = "BattleScene";
    private bool isTransitioning = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartEncounter(bool playerAdvantage)
    {
        if (isTransitioning) return;
        isTransitioning = true;

        BattleStartData.PlayerAdvantage = playerAdvantage;
        StartCoroutine(LoadBattleScene());
    }

    System.Collections.IEnumerator LoadBattleScene()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(battleSceneName);
        isTransitioning = false;
    }
}
