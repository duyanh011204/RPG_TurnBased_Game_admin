using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatPanelSystem : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI healthText;

    public Button attackPlusButton;
    public Button speedPlusButton;
    public Button healthPlusButton;
    public Button resetButton;
    public Button confirmButton;

    [Header("Settings")]
    public int pointsPerLevel = 5;

    // Temp stats để test khi click plus/reset
    private int tempLevel;
    private int tempPoints;
    private int tempAttack;
    private int tempSpeed;
    private int tempHealth;

    // Stats gốc để reset
    private int baseLevel;
    private int basePoints;
    private int baseAttack;
    private int baseSpeed;
    private int baseHealth;

    void Start()
    {
        // Khởi tạo ban đầu
        baseLevel = 0;
        basePoints = 0;
        baseAttack = 0;
        baseSpeed = 10;
        baseHealth = 100;

        // Khi play -> nâng level 1
        tempLevel = baseLevel + 1;
        tempPoints = pointsPerLevel;
        tempAttack = baseAttack;
        tempSpeed = baseSpeed;
        tempHealth = baseHealth;

        UpdateUI();

        // Gán các nút
        attackPlusButton.onClick.AddListener(() => IncreaseStat(ref tempAttack));
        speedPlusButton.onClick.AddListener(() => IncreaseStat(ref tempSpeed));
        healthPlusButton.onClick.AddListener(() => IncreaseStat(ref tempHealth));
        resetButton.onClick.AddListener(ResetStats);
        confirmButton.onClick.AddListener(SaveStats);
        // confirmButton tạm bỏ, logic confirm sau
    }

    void IncreaseStat(ref int stat, int increment = 1)
    {
        if (tempPoints > 0)
        {
            stat += increment;
            tempPoints--;
            UpdateUI();
        }
    }

    void ResetStats()
    {
        tempLevel = baseLevel + 1; // level 1
        tempPoints = pointsPerLevel;
        tempAttack = baseAttack;
        tempSpeed = baseSpeed;
        tempHealth = baseHealth;
        SaveSystem.DeleteSave();
        UpdateUI();
    }

    void SaveStats()
    {
        PlayerData data = new PlayerData();
        data.attack = tempAttack; // attackBonus
        data.speed = tempSpeed;
        data.maxHP = tempHealth;
        data.level = tempLevel;
        data.points = tempPoints;
        SaveSystem.SaveData(data);

        Debug.Log("✅ StatPanelSystem: Stats saved with attack bonus " + tempAttack);
    }


    void UpdateUI()
    {
        if (levelText != null) levelText.text = $"STAT LV{tempLevel}";
        if (pointsText != null) pointsText.text = $"Points:{tempPoints}";
        if (attackText != null) attackText.text = $"Attack:{tempAttack}";
        if (speedText != null) speedText.text = $"Speed:{tempSpeed}";
        if (healthText != null) healthText.text = $"Health:{tempHealth}";

        bool canAdd = tempPoints > 0;
        attackPlusButton.interactable = canAdd;
        speedPlusButton.interactable = canAdd;
        healthPlusButton.interactable = canAdd;
    }
}
    