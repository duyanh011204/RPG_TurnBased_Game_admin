﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class BattleManager : MonoBehaviour
{
    [Header("References (Scene Objects)")]
    [SerializeField] private GameObject playerInstance;
    [SerializeField] private GameObject enemyPrefab3D;
    [SerializeField] private GameObject enemy2DReference;
    [SerializeField] private Transform enemySpawn;

    [Header("UI Panels")]
    [SerializeField] private GameObject panelAction;
    [SerializeField] private GameObject panelSkill;

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

    private GameObject enemyInstance3D;
    private Vector3 lastPlayerPosition;
    private bool playerTurn;
    private int turnCount = 1;

    void Start()
    {
        lastPlayerPosition = BattleStartData.LastPlayerPosition;
        playerTurn = BattleStartData.PlayerFirst;
        StartCoroutine(SpawnEnemyAndStartBattle());
    }
﻿    using UnityEngine;
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

        [Header("UI Panels")]
        [SerializeField] private GameObject panelAction;
        [SerializeField] private GameObject panelSkill;
        [SerializeField] private GameObject panelItem;

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

            PlayerStrikeUI strikeUI = playerInstance.GetComponent<PlayerStrikeUI>();
            if (strikeUI != null)
                strikeUI.SetEnemyTarget(enemyInstance3D.transform);

            if (enemyInstance3D != null && enemy2DReference != null)
            {
                EnemyAI3D ai3D = enemyInstance3D.GetComponent<EnemyAI3D>();
                EnemyAI2D ai2D = enemy2DReference.GetComponent<EnemyAI2D>();
                if (ai3D != null && ai2D != null && ai2D.currentHP > 0)
                    ai3D.currentHP = ai2D.currentHP;
                else if (ai3D != null)
                    ai3D.currentHP = ai3D.maxHP;
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
                enemyAI.PerformAttack(playerStats);
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
                StartCoroutine(EnemyTurn());
            else if (playerHasActed && enemyHasActed)
            {
                turnCount++;
                turnCounterText.text = "Turn: " + turnCount;
                StartPlayerTurn();
            }
        }

        IEnumerator EndBattle()
        {
            EnemyAI3D enemyAI = enemyInstance3D.GetComponent<EnemyAI3D>();

            if (enemyAI != null && enemyAI.currentHP <= 0 && enemy2DReference != null)
            {
                EnemyAI2D enemy2D = enemy2DReference.GetComponent<EnemyAI2D>();
                if (enemy2D != null)
                {
                    Animator anim = enemy2DReference.GetComponent<Animator>();
                    if (anim != null)
                        anim.SetTrigger("Die");

                    if (enemy2DReference.scene.IsValid())
                        Destroy(enemy2DReference, 1.5f);
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
