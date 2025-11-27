using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    [Header("References (Scene Objects)")]
    [SerializeField] private GameObject playerInstance;
    [SerializeField] private GameObject enemyPrefab3D;
    [SerializeField] private GameObject enemy2DReference;
    [SerializeField] private Transform enemySpawn;
    [SerializeField] private float startingMP = 10f;

    [Header("UI Panels")]
    [SerializeField] private GameObject panelAction;
    [SerializeField] private GameObject panelSkill;
    [SerializeField] private GameObject panelItem;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private Button backToHomeButton;

    [Header("UI Elements")]
    [SerializeField] private Slider playerHPBar;
    [SerializeField] private Slider playerMPBar;
    [SerializeField] private Slider enemyHPBar;
    [SerializeField] private Slider enemyMPBar;
    [SerializeField] private TMP_Text turnCounterText;
    [SerializeField] private TMP_Text playerHPText;
    [SerializeField] private TMP_Text playerMPText;
    [SerializeField] private TMP_Text enemyHPText;
    [SerializeField] private TMP_Text enemyMPText;

    [Header("Enemy Settings")]
    [SerializeField] private MonoBehaviour[] allEnemiesInCombat;
    private MonoBehaviour currentEnemy;
  



    private EnemyAI3D enemyInstance3DComp;
    private GameObject enemyInstance3D;
    private Vector3 lastPlayerPosition;
    private bool playerTurn;
    private bool playerHasActed = false;
    private bool enemyHasActed = false;
    private int turnCount = 1;
    private bool playerFirstTurn;


    void Start()
    {

        lastPlayerPosition = BattleStartData.LastPlayerPosition;
        playerFirstTurn = BattleStartData.PlayerFirst;
        playerTurn = playerFirstTurn;
        StartCoroutine(SpawnEnemyAndStartBattle());
    }

    IEnumerator SpawnEnemyAndStartBattle()
    {
        string enemyIDFrom2D = BattleStartData.SelectedEnemyID;

        foreach (var enemy in allEnemiesInCombat)
        {
            string id = "";
            if (enemy is EnemyAI3D e3d) id = e3d.enemyID;
            else if (enemy is EnemyAI3DKing king) id = king.enemyID;

            if (id == enemyIDFrom2D)
            {
                enemy.gameObject.SetActive(true);
                enemyInstance3D = enemy.gameObject;
                currentEnemy = enemy;
            }
            else
            {
                enemy.gameObject.SetActive(false);
            }
        }

        if (currentEnemy != null)
        {
            if (currentEnemy is EnemyAI3D e3d)
            {
                e3d.currentHP = e3d.maxHP;
                e3d.currentMP = startingMP;
            }
            else if (currentEnemy is EnemyAI3DKing king)
            {
                king.currentHP = king.maxHP;
                king.currentMP = startingMP;
            }
        }

      
        PlayerStrikeUI strikeUI = playerInstance.GetComponent<PlayerStrikeUI>();
        if (strikeUI != null && enemyInstance3D != null)
            strikeUI.SetEnemyTarget(enemyInstance3D.transform);

        UpdateUI();
        yield return new WaitForSeconds(1f);
        StartCoroutine(StartBattle());
    }




    IEnumerator StartBattle()
    {
        PlayerStats playerStats = playerInstance.GetComponent<PlayerStats>();
        if (playerStats != null)
            playerStats.ResetGuard();

        playerHasActed = false;
        enemyHasActed = false;

        yield return new WaitForSeconds(2f);
        turnCounterText.text = "Turn: " + turnCount;

        if (playerFirstTurn)
            StartPlayerTurn();
        else
            StartCoroutine(EnemyTurn());
    }


    void StartPlayerTurn()
    {
        playerTurn = true;
        playerHasActed = false;
        panelAction.SetActive(true);
        panelSkill.SetActive(false);
        panelItem.SetActive(false);

        PlayerStats playerStats = playerInstance.GetComponent<PlayerStats>();
        if (playerStats != null)
            playerStats.ResetGuard();
    }




    IEnumerator EnemyTurn()
    {
        playerTurn = false;
        enemyHasActed = false;
        panelAction.SetActive(false);
        panelSkill.SetActive(false);
        panelItem.SetActive(false);

        yield return new WaitForSeconds(1f);

        PlayerStats playerStats = playerInstance.GetComponent<PlayerStats>();
        if (enemyInstance3D != null && playerStats != null)
        {
            if (enemyInstance3D.GetComponent<EnemyAI3D>() is EnemyAI3D e3d)
                e3d.PerformAttack(playerStats);
            else if (enemyInstance3D.GetComponent<EnemyAI3DKing>() is EnemyAI3DKing king)
                king.PerformAttack(playerStats);
        }

        UpdateUI();

        yield return new WaitForSeconds(0.5f);

        enemyHasActed = true;
        CheckTurnEnd();
    }


    public void OnAttackButton()
    {
        panelAction.SetActive(false);
        panelSkill.SetActive(true);
    }

    public void OnItemButton()
    {
        panelAction.SetActive(false);
        panelItem.SetActive(true);
    }

    public void OnItemBackButton()
    {
        panelItem.SetActive(false);
        panelAction.SetActive(true);
    }

    public void OnStrikeButton()
    {
        PlayerStrikeUI strikeUI = playerInstance.GetComponent<PlayerStrikeUI>();
        if (strikeUI != null)
            strikeUI.OnStrikeButton();

        panelSkill.SetActive(false);
    }

    public void OnBackButton()
    {
        panelSkill.SetActive(false);
        panelAction.SetActive(true);
    }

    public void OnMeditateButton()
    {
        PlayerStats playerStats = playerInstance.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.RecoverMana(10f);
            UpdateUI();
        }

        playerHasActed = true;
        panelAction.SetActive(false);
        panelSkill.SetActive(false);
        CheckTurnEnd();
    }

    public void OnGuardButton()
    {
        PlayerStats playerStats = playerInstance.GetComponent<PlayerStats>();
        if (playerStats != null)
            playerStats.ActivateGuard();

        playerHasActed = true;
        panelAction.SetActive(false);
        panelSkill.SetActive(false);
        CheckTurnEnd();
    }

    public void OnEscapeButton()
    {
        float chance = Random.value;
        if (chance <= 0.25f)
        {
            StartCoroutine(EscapeBattle());
        }
        else
        {
            playerHasActed = true;
            panelAction.SetActive(false);
            panelSkill.SetActive(false);
            CheckTurnEnd();
        }
    }

    private IEnumerator EscapeBattle()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("GameWorld");

        yield return new WaitForSeconds(0.2f);
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerObj.transform.position = lastPlayerPosition;

            EnemyAI2D[] enemies = FindObjectsOfType<EnemyAI2D>();
            foreach (var enemy in enemies)
                enemy.StartCoroutine(enemy.SetPlayerInvisible(10f));
        }
    }

    public void EndTurn()
    {
        PlayerStats playerStats = playerInstance.GetComponent<PlayerStats>();
        EnemyAI3D enemyAI = enemyInstance3D.GetComponent<EnemyAI3D>();

        if ((playerStats != null && playerStats.currentHP <= 0) ||
            (enemyAI != null && enemyAI.currentHP <= 0))
        {
            StartCoroutine(EndBattle());
            return;
        }

        playerHasActed = true;
        CheckTurnEnd();
    }

    private void CheckTurnEnd()
    {
        PlayerStats playerStats = playerInstance.GetComponent<PlayerStats>();
        EnemyAI3D enemyAI = enemyInstance3D?.GetComponent<EnemyAI3D>();

        if ((playerStats != null && playerStats.currentHP <= 0) ||
            (enemyAI != null && enemyAI.currentHP <= 0))
        {
            StartCoroutine(EndBattle());
            return;
        }

        if (!playerHasActed || !enemyHasActed)
        {
            if (!playerHasActed)
                StartPlayerTurn();
            else if (!enemyHasActed)
                StartCoroutine(EnemyTurn());
        }
        else
        {
            playerStats.RecoverMana(10f);
            enemyAI?.RecoverMana(10f);
            playerStats.ProcessPoison();

            turnCount++;
            turnCounterText.text = "Turn: " + turnCount;

            playerHasActed = false;
            enemyHasActed = false;

            
            if (playerFirstTurn)
                StartPlayerTurn();
            else
                StartCoroutine(EnemyTurn());
        }
    }


    IEnumerator EndBattle()
    {
        PlayerStats playerStats = playerInstance.GetComponent<PlayerStats>();
        EnemyAI3D enemyAI = enemyInstance3D != null ? enemyInstance3D.GetComponent<EnemyAI3D>() : null;

        yield return new WaitForSeconds(1f);

        panelAction.SetActive(false);
        panelSkill.SetActive(false);
        panelItem.SetActive(false);

        resultPanel.SetActive(true);

        if (enemyAI == null || enemyAI.currentHP <= 0)
            resultText.text = "Victory";
        else if (playerStats != null && playerStats.currentHP <= 0)
            resultText.text = "Defeat";

        backToHomeButton.onClick.RemoveAllListeners();
        backToHomeButton.onClick.AddListener(() =>
        {
            if (enemyInstance3D != null)
            {

                EnemyAI3D enemyAI = enemyInstance3D.GetComponent<EnemyAI3D>();
                if (enemyAI != null)
                {
                    enemyAI.currentHP = enemyAI.maxHP;
                    enemyAI.currentMP = startingMP;
                }
            }

            PlayerStats playerStats = playerInstance.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                PlayerData data = new PlayerData
                {
                    attack = playerStats.attackBonus,
                    maxHP = (int)playerStats.maxHP,
                    speed = (int)playerStats.speed,
                    maxMP = (int)playerStats.maxMP,
                    level = playerStats.level,
                    exp = playerStats.exp,
                    points = playerStats.points,
                    expToNextLevel = playerStats.expToNextLevel
                };
                SaveSystem.SaveData(data);
            }

            BattleStartData.ResetBattleFlag();
            SceneManager.LoadScene("GameWorld");
        });



    }

    public void UpdateUI()
    {
        PlayerStats playerStats = playerInstance?.GetComponent<PlayerStats>();

        float enemyCurrentHP = 0f;
        float enemyMaxHP = 1f;
        float enemyCurrentMP = 0f;
        float enemyMaxMP = 1f;

        if (enemyInstance3D != null)
        {
            if (enemyInstance3D.GetComponent<EnemyAI3D>() is EnemyAI3D e3d)
            {
                enemyCurrentHP = e3d.currentHP;
                enemyMaxHP = e3d.maxHP;
                enemyCurrentMP = e3d.currentMP;
                enemyMaxMP = e3d.maxMP;
            }
            else if (enemyInstance3D.GetComponent<EnemyAI3DKing>() is EnemyAI3DKing king)
            {
                enemyCurrentHP = king.currentHP;
                enemyMaxHP = king.maxHP;
                enemyCurrentMP = king.currentMP;
                enemyMaxMP = king.maxMP;
            }
        }

        if (playerStats != null)
        {
            if (playerHPBar != null) playerHPBar.value = playerStats.currentHP / playerStats.maxHP;
            if (playerMPBar != null) playerMPBar.value = playerStats.currentMP / playerStats.maxMP;
            if (playerHPText != null) playerHPText.text = $" {Mathf.CeilToInt(playerStats.currentHP)}/{Mathf.CeilToInt(playerStats.maxHP)}";
            if (playerMPText != null) playerMPText.text = $" {Mathf.CeilToInt(playerStats.currentMP)}/{Mathf.CeilToInt(playerStats.maxMP)}";
        }

        if (enemyHPBar != null) enemyHPBar.value = enemyCurrentHP / enemyMaxHP;
        if (enemyMPBar != null) enemyMPBar.value = enemyCurrentMP / enemyMaxMP;
        if (enemyHPText != null) enemyHPText.text = $" {Mathf.CeilToInt(enemyCurrentHP)}/{Mathf.CeilToInt(enemyMaxHP)}";
        if (enemyMPText != null) enemyMPText.text = $" {Mathf.CeilToInt(enemyCurrentMP)}/{Mathf.CeilToInt(enemyMaxMP)}";
    }




    public void DamagePlayer(float amount)
    {
        if (playerInstance == null) return;
        PlayerStats playerStats = playerInstance.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.TakeDamage(amount);
            UpdateUI();
        }
    }

    public bool PlayerTurn => playerTurn;


    public void OnEnemyDefeated(MonoBehaviour enemy)
    {
        if (enemy == null) return;

        if (enemy is EnemyAI3D e3d)
        {
            EnemyBattleData.SetDead(e3d.enemyID);
            e3d.gameObject.SetActive(false);
            playerInstance.GetComponent<PlayerStats>()?.AddExp(e3d.expReward);
        }
        else if (enemy is EnemyAI3DKing king)
        {
            EnemyBattleData.SetDead(king.enemyID);
            king.gameObject.SetActive(false);
            playerInstance.GetComponent<PlayerStats>()?.AddExp(king.expReward);
        }

        playerHasActed = true;
        StartCoroutine(EndBattle());
    }

}
  