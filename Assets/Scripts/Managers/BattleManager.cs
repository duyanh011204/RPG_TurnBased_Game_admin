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
        
        private EnemyAI3D enemyInstance3DComp;
        private GameObject enemyInstance3D;
        private Vector3 lastPlayerPosition;
        private bool playerTurn;
        private bool playerHasActed = false;
        private bool enemyHasActed = false;
        private int turnCount = 1;

        void Start()
        {
            lastPlayerPosition = BattleStartData.LastPlayerPosition;
            playerTurn = BattleStartData.PlayerFirst;
            StartCoroutine(SpawnEnemyAndStartBattle());
        }

        IEnumerator SpawnEnemyAndStartBattle()
        {
            enemyInstance3D = enemyPrefab3D;
            yield return null;

            // Set target cho PlayerStrikeUI
            PlayerStrikeUI strikeUI = playerInstance.GetComponent<PlayerStrikeUI>();
            if (strikeUI != null)
                strikeUI.SetEnemyTarget(enemyInstance3D.transform);

            // Enemy setup
            if (enemyInstance3D != null)
            {
                EnemyAI3D ai3D = enemyInstance3D.GetComponent<EnemyAI3D>();
                if (ai3D != null)
                {
                    ai3D.currentHP = ai3D.maxHP;
                    ai3D.currentMP = startingMP; // Enemy bắt đầu với 10/50 MP
                }
            }

            // Player setup
            if (playerInstance != null)
            {
                PlayerStats playerStats = playerInstance.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.currentHP = playerStats.maxHP;
                    playerStats.currentMP = startingMP; // Player bắt đầu với 10/50 MP
                }
            }

            UpdateUI();
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(StartBattle());
        }


        IEnumerator StartBattle()
        {
            yield return new WaitForSeconds(2f);
            turnCounterText.text = "Turn: " + turnCount;
            StartPlayerTurn();
        }

        void StartPlayerTurn()
        {
            playerTurn = true;
            playerHasActed = false;
            enemyHasActed = false;
            panelAction.SetActive(true);
            panelSkill.SetActive(false);
            panelItem.SetActive(false);
        }

        IEnumerator EnemyTurn()
        {
            playerTurn = false;
            panelAction.SetActive(false);
            panelSkill.SetActive(false);
            panelItem.SetActive(false);

            yield return new WaitForSeconds(1f);

            EnemyAI3D enemyAI = enemyInstance3D.GetComponent<EnemyAI3D>();
            PlayerStats playerStats = playerInstance.GetComponent<PlayerStats>();

            if (enemyAI != null && playerStats != null)
            {
                enemyAI.PerformAttack(playerStats); // trừ mana ngay khi dùng skill
                UpdateUI();
                playerStats.ResetGuard();
            }

            yield return new WaitForSeconds(1f);

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
                Debug.Log("Escape successful!");
                StartCoroutine(EscapeBattle());
            }
            else
            {
                Debug.Log("Escape failed!");
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
            EnemyAI3D enemyAI = enemyInstance3D.GetComponent<EnemyAI3D>();

            if ((playerStats != null && playerStats.currentHP <= 0) ||
                (enemyAI != null && enemyAI.currentHP <= 0))
            {
                StartCoroutine(EndBattle());
                return;
            }

            if (playerHasActed && !enemyHasActed)
            {
                StartCoroutine(EnemyTurn());
            }
            else if (playerHasActed && enemyHasActed)
            {
                // Kết thúc turn: cộng 10 mana cho cả player và enemy
                if (playerStats != null) playerStats.RecoverMana(10f);
                if (enemyAI != null) enemyAI.RecoverMana(10f);
                if (playerStats != null) playerStats.ProcessPoison();

                turnCount++;
                turnCounterText.text = "Turn: " + turnCount;
                StartPlayerTurn();
                UpdateUI();
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
                SceneManager.LoadScene("GameWorld");
            });
        }



    public void UpdateUI()
        {
            PlayerStats playerStats = playerInstance.GetComponent<PlayerStats>();
            EnemyAI3D enemyAI = enemyInstance3D.GetComponent<EnemyAI3D>();

            if (playerStats != null)
            {
                if (playerHPBar != null) playerHPBar.value = playerStats.currentHP / playerStats.maxHP;
                if (playerMPBar != null) playerMPBar.value = playerStats.currentMP / playerStats.maxMP;

                if (playerHPText != null)
                    playerHPText.text = $" {Mathf.CeilToInt(playerStats.currentHP)}/{Mathf.CeilToInt(playerStats.maxHP)}";
                if (playerMPText != null)
                    playerMPText.text = $" {Mathf.CeilToInt(playerStats.currentMP)}/{Mathf.CeilToInt(playerStats.maxMP)}";
            }

            if (enemyAI != null)
            {
                if (enemyHPBar != null) enemyHPBar.value = enemyAI.currentHP / enemyAI.maxHP;
                if (enemyMPBar != null) enemyMPBar.value = enemyAI.currentMP / enemyAI.maxMP;

                if (enemyHPText != null)
                    enemyHPText.text = $" {Mathf.CeilToInt(enemyAI.currentHP)}/{Mathf.CeilToInt(enemyAI.maxHP)}";
                if (enemyMPText != null)
                    enemyMPText.text = $" {Mathf.CeilToInt(enemyAI.currentMP)}/{Mathf.CeilToInt(enemyAI.maxMP)}";
            }
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


        public void OnEnemyDefeated(EnemyAI3D enemy)
        {
            if (enemy != null && enemyInstance3D == enemy)
                enemyInstance3D = null;          // tránh crash UpdateUI
            if (enemy != null && enemyInstance3DComp == enemy.GetComponent<EnemyAI3D>())
                enemyInstance3DComp = null;      // tránh crash UpdateUI

            // Kết thúc battle nếu enemy chết
            StartCoroutine(EndBattle());
        }
        
        

}
