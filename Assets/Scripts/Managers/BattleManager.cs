using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class BattleManager : MonoBehaviour
{
    [Header("Prefabs & References")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab3D;
    [SerializeField] private GameObject enemy2DReference;
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private Transform enemySpawn;

    [Header("UI Panels")]
    [SerializeField] private GameObject panelAction;
    [SerializeField] private GameObject panelSkill;

    [Header("UI Elements")]
    [SerializeField] private Slider playerHPBar;
    [SerializeField] private Slider playerMPBar;
    [SerializeField] private Slider enemyHPBar;
    [SerializeField] private TMP_Text turnCounterText;

    private GameObject playerInstance;
    private GameObject enemyInstance3D;
    private Vector3 lastPlayerPosition;
    private bool playerTurn;
    private int turnCount = 1;

    void Start()
    {
        lastPlayerPosition = BattleStartData.LastPlayerPosition;
        playerTurn = BattleStartData.PlayerFirst;

        SpawnCombatants();
        UpdateUI();
        StartCoroutine(StartBattle());
    }

    void SpawnCombatants()
    {
        playerInstance = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
        enemyInstance3D = Instantiate(enemyPrefab3D, enemySpawn.position, Quaternion.identity);

        if (enemyInstance3D != null && enemy2DReference != null)
        {
            EnemyAI3D ai3D = enemyInstance3D.GetComponent<EnemyAI3D>();
            EnemyAI2D ai2D = enemy2DReference.GetComponent<EnemyAI2D>();
            if (ai3D != null && ai2D != null)
                ai3D.currentHP = ai2D.currentHP;
        }
    }

    IEnumerator StartBattle()
    {
        yield return new WaitForSeconds(0.5f);
        turnCounterText.text = "Turn: " + turnCount;

        if (playerTurn)
            StartPlayerTurn();
        else
            StartCoroutine(EnemyTurn());
    }

    void StartPlayerTurn()
    {
        playerTurn = true;
        panelAction.SetActive(true);
        panelSkill.SetActive(false);
    }

    IEnumerator EnemyTurn()
    {
        playerTurn = false;
        panelAction.SetActive(false);
        panelSkill.SetActive(false);

        // Delay trước khi Enemy attack (nhìn Player chuẩn bị)
        yield return new WaitForSeconds(0.5f);

        EnemyAI3D enemyAI = enemyInstance3D.GetComponent<EnemyAI3D>();
        PlayerStats playerStats = playerInstance.GetComponent<PlayerStats>();

        if (enemyAI != null && playerStats != null)
        {
            enemyAI.PerformAttack(playerStats); // Enemy đánh
            UpdateUI();                         // cập nhật thanh HP Player
        }

        // Delay sau khi Enemy attack để player thấy HP giảm và animation
        yield return new WaitForSeconds(0.5f);

        EndTurn(); // chuyển lượt
    }

    public void OnAttackButton()
    {
        panelAction.SetActive(false);
        panelSkill.SetActive(true);
    }

    public void OnStrikeButton()
    {
        EnemyAI3D enemyAI = enemyInstance3D.GetComponent<EnemyAI3D>();
        if (enemyAI != null)
        {
            enemyAI.TakeDamage(20f);
            if (enemyAI.animator != null) enemyAI.animator.SetTrigger("Hit");
        }

        UpdateUI();
        panelSkill.SetActive(false);
        EndTurn();
    }

    public void OnBackButton()
    {
        panelSkill.SetActive(false);
        panelAction.SetActive(true);
    }

    public void OnItemButton() { Debug.Log("Item button pressed"); }
    public void OnMeditateButton() { Debug.Log("Meditate button pressed"); }
    public void OnGuardButton() { Debug.Log("Guard button pressed"); }
    public void OnEscapeButton() { Debug.Log("Escape button pressed"); }

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

        playerTurn = !playerTurn;
        turnCount++;
        turnCounterText.text = "Turn: " + turnCount;

        if (playerTurn)
            StartPlayerTurn();
        else
            StartCoroutine(EnemyTurn());
    }

    IEnumerator EndBattle()
    {
        EnemyAI3D enemyAI = enemyInstance3D.GetComponent<EnemyAI3D>();

        if (enemyAI != null && enemyAI.currentHP <= 0 && enemy2DReference != null)
        {
            EnemyAI2D enemy2D = enemy2DReference.GetComponent<EnemyAI2D>();
            if (enemy2D != null)
            {
                enemy2D.TakeDamage(9999);
                Animator anim = enemy2DReference.GetComponent<Animator>();
                if (anim != null) anim.SetTrigger("Die");
                Destroy(enemy2DReference, 1f);
            }
        }

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("GameWorld");
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            playerObj.transform.position = lastPlayerPosition;
    }

    public void UpdateUI()
    {
        PlayerStats playerStats = playerInstance.GetComponent<PlayerStats>();
        EnemyAI3D enemyAI = enemyInstance3D.GetComponent<EnemyAI3D>();

        if (playerStats != null)
        {
            if (playerHPBar != null) playerHPBar.value = playerStats.currentHP / playerStats.maxHP;
            if (playerMPBar != null) playerMPBar.value = playerStats.currentMP / playerStats.maxMP;
        }

        if (enemyAI != null && enemyHPBar != null)
            enemyHPBar.value = enemyAI.currentHP / enemyAI.maxHP;
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
}
